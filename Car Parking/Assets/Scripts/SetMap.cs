using UnityEditor;
using UnityEngine;

public class SetMap : MonoBehaviour
{

    public GameObject city, cyber;

    void Start()
    {
        if (PlayerPrefs.GetString("CurrentMap") == "Cyber")
        {
            cyber.SetActive(true);
        }
        else
        {
            city.SetActive(true);
        }

    }

}
