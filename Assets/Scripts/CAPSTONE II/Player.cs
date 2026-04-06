using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Mobility and Movement Utils")]
    public Rigidbody rb;
    Vector3 moveDirection;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [Header("Player Stats")]
    // Don't adjust here, use the Player Component in the Inspector
    //public int moveSpeed = 10;

    [Header("Item Utils")]
    //private IPlayerInteractable nearbyInteractable;
    public bool isInteracting = false;

    public AudioSource audioSource;

    public PlayerInput input;
    public bool playerIsTrapped = false;
    public int trapID = 0;
    public int trapCount = 0;
    public int trapFree = 0;

    public PlayerMovement playerMovement;

    [Header("Safety Net")]
    public float fallThreshold = -10f;
    public float recordInterval = 0.2f;   // position saved every 0.2s
    public float historyDuration = 5f;    // save last 5 seconds

    // Animation Utils
    private static readonly int collectParam = Animator.StringToHash("PlayerCollect");


    private float safetyTimer;
    private readonly Queue<PositionRecord> positionHistory = new Queue<PositionRecord>();

    private struct PositionRecord
    {
        public Vector3 pos;
        public float time;
        public PositionRecord(Vector3 p, float t)
        {
            pos = p;
            time = t;
        }
    }

    private void Update()
    {
        RecordSafetyPositions();
        CheckFallSafetyNet();

        if (GameManager.Instance.IsPlayerPaused)
        {
            playerMovement.enabled = false;
            isInteracting = true;
            input.enabled = false;
        }
        else if (!GameManager.Instance.IsPlayerPaused)
        {
            playerMovement.enabled = true;
            isInteracting = false;
            input.enabled = true;
        }

        if (playerIsTrapped)
        {
            GameManager.Instance.IsPlayerPaused = true;
            
            if(trapID == 1)
            {
                trapFree = 7;
            }
            else if (trapID == 2)
            {
                trapFree = 15;
            }

            if(Input.GetKeyDown(KeyCode.X))
            {
                trapCount++;

                if (trapID == 1)
                {
                    animator.SetTrigger("Trap1Click");
                }
                else
                {
                    animator.SetTrigger("Trap2Click");
                }


                if (trapCount == trapFree)
                {
                    trapCount = 0;
                    playerIsTrapped = false;
                    GameManager.Instance.IsPlayerPaused = false;

                    if (trapID == 1)
                    {
                        animator.SetTrigger("Trap1Free");
                        GameManager.Instance.A2isFreeFromTrap = true;
                    }
                    else
                    {
                        animator.SetTrigger("Trap2Free");
                        GameManager.Instance.A4isFreeFromTrap = true;
                    }

                }
            }
        }

    }


    #region Safety Net Methods

    private void RecordSafetyPositions()
    {
        safetyTimer += Time.deltaTime;

        if (safetyTimer >= recordInterval)
        {
            safetyTimer = 0f;
            positionHistory.Enqueue(new PositionRecord(transform.position, Time.time));
        }

        // Remove older-than-5s records
        while (positionHistory.Count > 0 &&
               Time.time - positionHistory.Peek().time > historyDuration)
        {
            positionHistory.Dequeue();
        }
    }

    private void CheckFallSafetyNet()
    {
        if (transform.position.y < fallThreshold && positionHistory.Count > 0)
        {
            PositionRecord safePos = positionHistory.Peek();

            transform.position = safePos.pos;
            rb.linearVelocity = Vector3.zero;

            Debug.Log("Safety Net Activated");
        }
    }

    #endregion

    #region Collecting Item Methods

    public void OnTriggerEnter(Collider other)
    {
        // Check the end of the game
        if (other.gameObject.CompareTag("EndGameTrigger"))
        {
            // Trigger endgame sequence
            Debug.Log("Endgame Triggered!");
            // endgame method here
            GameManager.Instance.GameWon();
        }
    }
    #endregion

    //NewCode
    #region Skills
    //SKILLS USAGE
    public void Skill1(InputAction.CallbackContext context)
    {
        if (context.action.inProgress && !isInteracting && GameManager.Instance.A1Skill1Acquired)
        {
            isInteracting = true;
            GameManager.Instance.IsPlayerPaused = true;

            StartCoroutine(UseSkill1());
            Debug.Log("Skill1 used");
        }
        else return;
    }
    public void Skill2(InputAction.CallbackContext context)
    {
        if (context.action.inProgress && !isInteracting && GameManager.Instance.A2Skill2Acquired)
        {
            isInteracting = true;
            GameManager.Instance.IsPlayerPaused = true;

            StartCoroutine(UseSkill2());
            Debug.Log("Skill2 used");
        }
        else return;
    }
    public void Skill3(InputAction.CallbackContext context)
    {
        if (context.action.inProgress && !isInteracting && GameManager.Instance.A4Skill3Acquired)
        {
            isInteracting = true;
            GameManager.Instance.IsPlayerPaused = true;

            StartCoroutine(UseSkill3());
            Debug.Log("Skill3 used");
        }
        else return;
    }

    IEnumerator UseSkill1()
    {
        animator.SetTrigger("Skill1");
        audioSource.Play();

        yield return new WaitForSeconds(2f);
        GameManager.Instance.IsPlayerPaused = false;
        isInteracting = false;
        StopAllCoroutines();
    }

    IEnumerator UseSkill2()
    {
        animator.SetTrigger("Skill2");
        audioSource.Play();

        yield return new WaitForSeconds(2f);
        GameManager.Instance.IsPlayerPaused = false;
        isInteracting = false;
        StopAllCoroutines();
    }

    IEnumerator UseSkill3()
    {
        animator.SetTrigger("Skill3");
        audioSource.Play();

        yield return new WaitForSeconds(2f);
        GameManager.Instance.IsPlayerPaused = false;
        isInteracting = false;
        StopAllCoroutines();
    }
    
    #endregion
}

