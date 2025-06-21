using UnityEngine;

public class StartGame : MonoBehaviour
{

    public static bool IsGameStarted = false;
    public GameObject Logo, PlayButton, ShelterButton;

    public void PlayGame()
    {
        IsGameStarted = true;
        Logo.SetActive(false);
        PlayButton.SetActive(false);
        ShelterButton.SetActive(true);
    }

}
