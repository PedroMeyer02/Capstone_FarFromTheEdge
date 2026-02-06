using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundLoad : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load in the background")]
    public string sceneToLoad;

    [Header("UI References")]
    //public Image loadingAnimationImage;
    //public Sprite[] animationSprites; // Should contain exactly 2 sprites
    public TMP_Text progressText;
    public TMP_Text pressAnyButtonText;
    //public GameObject progressBarContainer;

    [Header("Settings")]
    //public float animationSwitchInterval = 0.5f;
    public float fakeLoadingMinTime = 1f; // Minimum time to show loading (for smooth transition)

    private AsyncOperation loadingOperation;
    private bool isLoadingComplete = false;
    //private bool animationActive = false;
    //private int currentSpriteIndex = 0;

    void Start()
    {
        // Start loading the scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            StartCoroutine(LoadSceneAsync());
        }
        else
        {
            Debug.LogError("No scene to load specified!");
        }
    }

    IEnumerator LoadSceneAsync()
    {
        // Start loading the scene asynchronously
        loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingOperation.allowSceneActivation = false;

        float timer = 0f;
        float progress = 0f;

        // Wait for loading to complete
        while (!loadingOperation.isDone)
        {
            timer += Time.deltaTime;

            // Calculate progress (0-0.9 for loading, 0.9-1.0 for activation)
            progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            // Update progress text
            progressText.text = $"Loading... {Mathf.Round(progress * 100)}%";

            // Check if loading is "complete" (reached 90%)
            if (loadingOperation.progress >= 0.9f && timer >= fakeLoadingMinTime)
            {
                isLoadingComplete = true;
                loadingOperation.allowSceneActivation = false;

                // Show "Press any button" text
                pressAnyButtonText.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }


    void Update()
    {
        // Check for any input when loading is complete
        if (isLoadingComplete && Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            //animationActive = false;
            loadingOperation.allowSceneActivation = true;
        }
    }

    // For testing purposes - can be called from another script to trigger the loading
    public void StartLoadingScene(string sceneName)
    {
        sceneToLoad = sceneName;
        StartCoroutine(LoadSceneAsync());
    }
}
