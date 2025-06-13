using UnityEngine;

public class ButtonPlaceBuilding : MonoBehaviour
{
    /* 
       Field that stores the object that we
       are building (previously called through the UI interface) 
    */
    public GameObject building;

    // Set the called object on the map
    public void PlaceBuild()
    {
        Instantiate(building, Vector3.zero, Quaternion.identity);
    }
}
