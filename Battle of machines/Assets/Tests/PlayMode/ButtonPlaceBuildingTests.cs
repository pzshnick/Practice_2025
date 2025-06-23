using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class ButtonPlaceBuildingTests
{
    private GameObject buttonObject;
    private ButtonPlaceBuilding buttonScript;
    private GameObject dummyBuilding;

    [SetUp]
    public void Setup()
    {
        buttonObject = new GameObject("Button");
        buttonScript = buttonObject.AddComponent<ButtonPlaceBuilding>();

        dummyBuilding = GameObject.CreatePrimitive(PrimitiveType.Cube);
        dummyBuilding.name = "BuildingPrefab";

        buttonScript.building = dummyBuilding;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(buttonObject);
        Object.DestroyImmediate(dummyBuilding);

        var existing = GameObject.Find("BuildingPrefab(Clone)");
        if (existing) Object.DestroyImmediate(existing);
    }

    [UnityTest]
    public IEnumerator PlaceBuild_SheltersMoreThanZero_BuildsAndDecrements()
    {
        StartGame.shelters = 2;

        buttonScript.PlaceBuild();
        yield return null;

        var spawned = GameObject.Find("BuildingPrefab(Clone)");
        Assert.IsNotNull(spawned);
        Assert.AreEqual(1, StartGame.shelters);
    }

    [UnityTest]
    public IEnumerator PlaceBuild_SheltersZero_DoesNotBuild()
    {
        StartGame.shelters = 0;

        buttonScript.PlaceBuild();
        yield return null;

        var spawned = GameObject.Find("BuildingPrefab(Clone)");
        Assert.IsNull(spawned);
        Assert.AreEqual(0, StartGame.shelters);
    }
}
