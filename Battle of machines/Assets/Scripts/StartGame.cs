using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleStatus
{
    Preparation,
    Battle
}

public enum GameEnd
{
    win,
    lose,
    draw
}

public class StartGame : MonoBehaviour
{
    public static bool IsGameStarted = false;
    public static bool IsGameFinished = false;
    public static bool IsBattleStarted = false;

    public GameObject Logo,
                      PlayButton,
                      ShelterButton,
                      DetailedInfo;

    public Text MachinesCount,
                EnemiesCount,
                SheltersCount;

    public Text GameEndText;

    public static int shelters;
    public static int machines = 0;
    public static int enemies = 0;

    private GameEnd gameEnd;

    public void PlayGame()
    {  
        if (!IsGameFinished)
        {
            IsBattleStarted = false;
            IsGameFinished = false;
            IsGameStarted = true;

            Logo.SetActive(false);
            PlayButton.transform.localScale = new Vector3(0, 0, 0);
            ShelterButton.SetActive(true);
            DetailedInfo.SetActive(true);

            shelters = 2;
            machines = 0;
            enemies = 0;

            StartCoroutine(UpdateUI());
        }
        else
        {
            IsBattleStarted = false;
            IsGameFinished = false;
            IsGameStarted = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void EndGame()
    {
        switch (gameEnd)
        {
            case GameEnd.win:
                GameEndText.text = "You win!";
                break;
            case GameEnd.lose:
                GameEndText.text = "You lose...";
                break;
            case GameEnd.draw:
                GameEndText.text = "Draw";
                break;
            default:
                GameEndText.text = "";
                break;
        }

        IsGameStarted = false;
        IsGameFinished = true;

        Logo.SetActive(true);
        GameEndText.gameObject.SetActive(true);
        PlayButton.transform.localScale = new Vector3(1, 1, 0);
        ShelterButton.SetActive(false);
        DetailedInfo.SetActive(false);
    }

    IEnumerator UpdateUI()
    {
        while (IsGameStarted)
        {
            yield return new WaitForSeconds(0.2f);

            if (shelters <= 0 && ShelterButton.activeInHierarchy)
            {
                ShelterButton.SetActive(false);
            }

            if (enemies > 1 && machines > 1 && !IsBattleStarted)
            {
                IsBattleStarted = true;
            }

            MachinesCount.text = machines.ToString();
            EnemiesCount.text = enemies.ToString();
            SheltersCount.text = shelters.ToString();

            if (IsBattleStarted)
            {
                if (enemies == 0 && machines > 0)
                {
                    gameEnd = GameEnd.win;
                    EndGame();
                }
                else if (machines == 0 && enemies > 0)
                {
                    gameEnd = GameEnd.lose;
                    EndGame();
                }
                else if (machines == 0 && enemies == 0)
                {
                    gameEnd = GameEnd.draw;
                    EndGame();
                }
            }
   
        }
    }

}