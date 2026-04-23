using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
    #region NEW CODE

    GameObject cameraPlaceholder;
    Player player;
    public UIManager UIManager;
    bool gameStarted = false;

    // Pause Player State
    [SerializeField] public bool IsPlayerPaused = false;
    //[SerializeField] public bool IsPlayerPaused { get; set; } = false;

    //START
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        UIManager = FindAnyObjectByType<UIManager>();
    }

    IEnumerator GameStart()
    {
        IsPlayerPaused = true;
        yield return new WaitForSeconds(1.25f);
        StartCoroutine(GameResume());
    }
    IEnumerator GameResume()
    {
        IsPlayerPaused = false;
        yield return new WaitForSeconds(.1f);
        StopAllCoroutines();
    }


    public void CameraToCharacter()
    {
        cameraPlaceholder.SetActive(false);
        cameraPlaceholder = null;  
    }

    public void CameraToObject(GameObject newTarget)
    {
        cameraPlaceholder = newTarget;
        newTarget.SetActive(true);
    }

    //A1
    public bool A1Skill1Acquired { get; set; } = false;
    public bool A1WallOpen { get; set; } = false;
    
    //A2
    public bool A2Skill2Acquired { get; set; } = false;
    public bool A2FirstUseSkill { get; set; } = false;
    public bool A2isFreeFromTrap { get; set; } = false;
    public bool A2FinalDialogueActivate { get; set; } = false;

    public bool A2RuneWasActivated { get; set; } = false;

    //A3
    public bool A3PortalActive { get; set; } = false;

    //A4
    public bool A4Skill3Acquired { get; set; } = false;
    public bool A4isFreeFromTrap { get; set; } = false;
    public int A4PortalsActives { get; set; } = 0;
    public bool A4FinalDialogueActivate { get; set; } = false;


    private void Update()
    {
        //A3 REMOVE
        if(Input.GetKeyDown(KeyCode.T))
        {
            //A3PortalActive = true;
            //A1Skill1Acquired = true;
            //A2Skill2Acquired = true;
        //    //A4Skill3Acquired = true;
        //    //wA2isFreeFromTrap = true;
        }

        //if(Input.GetKeyDown(KeyCode.J))
        //{
        //    IsPlayerPaused = false;
        //}

        if (!gameStarted)
        {
            StartCoroutine(GameStart());
            gameStarted = true;
        }

    }
    #endregion

    // Effects
    public Image transitionCanvas;

    //TextSpeed for menu settings
    public float textSpeed = 0.01f;

    /// <summary>
    /// Pause the Game on pressing ESC, Attached to Player Input 
    /// </summary>
    public void PauseMenuManager()
    {
        if (Time.timeScale == 1f)
        {

            if (GameObject.FindGameObjectWithTag("PauseMenu") == null)
            {
                Instantiate(Resources.Load("PauseMenu"));
            }

            // Pause
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f)
        {

            // Destroy the pause menu
            Destroy(GameObject.FindGameObjectWithTag("PauseMenu"));

            // Resume 
            Time.timeScale = 1f;
        }
    }



    #region ENDGAME

    public void GameWon()
    {
        StopAllCoroutines(); // Stop any ongoing fade in/out coroutines
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float currentFadeTime = 0f;
        float fadeOutInSeconds = 3f;
        Color c = transitionCanvas.color;

        if (c.a > 0) currentFadeTime = fadeOutInSeconds * c.a;

        while (currentFadeTime < fadeOutInSeconds)
        {
            currentFadeTime += Time.deltaTime;

            c.a = Mathf.Clamp(currentFadeTime / fadeOutInSeconds, 0f, 2f);

            transitionCanvas.color = c;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);

        LoadGameWonScene();
    }

    private void LoadGameWonScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    #endregion




}
