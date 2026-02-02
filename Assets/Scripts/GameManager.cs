using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region NEW CODE

    // Pause Player State
    public bool IsPlayedPaused { get; set; } = false;

    //START
    private void Start()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        IsPlayedPaused = true;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(GameResume());
    }
    IEnumerator GameResume()
    {
        IsPlayedPaused = false;
        StopAllCoroutines();
        return null;
    }

    //A1
    public bool A1OrbAcquired { get; set; } = false;


    //A3
    public bool A3PortalActive { get; set; } = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            A3PortalActive = true;
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

    #region Second Area

    public void CheckArea2()
    {
        if (Pedestal.GetComponent<Pedestal>().PedestalCompleted)
        {
            Area2PedestalCompleted = true;
        }
    }

    #endregion

    #region Third Area

    public bool CheckArea3Quest()
    {
        if (OreItemCount >= 3)
        {
            return Quest1Completed = true;
        }
        else
        {
            return Quest1Completed = false;
        }
    }

    // Fire Pedestal for Now
    public void CheckArea3()
    {
        if (FirePedestal.GetComponent<FirePedestal>().FirePedestalCompleted)
        {
            Area3PedestalCompleted = true;
        }
    }

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

    #region 7th Area

    /// <summary>
    /// Maybe call it in one of the runes
    /// </summary>
    /// <returns></returns>
    public bool CheckArea71()
    {
        if (Altar1.GetComponent<Altar>().isActivated &&
            Altar2.GetComponent<Altar>().isActivated)
        {
            Debug.Log("Area 7.1 Set");

            RuneLights[0].gameObject.SetActive(true);
            RuneLights[1].gameObject.SetActive(true);

            return Area1Set = true;
        }
        else
        {
            return  Area1Set = false;
        }
    }

    public bool CheckArea72()
    {
        foreach (GameObject pedestal in Pedestals)
        {
            if (!pedestal.GetComponent<Lever>().isActivated)
            {
                return Area2Set = false;
            }
        }

        RuneLights[2].gameObject.SetActive(true);
        RuneLights[3].gameObject.SetActive(true);

        Debug.Log("Area 7.2 Set");
        return Area2Set = true;
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
