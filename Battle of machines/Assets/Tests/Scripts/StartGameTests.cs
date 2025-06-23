namespace DefaultNamespace;

public class StartGameTests
{
    private StartGame startGame;
    private GameObject go;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject();
        startGame = go.AddComponent<StartGame>();

        startGame.Logo = new GameObject();
        startGame.PlayButton = new GameObject();
        startGame.ShelterButton = new GameObject();
        startGame.DetailedInfo = new GameObject();

        startGame.MachinesCount = go.AddComponent<Text>();
        startGame.EnemiesCount = go.AddComponent<Text>();
        startGame.SheltersCount = go.AddComponent<Text>();
        startGame.GameEndText = go.AddComponent<Text>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    [UnityTest]
    public IEnumerator PlayGame_SetsInitialState()
    {
        StartGame.IsGameFinished = false;
        startGame.PlayGame();

        yield return null;

        Assert.IsTrue(StartGame.IsGameStarted);
        Assert.IsFalse(StartGame.IsGameFinished);
        Assert.IsFalse(StartGame.IsBattleStarted);
        Assert.IsFalse(startGame.Logo.activeSelf);
        Assert.AreEqual(new Vector3(0, 0, 0), startGame.PlayButton.transform.localScale);
        Assert.IsTrue(startGame.ShelterButton.activeSelf);
        Assert.IsTrue(startGame.DetailedInfo.activeSelf);
        Assert.AreEqual(2, StartGame.shelters);
        Assert.AreEqual(0, StartGame.machines);
        Assert.AreEqual(0, StartGame.enemies);
    }

    [UnityTest]
    public IEnumerator EndGame_SetsWinText()
    {
        startGame.GameEndText.text = "";
        typeof(StartGame).GetField("gameEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(startGame, GameEnd.win);

        startGame.EndGame();

        yield return null;

        Assert.AreEqual("You win!", startGame.GameEndText.text);
        Assert.IsFalse(StartGame.IsGameStarted);
        Assert.IsTrue(StartGame.IsGameFinished);
        Assert.IsTrue(startGame.Logo.activeSelf);
        Assert.IsTrue(startGame.GameEndText.gameObject.activeSelf);
        Assert.AreEqual(new Vector3(1, 1, 0), startGame.PlayButton.transform.localScale);
        Assert.IsFalse(startGame.ShelterButton.activeSelf);
        Assert.IsFalse(startGame.DetailedInfo.activeSelf);
    }

    [UnityTest]
    public IEnumerator UpdateUI_EndsGameOnWin()
    {
        StartGame.IsGameStarted = true;
        StartGame.IsBattleStarted = true;
        StartGame.machines = 1;
        StartGame.enemies = 0;

        var gameEndField = typeof(StartGame).GetField("gameEnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var coroutine = startGame.StartCoroutine("UpdateUI");
        yield return new WaitForSeconds(0.3f);

        Assert.AreEqual(GameEnd.win, (GameEnd)gameEndField.GetValue(startGame));
        Assert.IsTrue(StartGame.IsGameFinished);

        startGame.StopCoroutine(coroutine);
    }
}