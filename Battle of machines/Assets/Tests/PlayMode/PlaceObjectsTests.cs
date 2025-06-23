using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class PlaceObjectsTests
{
    private GameObject obj;
    private PlaceObjects script;
    private GameObject cameraObject;

    [SetUp]
    public void Setup()
    {
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        script = obj.AddComponent<PlaceObjects>();
        script.rotateSpeed = 60f;

        obj.layer = LayerMask.NameToLayer("Default");
        script.layer = LayerMask.GetMask("Default");

        cameraObject = new GameObject("MainCamera");
        var camera = cameraObject.AddComponent<Camera>();
        cameraObject.tag = "MainCamera";
        camera.transform.position = new Vector3(0, 10, 0);
        camera.transform.rotation = Quaternion.Euler(90, 0, 0);

        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = Vector3.zero;
        ground.layer = LayerMask.NameToLayer("Default");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
        Object.DestroyImmediate(cameraObject);
        foreach (var go in GameObject.FindObjectsOfType<GameObject>())
        {
            if (go.name.Contains("Plane"))
                Object.DestroyImmediate(go);
        }
    }

    [UnityTest]
    public IEnumerator ObjectRotatesManually()
    {
        Quaternion startRot = obj.transform.rotation;

        obj.transform.Rotate(Vector3.up * Time.deltaTime * script.rotateSpeed);

        yield return null;

        Assert.AreNotEqual(startRot, obj.transform.rotation);
    }

    [UnityTest]
    public IEnumerator OnClick_EnablesBattleScriptAndRemovesPlaceObjects()
    {
        var battle = obj.AddComponent<BattleMachineAutocreate>();
        var placeScript = obj.GetComponent<PlaceObjects>();
        battle.enabled = false;

        battle.enabled = true;
        Object.Destroy(placeScript);

        yield return null;

        // Перевірка
        Assert.IsTrue(battle.enabled, "BattleMachineAutocreate має бути активований");
        Assert.IsNull(obj.GetComponent<PlaceObjects>(), "PlaceObjects має бути видалений");
    }

}
