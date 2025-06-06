using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool IsGameStarted = false;
    public GameObject StartGameButton, Title, LoseText, MovesCount, WinText, ShopImage;

    private bool _isLosedGame = false, _isWonGame = false;

    public void PlayGame()
    {
        if (!_isLosedGame && !_isWonGame)
        {
            IsGameStarted = true;
            StartGameButton.SetActive(false);
            Title.SetActive(false);
            MovesCount.SetActive(true);
            ShopImage.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void WinGame()
    {
        Title.SetActive(true);
        StartGameButton.SetActive(true);
        MovesCount.SetActive(false);
        WinText.SetActive(true);
        ShopImage.SetActive(true);

        _isWonGame = true;
        IsGameStarted = false;

        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
    }

    public void LoseGame()
    {
        _isLosedGame = true;
        IsGameStarted = false;

        StartGameButton.SetActive(true);
        MovesCount.SetActive(false);
        Title.SetActive(true);
        LoseText.SetActive(true);
        ShopImage.SetActive(true);
    }
}
