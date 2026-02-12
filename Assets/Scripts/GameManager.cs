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
    
    //A2
    public bool A2Skill2Acquired { get; set; } = false;

    //A3
    public bool A3PortalActive { get; set; } = false;

    //A4
    public bool A4Skill3Acquired { get; set; } = false;
    public int A4PortalsActives { get; set; } = 0;
    private void Update()
    {
        //A3 REMOVE
        if(Input.GetKeyDown(KeyCode.T))
        {
            //A3PortalActive = true;
            //A1Skill1Acquired = true;
            //A2Skill2Acquired = true;
            //A4Skill3Acquired = true;
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




    [Header("Area 1 Utilities")]
    public bool BlueOrbItem { get; set; } = false;

    // Area 2 UTILS 
    public bool Area2PedestalCompleted { get; set; } = false;

    // Area 3 UTILS
    public bool Quest1Completed { get; set; } = false;
    public bool Quest1ReadytoComplete { get; set; } = false;
    public bool Area3PedestalCompleted { get; set; } = false;

    // Area 4 UTILS

    // Area 5 UTILS

    // Area 6 UTILS
    public bool SightAbilityUnlocked { get; set; } = false;
    public bool Quest2ReadytoComplete { get; set; } = false;
    public bool Quest2Completed { get; set; } = false;
    public bool Area6PuzzleCompleted { get; set; } = false;
    public int[] correctOrder = { 0, 1, 2, 3 };
    public int progress = 0;

    // Area 7 UTILS
    public bool Area1Set { get; set; } = false;
    public bool Area2Set { get; set; } = false;
    public Light[] RuneLights;

    // ----------------------------

    // Item Utils
    public int PedalItemCount { get; set; } = 0;
    public int OreItemCount { get; set; } = 0;
    public int BlueOreItemCount { get; set; } = 0;
    public int GoldenPedalItemCount { get; set; } = 0;
    public int GoldenOreItemCount { get; set; } = 0;

    // Equipment Utils
    public bool HasPickaxeEquipped { get; set; } = false;
    public bool HasFireOrbEquipped { get; set; } = false;
    public bool HasGreenOrbEquipped { get; set; } = false;

    public int PickaxeItem { get; set; } = 0;
    public int FireOrbItem { get; set; } = 0;
    public int GreenOrbItem { get; set; } = 0;

    //----------------------------

    // 2nd Area Puzzle Completed
    [SerializeField]
    GameObject Pedestal;

    // 3rd Area Puzzle Completed
    [SerializeField]
    GameObject FirePedestal;

    // 4th area puzzle completed
    [SerializeField]
    GameObject AltarPedestal;

    // 7th Area Puzzle Completed
    [SerializeField]
    GameObject Altar1, Altar2;

    [SerializeField]
    GameObject[] Pedestals;

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

    #region First Area


    #endregion

    #region Fourth Area

    public bool CheckAlchemistQuest()
    {
        if (PedalItemCount >= 8 && BlueOreItemCount >= 6)
        {
            return Quest2Completed = true;
        }
        else
        {
            return Quest2Completed = false;
        }
    }

    #endregion

    #region 6th Area

    public void TryActivateRune(int runeIndex)
    {
        // Player hit the correct next rune
        if (runeIndex == correctOrder[progress])
        {
            Debug.Log("Correct Rune: " + runeIndex);
            progress++;

            //(runeIndex);

            // Check if puzzle solved
            if (progress >= correctOrder.Length)
            {
                Area6PuzzleCompleted = true;

                Debug.Log("Grats");
            }
        }
        else
        {
            Debug.Log("Wrong Rune, Resetting puzzle");
            progress = 0;
        }
    }

    #endregion

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
        SceneManager.LoadScene(3);
    }

    #endregion




}
