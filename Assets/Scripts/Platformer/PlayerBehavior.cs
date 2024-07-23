using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{

    #region Serialized Fields
    [Header("Run")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float drag = 0.6f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 12f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float jumpGravity = 5f;

    [Header("Slide")]
    [SerializeField] float slideSpeed = 10f;
    [SerializeField] float slidingTime = 0.2f;
    [SerializeField] float slidingCooldown = 0.5f;

    #endregion

    #region Private Properties
    private float gravity;
    private PlatformEffector2D platformEffector;
    private bool isAlive = true;
    private bool canSlide = true;
    private bool isSliding;
    private bool isJumping;
    private float coyoteTimeCounter;

    #endregion

    #region Game Object Components

    Vector2 moveDirection;
    Rigidbody2D rb;
    Animator animator;
    CircleCollider2D bodyCollider;
    CapsuleCollider2D feetCollider;
    PlayerInput playerInput;

    #endregion

    #region Checks

    private bool isGrounded => feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    private bool isOnThroughPlatform => feetCollider.IsTouchingLayers(LayerMask.GetMask("JumpThroughPlatform"));
    //private bool isChangingDirection => (rb.velocity.x > 0f && moveDirection.x < 0f) || (rb.velocity.x < 0f && moveDirection.x > 0f);

    private bool hasSpeed => Mathf.Abs(rb.velocity.x) > 0.01;
    private bool hasHeight => Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;

    #endregion

    #region Behavior Methods

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CircleCollider2D>();
        feetCollider = GetComponent<CapsuleCollider2D>();
        gravity = 3;
        platformEffector = GameObject.FindWithTag("JumpThroughPlatform").GetComponent<PlatformEffector2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
    }

    void Update()
    {
        if (isSliding)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isSliding", true);
            return;
        }
        else
        {
            rb.gravityScale = gravity;
        }
        if (isGrounded || isOnThroughPlatform) canSlide = true;

        Run();
        FlipSprite();
        AnimateJumping();
        Die();
    }

    private void FixedUpdate()
    {
        ApplyDrag();
    }

    #endregion

    #region Input Events

    void OnMove(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        moveDirection = inputValue.Get<Vector2>();

        if (moveDirection.y == -1)
        {
            platformEffector.surfaceArc = 0;
            StartCoroutine(EnableJumpThroughPlatform());
        }
    }

    void OnJump(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        isJumping = inputValue.isPressed;

        if (coyoteTimeCounter > 0f && isJumping)
        {
            rb.velocity += new Vector2(0, jumpHeight);
        }
    }

    void OnSlide(InputValue inputValue)
    {
        if (inputValue.isPressed && canSlide)
        {
            isSliding = true;
            canSlide = false;
            StartCoroutine(Dash());
        }
    }

    #endregion

    #region Helper Methods

    private void ApplyDrag()
    {
        if (isGrounded || isOnThroughPlatform)
        {
            if (moveDirection.x == 0)
                rb.velocity = new Vector2(rb.velocity.x * drag, rb.velocity.y);
        }
    }

    private void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Danger")))
        {
            playerInput.DeactivateInput();

            animator.SetBool("isJumping", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isMoving", false);
            animator.SetTrigger("death");
        }
    }

    IEnumerator EnableJumpThroughPlatform()
    {
        yield return new WaitForSeconds(0.3f);
        platformEffector.surfaceArc = 180;
    }

    private void FlipSprite()
    {
        if (hasSpeed)
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    private void Run()
    {
        bool canClimb = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        float velocity_y = canClimb ? 0 : rb.velocity.y;
        if (moveDirection.x != 0)
        {
            rb.velocity = new Vector2(moveDirection.x * runSpeed, velocity_y);
        }
        if (hasSpeed)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }


    private void AnimateJumping()
    {
        if (isGrounded || isOnThroughPlatform)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (!isJumping && hasHeight)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = gravity;
        }


        if (!isGrounded && !isOnThroughPlatform && hasHeight)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private IEnumerator Dash()
    {
        canSlide = false;
        isSliding = true;
        animator.SetBool("isSliding", true);
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * slideSpeed, 0f);
        yield return new WaitForSeconds(slidingTime);
        rb.gravityScale = gravity;
        isSliding = false;
        animator.SetBool("isSliding", false);
        yield return new WaitForSeconds(slidingCooldown);
        if (isGrounded || isOnThroughPlatform) canSlide = true;
    }

    #endregion
}
