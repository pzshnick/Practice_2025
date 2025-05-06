using UnityEngine;
using UnityEngine.UI;

public class SetMoney : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Count").ToString();
    }
}
