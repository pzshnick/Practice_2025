using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Base camera rotation speed field (pre-modifiable in the editor)
    public float rotateSpeed = 10.0f, speed = 10.0f, zoomSpeed = 100.0f;

    // When the Shift hotkey is pressed, the camera
    // speed will be accelerated by the value of this field.
    private float _MovementMult = 1f;

    // Render the current object (camera) every frame
    private void Update()
    {
        // 'A' or 'D' keys and <- or -> tracking
        float horizontal = Input.GetAxis("Horizontal");

        // Similar to the previous variable
        float vertical = Input.GetAxis("Vertical");

        // Track hotkeys to rotate the camera
        float rotate = 0f;

        // Left
        if (Input.GetKey(KeyCode.Q))
        {
            rotate = -1f;
        }

        // Right
        else if(Input.GetKey(KeyCode.E))
        {
            rotate = 1f;
        }

        // Condition for accelerating camera rotation
        _MovementMult = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
 
        /*
            Vector3.up => Y Axis
          
            Time.deltaTime => Same rotation speed 
            regardless of the number of frames per second 

            Space.World => Rotation relative to the 
            global world, not the current object
        */
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * rotate * _MovementMult, Space.World);
        
        // Moving around the world
        transform.Translate(new Vector3(horizontal, 0, vertical) * Time.deltaTime * _MovementMult * speed, Space.Self);

        // Zoom camera through mouse wheel
        transform.position += transform.up * zoomSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");

        // Allowable zoom values
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -10, 5), transform.position.z);
    }
}
