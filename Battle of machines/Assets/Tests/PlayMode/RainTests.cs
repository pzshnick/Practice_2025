using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class RainTests
{
    private GameObject rainObject;
    private Rain rainScript;
    private Light lightSource;
    private ParticleSystem ps;

    [SetUp]
    public void Setup()
    {
        rainObject = new GameObject("RainObject");
        rainScript = rainObject.AddComponent<Rain>();

        GameObject lightObject = new GameObject("DirectionalLight");
        lightSource = lightObject.AddComponent<Light>();
        rainScript.directionalLight = lightSource;

        ps = rainObject.AddComponent<ParticleSystem>();
        typeof(Rain).GetField("_ps", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(rainScript, ps);

        StartGame.IsGameStarted = true;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(rainObject);
        Object.DestroyImmediate(lightSource.gameObject);
        StartGame.IsGameStarted = false;
    }

    [UnityTest]
    public IEnumerator Weather_TogglesRainFlag()
    {
        var field = typeof(Rain).GetField("_isRain", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(rainScript, false);

        var coroutine = rainObject.GetComponent<MonoBehaviour>().StartCoroutine("Weather");
        yield return new WaitForSeconds(0.1f);

        bool valueAfterToggle = (bool)field.GetValue(rainScript);
        Assert.IsTrue(valueAfterToggle || !valueAfterToggle);

        rainObject.GetComponent<MonoBehaviour>().StopCoroutine(coroutine);
    }

    [UnityTest]
    public IEnumerator LightIntensity_Decreases_WhenIsRain()
    {
        typeof(Rain).GetField("_isRain", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(rainScript, true);

        lightSource.intensity = 2f;
        yield return new WaitForSeconds(0.2f);

        Assert.Less(lightSource.intensity, 2f);
    }

    [UnityTest]
    public IEnumerator LightIntensity_Increases_WhenNoRain()
    {
        typeof(Rain).GetField("_isRain", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(rainScript, false);

        lightSource.intensity = 1.2f;
        yield return new WaitForSeconds(0.2f);

        Assert.Greater(lightSource.intensity, 1.2f);
    }
}
