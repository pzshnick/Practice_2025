using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class Rain : MonoBehaviour
{
    /*
        For a visual effect light our main 
        map to reduce its intensity in a gloomy hike
    */
    public Light directionalLight;

    /*
         particle system is the main element for rain, which is 
         visually embedded in the Unity editor. Previously, 
         the 'isRain' flag is used for call conditions
    */
    private ParticleSystem _ps;
    private bool _isRain = false;

    // Lighting constants
    private float weatherMaxLightVal = 3f;
    private float weatherMinLightVal = 1f;

    /* 
       Start the weather process asynchronously 
       without blocking the main game stream 
    
       Coroutine is a simplified way to configure 
       the asynchronous operation of the game, unlike async/await
     */
    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        StartCoroutine(Weather());
    }

    /*
        Used to smoothly change the visual scene 
        of the game due to the fact that the Update 
        method is tied to the frame rate of the screen 
    */
    private void Update()
    {
        if (!StartGame.IsGameStarted) return;

        if (_isRain && directionalLight.intensity > weatherMinLightVal)
        {
            LightIntensity(-1);
        }
        else if (!_isRain && directionalLight.intensity < weatherMaxLightVal)
        {
            LightIntensity(1);
        }
    }

    private void LightIntensity(int mult)
    {
        directionalLight.intensity += 0.7f * Time.deltaTime * mult;
    }

    /*
        Implicit delegate that changes 
        weather conditions at specified intervals 
    */
    IEnumerator Weather()
    {
        yield return new WaitUntil(() => StartGame.IsGameStarted);

        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 20f));

            if (_isRain)
            {
                _ps.Stop();
            }
            else
            {
                _ps.Play();
            }

            _isRain = !_isRain;
        }
    }
}
