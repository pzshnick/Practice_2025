using UnityEngine;

public class CreateSoundObject : MonoBehaviour
{
    private static bool _isCreated = false;
    public GameObject soundObject;

    private void Start()
    {
        if (_isCreated) return;

        _isCreated = true;

        // The object will not be destroyed after reloading the scene
        DontDestroyOnLoad(Instantiate(soundObject));
    }
}
