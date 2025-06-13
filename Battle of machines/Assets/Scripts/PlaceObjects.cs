using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    public LayerMask layer;
    public float rotateSpeed = 60.0f;

    public void Start()
    {
        PositionObject();
    }

    /*
        The scenes of object positioning, rotation, and alignment 
        with the mouse ray are updated every frame — that’s exactly 
        why the Update() method is used.
    */
    private void Update()
    {
        PositionObject();

        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<BattleMachineAutocreate>().enabled = true;
            Destroy(gameObject.GetComponent<PlaceObjects>());
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }

    /*
        Using a ray (cast from the camera to the mouse pointer) 
        to accurately position an object on a specified layer (Ground) 

        Physics.Raycast(ray, out hit, 1000f, layer) is a check 
        to see if the ray hits an object on the specified layer 
        within a distance of up to 1000 units.
    */
    private void PositionObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, layer))
        {
            transform.position = hit.point;
        }
    }

    
}
