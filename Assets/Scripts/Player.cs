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
    public int moveSpeed = 10;

    [Header("Item Utils")]
    private IPlayerInteractable nearbyInteractable;
    private bool isInteracting = false;

    [Header("Safety Net")]
    public float fallThreshold = -10f;
    public float recordInterval = 0.2f;   // position saved every 0.2s
    public float historyDuration = 5f;    // save last 5 seconds

    // Animation Utils
    private static readonly int collectParam = Animator.StringToHash("PlayerCollect");

    //[Header("Equipment Utils")]
    //public GameObject pickaxe;
    //public GameObject fireOrb;

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

        if (GameManager.Instance.IsPlayedPaused)
        {
            animator.SetFloat("MoveSpeed", 0f);
            return;
        }
        else 
        {
            // Update animation parameter every frame
            float speed = moveDirection.magnitude;
            animator.SetFloat("MoveSpeed", speed);

            // Flip the sprite
            GetFacingDirection(-moveDirection.x);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Stop movement when paused
        if (GameManager.Instance.IsPlayedPaused)
        {
            rb.linearVelocity = Vector3.zero;
            moveDirection = Vector2.zero;
            return;
        }

        // This is an Movement implementation for Unity's Input System
        Vector2 velocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.y);

        // Flip the player sprite based on movement direction
        GetFacingDirection(-moveDirection.x);
    }

    //private void UpdateEquipedItem()
    //{
    //    if (GameManager.Instance.HasPickaxeEquipped)
    //    {
    //        pickaxe.SetActive(true);
    //    }
    //    else
    //    {
    //        pickaxe.SetActive(false);
    //    }

    //    if (GameManager.Instance.HasFireOrbEquipped)
    //    {
    //        fireOrb.SetActive(true);
    //    }
    //    else
    //    {
    //        fireOrb.SetActive(false);
    //    }
    //}

    #region Movement Methods

    /// <summary>
    /// Method that flips the player sprite based on direction
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
    private float GetFacingDirection(float moveDirection)
    {
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = false;
            return 1f;
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
            return -1f;
        }

        return 0f;
    }

    /// <summary>
    /// Called in Player's "Player Input" Component to move
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.inProgress && !isInteracting)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else moveDirection = Vector3.zero;
    }

    #endregion

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

    /// <summary>
    ///  Collect Items BOI by Pressing E
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.action.inProgress && nearbyInteractable != null && !isInteracting)
        {
            // If the object was destroyed, ignore
            if (nearbyInteractable == null)
            {
                nearbyInteractable = null;
                return;
            }

            // Pause Player
            isInteracting = true;
            GameManager.Instance.IsPlayedPaused = true;

            moveDirection = Vector3.zero;
            rb.linearVelocity = Vector3.zero;

            animator.SetTrigger(collectParam);
            nearbyInteractable.OnPlayerInteraction(this);

            // Only clear for objects that are one-time (collectibles)
            if (nearbyInteractable is Item)
            {
                nearbyInteractable = null;
            }

            // Resume Pause State (Number corresponds to collect animation time)
            StartCoroutine(ResumeAfterPause(0.7540984f));
        }
        else
        {
            Debug.Log("No interactable nearby or already interacting.");
        }
    }

    /// <summary>
    /// Resume after the pause state
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ResumeAfterPause(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.IsPlayedPaused = false;
        isInteracting = false;

        // Immediately apply movement
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            var moveAction = playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveDirection = moveAction.ReadValue<Vector2>();
            }
        }
    }

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

    /// <summary>
    /// Visual Cue pop up 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent(out IPlayerInteractable interactable))
        {
            nearbyInteractable = interactable;

            // Tell the item to show its cue if it’s an Item
            if (interactable is Item item)
                item.ShowCue();

            if (interactable is Lever lever)
            {
                if (!lever.isActivated)
                    lever.ShowCue();
                else
                    lever.HideCue();
            }

            if (interactable is Pedestal pedestal)
            {
                if (!pedestal.PedestalCompleted && GameManager.Instance.PedalItemCount > 0)
                    pedestal.ShowCue();
                else
                    pedestal.HideCue();
            }

            if (interactable is FirePedestal firePedestal)
            {
                if (!firePedestal.PedestalCompleted && GameManager.Instance.FireOrbItem > 0 && GameManager.Instance.HasFireOrbEquipped)
                    firePedestal.ShowCue();
                else
                    firePedestal.HideCue();
            }

            if (interactable is LeverPedestal leverPedestal)
            {
                if (!leverPedestal.PedestalCompleted && !leverPedestal.isActivated)
                    leverPedestal.ShowCue();
                else
                    leverPedestal.HideCue();
            }

            if (interactable is Altar altar)
            {
                if (!altar.isActivated)
                    altar.ShowCue();
                else
                    altar.HideCue();
            }
        }

        
    }

    /// <summary>
    /// Disable Visual Cue when nearby items are missing (collected)
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayerInteractable interactable))
        {
            // Tell the item to hide its cue if it’s an Item
            if (interactable is Item item)
                item.HideCue();

            if (interactable is Pedestal pedestal)
                pedestal.HideCue();

            if (interactable is FirePedestal firePedestal)
                firePedestal.HideCue();

            if (interactable is LeverPedestal leverPedestal)
                leverPedestal.HideCue();

            if (interactable is Lever lever)
                lever.HideCue();

            if (interactable is Altar altar)
                altar.HideCue();

                nearbyInteractable = null;
        }
    }

    #endregion
}

