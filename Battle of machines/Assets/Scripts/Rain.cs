using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Rain : MonoBehaviour
{
    public Light directionalLight;

    private ParticleSystem _ps;
    private bool _isRain = false;

    private float weatherMaxLightVal = 3f;
    private float weatherMinLightVal = 1f;

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        StartCoroutine(Weather());
    }

    private void Update()
    {
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

    IEnumerator Weather()
    {
        while(true)
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
