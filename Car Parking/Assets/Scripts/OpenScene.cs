using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    public void OpenNewScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
