using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class CameraControllerTests
{
    private GameObject cameraObject;
    private CameraController cameraController;

    [SetUp]
    public void Setup()
    {
        cameraObject = new GameObject("MainCamera");
        cameraController = cameraObject.AddComponent<CameraController>();

        cameraController.speed = 10f;
        cameraController.rotateSpeed = 90f;
        cameraController.zoomSpeed = 20f;

        cameraObject.transform.position = Vector3.zero;
        StartGame.IsGameStarted = true;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(cameraObject);
        StartGame.IsGameStarted = false;
    }

    [UnityTest]
    public IEnumerator Camera_TranslateForward_Manually()
    {
        Vector3 direction = Vector3.forward * cameraController.speed * Time.deltaTime;
        cameraObject.transform.Translate(direction, Space.Self);

        yield return null;

        Assert.Greater(cameraObject.transform.position.z, 0);
    }

    [UnityTest]
    public IEnumerator Camera_RotatesLeft_Manually()
    {
        Quaternion start = cameraObject.transform.rotation;

        cameraObject.transform.Rotate(Vector3.up * cameraController.rotateSpeed * Time.deltaTime, Space.World);

        yield return null;

        Assert.AreNotEqual(start, cameraObject.transform.rotation);
    }

    [UnityTest]
    public IEnumerator Camera_ZoomsIn_Manually()
    {
        float startY = cameraObject.transform.position.y;

        cameraObject.transform.position += cameraObject.transform.up * cameraController.zoomSpeed * Time.deltaTime;

        yield return null;

        Assert.Greater(cameraObject.transform.position.y, startY);
    }
}
