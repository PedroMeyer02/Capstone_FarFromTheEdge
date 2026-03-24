using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneMenu : MonoBehaviour
{

    public void MainMenu()
    {
        SceneManager.LoadScene("00_MenuScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
