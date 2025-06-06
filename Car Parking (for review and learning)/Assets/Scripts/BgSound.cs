using System.Collections;
using UnityEngine;

public class BgSound : MonoBehaviour
{
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        StartCoroutine(SoundBackground());
    }

    IEnumerator SoundBackground()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(15f, 20f));
            _audio.Play();
        }
    } 
}
