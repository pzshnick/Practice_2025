using System;
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
        if (StartGame.shelters > 0)
        {
            Instantiate(building, Vector3.zero, Quaternion.identity);
            StartGame.shelters = StartGame.shelters - 1;
        }
    }
}
