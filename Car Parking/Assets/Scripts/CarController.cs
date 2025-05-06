using System;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private Rigidbody _rb;
    public float speed = 10f, finalspeed = 20f, rotatespeed = 250f;
    private bool isMoving;

    private float clickedPointX, clickedPointY;

    private enum CarRotation
    { 
       Vertical,
       Horizontal,
       None
    }

    private enum MoveDirection
    { 
        Right,
        Left,
        Top,
        Bottom,
        None
    }

    private MoveDirection carDirectionX = MoveDirection.None;
    private MoveDirection carDirectionY = MoveDirection.None;

    private CarRotation carRotation = CarRotation.None;

    [NonSerialized] public Vector3 FinalPosition;

    // For count variable
    public Text MovesCount, MoneyCount;
    public GameObject _GameController;

    private static int _carsCount = 0;

    // Sound effects
    private AudioSource _audioSource;
    public AudioClip CarStart, CarCrash;

    // Visual effect
    public ParticleSystem CrashEffect;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        // Recognize rotation of spawned car
        Vector3 rotation = transform.rotation.eulerAngles;
        if (Mathf.Abs(rotation.y - 0) < 10 || Mathf.Abs(rotation.y - 180) < 10)
        {
            carRotation = CarRotation.Horizontal;
        }
        else if (Mathf.Abs(rotation.y - 90) < 10 || Mathf.Abs(rotation.y - 270) < 10)
        {
            carRotation = CarRotation.Vertical;
        }

        _carsCount++;
    }

    private void OnMouseDown()
    {
        // If the user has not pressed the button, then the game will not start
        if (!GameController.IsGameStarted) return;

        clickedPointX = Input.mousePosition.x;
        clickedPointY = Input.mousePosition.y;
    }

    private void OnMouseUp()
    {
        // If the user has not pressed the button, then the game will not start
        if (!GameController.IsGameStarted) return;

        // X direction
        if (Input.mousePosition.x - clickedPointX > 0)
        {
            carDirectionX = MoveDirection.Right;
        }
        else
        {
            carDirectionX = MoveDirection.Left;
        }

        // Y direction
        if (Input.mousePosition.y - clickedPointY > 0)
        {
            carDirectionY = MoveDirection.Top;
        }
        else
        {
            carDirectionY = MoveDirection.Bottom;
        }

        // If the number of moves is zero, the game ends
        MovesCount.text = Convert.ToString(Convert.ToInt32(MovesCount.text) - 1);
        Debug.Log(_carsCount);

        if (MovesCount.text == "0" && _carsCount > 1 && !isMoving)
        {
            _GameController.GetComponent<GameController>().LoseGame();
        }

        isMoving = true;

        // Car start sound
        _audioSource.Stop();
        _audioSource.clip = CarStart;
        _audioSource.Play();
    }

    private void FixedUpdate()
    {
        // If the user has not pressed the button, then the game will not start
        if (!GameController.IsGameStarted) return;

        if (isMoving && FinalPosition.x == 0)
        {
            Vector3 moveTo = this.carRotation == CarRotation.Horizontal ? Vector3.forward : Vector3.left;
            float currentSpeed = speed;

            if (carDirectionX == MoveDirection.Left && carRotation == CarRotation.Horizontal)
            {
                currentSpeed *= -1;
            }
            
            if (carDirectionY == MoveDirection.Bottom && carRotation == CarRotation.Vertical)
            {
                currentSpeed *= -1;
            }

            _rb.MovePosition(_rb.position + moveTo * currentSpeed * Time.fixedDeltaTime);
        }
        else if (isMoving && FinalPosition.x != 0)
        {
            if (Vector3.Distance(transform.position, FinalPosition) < 0.2f)
            {
                PlayerPrefs.SetInt("Count", PlayerPrefs.GetInt("Count") + 1);
                MoneyCount.text = Convert.ToString(Convert.ToInt32(MoneyCount.text) + 1);

                Destroy(gameObject);
                _carsCount--;

                // Output win window
                if (_carsCount == 0)
                {
                    _GameController.GetComponent<GameController>().WinGame();
                }
                return;
            }

            Vector3 lookAtPos = FinalPosition - transform.position;
            lookAtPos.y = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookAtPos), rotatespeed * Time.fixedDeltaTime);

            transform.position = Vector3.MoveTowards(transform.position, FinalPosition, finalspeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerStay(Collider otherCar)
    {
        if (otherCar.CompareTag("Car") || otherCar.CompareTag("Barrier"))
        {
            // Create visual effect
            Destroy(
              Instantiate(CrashEffect, otherCar.ClosestPoint(transform.position), Quaternion.Euler(new Vector3(270f, 0, 0))),
            2.0f);

            if (carRotation == CarRotation.Horizontal && isMoving)
            {
                float returnDist = carDirectionX == MoveDirection.Left ? 0.5f : -0.5f;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + returnDist);
            } else if (carRotation == CarRotation.Vertical && isMoving)
            {
                float returnDist = carDirectionY == MoveDirection.Bottom ? -0.5f : 0.5f;
                transform.position = new Vector3(transform.position.x + returnDist, transform.position.y, transform.position.z);
            }

            // Car start sound
            if (_audioSource.clip != CarCrash)
            {            
                _audioSource.Stop();
                _audioSource.clip = CarCrash;
                _audioSource.Play();
            }

            isMoving = false;
        }
    }

}