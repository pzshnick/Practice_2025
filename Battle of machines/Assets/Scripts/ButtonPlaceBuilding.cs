using UnityEngine;

public class ButtonPlaceBuilding : MonoBehaviour
{
    public GameObject building;

    public void PlaceBuild()
    {
        Instantiate(building, Vector3.zero, Quaternion.identity);
    }
}
