using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneToLoad;
    public Animator anim;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenCredits()
    {
        anim.SetTrigger("OpenCredits");
        Debug.Log("Triggered");
    }

    public void CloseCredits()
    {
        anim.SetTrigger("CloseCredits");
        Debug.Log("Triggered2");
    }

    public void OpenSettings()
    {
        anim.SetTrigger("OpenSettings");
        Debug.Log("Triggered");
    }

    public void CloseSettings()
    {
        anim.SetTrigger("CloseSettings");
        Debug.Log("Triggered2");
    }
}
