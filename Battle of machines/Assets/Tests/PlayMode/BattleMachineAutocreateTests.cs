using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class BattleMachineAutocreateTests
{
    private GameObject hangarObject;
    private BattleMachineAutocreate script;
    private GameObject carPrefab;

    [SetUp]
    public void Setup()
    {
        hangarObject = new GameObject("Hangar");
        script = hangarObject.AddComponent<BattleMachineAutocreate>();

        // create a dummy child position (required via GetChild (0))
        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.SetParent(hangarObject.transform);
        spawnPoint.transform.localPosition = Vector3.zero;

        // create a dummy machine
        carPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        carPrefab.name = "DummyCar";

        script.car = carPrefab;

        // test acceleration
        script.time = 0.01f; 

        StartGame.IsGameStarted = true;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(hangarObject);
        Object.DestroyImmediate(carPrefab);

        foreach (var obj in GameObject.FindGameObjectsWithTag("Player"))
            Object.DestroyImmediate(obj);
        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
            Object.DestroyImmediate(obj);
    }

    [UnityTest]
    public IEnumerator SpawnsMachines_WhenIsEnemyFalse_IncrementsMachines()
    {
        StartGame.machines = 0;
        script.isEnemy = false;

        yield return new WaitForSeconds(0.05f);

        Assert.AreEqual(3, StartGame.machines);
    }

    [UnityTest]
    public IEnumerator SpawnsEnemies_WhenIsEnemyTrue_IncrementsEnemies()
    {
        StartGame.enemies = 0;
        script.isEnemy = true;

        yield return new WaitForSeconds(0.05f);

        Assert.AreEqual(3, StartGame.enemies);
    }
}
