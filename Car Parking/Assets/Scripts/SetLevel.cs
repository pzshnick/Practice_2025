using System;
using UnityEngine;
using UnityEngine.UI;

public class SetLevel : MonoBehaviour
{

    [Serializable]
    public struct LevelList
    {
        public GameObject level;
        public int movesCount;
    }

    public LevelList[] levels;
    public Text movesCount, levelNumber;
    private LevelList _currentLevel;

    void Start()
    {
        int currentLevelNum = PlayerPrefs.GetInt("CurrentLevel");

        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
        }
        else if (currentLevelNum >= levels.Length)
        {
            _currentLevel = levels[levels.Length - 1];
            PlayerPrefs.SetInt("CurrentLevel", levels.Length - 1);
        }
        else
        {
            _currentLevel = levels[currentLevelNum];
            PlayerPrefs.SetInt("CurrentLevel", currentLevelNum);
        }

        _currentLevel.level.SetActive(true);

        movesCount.text = _currentLevel.movesCount.ToString();
        levelNumber.text = levelNumber.text + (PlayerPrefs.GetInt("CurrentLevel") + 1).ToString();
    }

}
