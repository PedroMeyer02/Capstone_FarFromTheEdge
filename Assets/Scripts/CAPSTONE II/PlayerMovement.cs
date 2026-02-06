using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mobility and Movement Utils")]
    public Rigidbody rb;
    Vector3 moveDirection;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Player player;


    [Header("Player Stats")]
    // Don't adjust here, use the Player Component in the Inspector
    public int moveSpeed = 10;






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPlayerPaused)
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

    void FixedUpdate()
    {
        // Stop movement when paused
        if (GameManager.Instance.IsPlayerPaused)
        {

            rb.linearVelocity = Vector3.zero;
            moveDirection = Vector2.zero;
            return;

        }
        else
        {

            // This is an Movement implementation for Unity's Input System
            Vector2 velocity = moveDirection * moveSpeed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.y);

            // Flip the player sprite based on movement direction
            GetFacingDirection(-moveDirection.x);
        }

    }

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

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.inProgress && !player.isInteracting && !GameManager.Instance.IsPlayerPaused)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else moveDirection = Vector3.zero;
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        moveDirection = Vector2.zero;
        animator.SetFloat("MoveSpeed", 0f);
    }

}
