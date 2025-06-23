using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;
using System.Linq.Expressions;

public class StartGameTests
{
    private GameObject obj;
    private StartGame script;
    private GameObject logo, playButton, shelterButton, detailedInfo;
    private Text machinesCount, enemiesCount, sheltersCount, gameEndText;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject("StartGameObject");
        script = obj.AddComponent<StartGame>();

        logo = new GameObject("Logo");
        playButton = new GameObject("PlayButton");
        shelterButton = new GameObject("ShelterButton");
        detailedInfo = new GameObject("DetailedInfo");

        machinesCount = CreateUIText("MachinesCount");
        enemiesCount = CreateUIText("EnemiesCount");
        sheltersCount = CreateUIText("SheltersCount");
        gameEndText = CreateUIText("GameEndText");

        script.Logo = logo;
        script.PlayButton = playButton;
        script.ShelterButton = shelterButton;
        script.DetailedInfo = detailedInfo;
        script.MachinesCount = machinesCount;
        script.EnemiesCount = enemiesCount;
        script.SheltersCount = sheltersCount;
        script.GameEndText = gameEndText;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
        Object.DestroyImmediate(logo);
        Object.DestroyImmediate(playButton);
        Object.DestroyImmediate(shelterButton);
        Object.DestroyImmediate(detailedInfo);
        Object.DestroyImmediate(machinesCount.gameObject);
        Object.DestroyImmediate(enemiesCount.gameObject);
        Object.DestroyImmediate(sheltersCount.gameObject);
        Object.DestroyImmediate(gameEndText.gameObject);
    }

    private Text CreateUIText(string name)
    {
        var go = new GameObject(name);
        return go.AddComponent<Text>();
    }

    [UnityTest]
    public IEnumerator PlayGame_SetsCorrectState()
    {
        StartGame.IsGameFinished = false;
        script.PlayGame();

        yield return null;

        Assert.IsTrue(StartGame.IsGameStarted);
        Assert.IsFalse(logo.activeSelf);
        Assert.AreEqual(Vector3.zero, playButton.transform.localScale);
        Assert.IsTrue(shelterButton.activeSelf);
        Assert.IsTrue(detailedInfo.activeSelf);
    }

    [UnityTest]
    public IEnumerator EndGame_ShowsWinUI()
    {
        typeof(StartGame)
            .GetField("gameEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(script, GameEnd.win);

        script.EndGame();
        yield return null;

        Assert.IsTrue(StartGame.IsGameFinished);
        Assert.AreEqual("You win!", gameEndText.text);
        Assert.IsTrue(logo.activeSelf);
        Assert.IsTrue(gameEndText.gameObject.activeSelf);
        Assert.AreEqual(new Vector3(1, 1, 0), playButton.transform.localScale);
        Assert.IsFalse(shelterButton.activeSelf);
        Assert.IsFalse(detailedInfo.activeSelf);
    }

    [UnityTest]
    public IEnumerator UpdateUI_ChangesBattleState_WhenConditionsMet()
    {
        StartGame.IsGameStarted = true;
        StartGame.machines = 2;
        StartGame.enemies = 2;
        StartGame.shelters = 1;
        shelterButton.SetActive(true);

        var coroutine = script.GetType()
            .GetMethod("UpdateUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(script, null) as IEnumerator;

        script.StartCoroutine(coroutine);

        yield return new WaitForSeconds(0.3f);

        Assert.IsTrue(StartGame.IsBattleStarted);
        Assert.AreEqual("2", machinesCount.text);
        Assert.AreEqual("2", enemiesCount.text);
        Assert.AreEqual("1", sheltersCount.text);
    }

    [UnityTest]
    public IEnumerator UpdateUI_TriggersGameEnd_WhenNoMachines()
    {
        StartGame.IsGameStarted = true;
        StartGame.machines = 0;
        StartGame.enemies = 2;
        StartGame.IsBattleStarted = true;

        var coroutine = script.GetType()
            .GetMethod("UpdateUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(script, null) as IEnumerator;

        script.StartCoroutine(coroutine);

        yield return new WaitForSeconds(0.3f);

        Assert.AreEqual("You lose...", gameEndText.text);
        Assert.IsTrue(StartGame.IsGameFinished);
    }
}
