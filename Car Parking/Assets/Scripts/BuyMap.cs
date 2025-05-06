using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyMap : MonoBehaviour
{
    public Text CoinsCount;
    public void Buy(int price)
    {
        if (PlayerPrefs.GetInt("Count") > price)
        {
            PlayerPrefs.SetInt("Count", PlayerPrefs.GetInt("Count") - price);
            PlayerPrefs.SetString("CurrentMap", "Cyber");
            CoinsCount.text = Convert.ToString(Convert.ToInt32(CoinsCount.text) - price);
        }
    }
}
