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

    Vector3 position;
    public bool isPulling = false;

    [Header("Player Stats")]
    // Don't adjust here, use the Player Component in the Inspector
    public int moveSpeed = 10;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPlayerPaused)
        {
            animator.SetFloat("MoveSpeedX", 0f);
            animator.SetFloat("MoveSpeedY", 0f);
            return;
        }
        else
        {

            // Update animation parameter every frame
            float speedX = moveDirection.x;
            animator.SetFloat("MoveSpeedX", speedX);

            float speedY = moveDirection.y;
            animator.SetFloat("MoveSpeedY", speedY);

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


            if (position.y == transform.position.y)
            {
                isPulling = false;
            }
            else
            {
                PullDown();
                isPulling = true;
            }

            position = player.transform.position;

                // Flip the player sprite based on movement direction
                GetFacingDirection(-moveDirection.x);
            GetFacingDirectionUpDown(-moveDirection.y);
        }

    }

    public void PullDown()
    {
        Vector3 downForce = new Vector3(0, -10, 0);
        rb.AddForce(downForce);
    }

    private float GetFacingDirection(float moveDirection)
    {
        if (moveDirection > 0)
        {
            animator.SetFloat("MoveSpeedX", -1);
            return 1f;
        }
        else if (moveDirection < 0)
        {
            animator.SetFloat("MoveSpeedX", 1);
            return -1f;
        }

        return 0f;
    }

    private float GetFacingDirectionUpDown(float moveDirection)
    {
        if (moveDirection > 0)
        {
            animator.SetFloat("MoveSpeedY", -1);
            animator.SetFloat("CheckUp", 0);
            return 1f;
        }
        else if (moveDirection < 0)
        {
            animator.SetFloat("MoveSpeedY", 1);
            animator.SetFloat("CheckUp", 1);
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
        animator.SetFloat("MoveSpeedX", 0f);
        animator.SetFloat("MoveSpeedY", 0f);
    }

}
