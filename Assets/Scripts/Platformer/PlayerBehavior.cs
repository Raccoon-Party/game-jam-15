using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float slideSpeed = 2f;
    [SerializeField] float slidingTime = 0.2f;
    [SerializeField] float slidingCooldown = 0.5f;
    float gravity;
    bool isAlive = true;
    private bool canSlide = true;
    private bool isSliding;

    Vector2 moveDirection;

    Rigidbody2D rb;
    Animator animator;
    CircleCollider2D bodyCollider;
    CapsuleCollider2D feetCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CircleCollider2D>();
        feetCollider = GetComponent<CapsuleCollider2D>();
        gravity = rb.gravityScale;
    }

    void Update()
    {
        if (isSliding)
        {
            return;
        }
        else
        {
            rb.gravityScale = gravity;
        }

        Run(GetHasSpeed());
        FlipSprite(GetHasSpeed());
        AnimateJumping();
        Die();
    }
    private void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Danger")))
        {
            isAlive = false;
            animator.SetTrigger("isDead");
            rb.velocity = new Vector2(0, 8f);
        }
    }

    void OnMove(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        moveDirection = inputValue.Get<Vector2>();
    }

    void OnJump(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        bool isTouchingGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isTouchingGround)
        {
            if (inputValue.isPressed)
            {
                rb.velocity += new Vector2(0, jumpHeight);
            }
        }
    }

    void OnSlide(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        if (inputValue.isPressed && canSlide)
        {
            isSliding = true;
            canSlide = false;
            StartCoroutine(Dash());
        }
    }

    private void FlipSprite(bool hasSpeed)
    {
        if (hasSpeed)
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);

    }

    private void Run(bool hasSpeed)
    {
        bool canClimb = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        float velocity_y = canClimb ? 0 : rb.velocity.y;
        rb.velocity = new Vector2(moveDirection.x * runSpeed, velocity_y);
        if (hasSpeed)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private bool GetHasSpeed()
    {
        return Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    }

    private void AnimateJumping()
    {
        bool isTouchingGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingGround && GetHasHeight())
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }
    private bool GetHasHeight()
    {
        return Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
    }
    private IEnumerator Dash()
    {
        canSlide = false;
        isSliding = true;
        animator.SetBool("isSliding", true);
        gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * slideSpeed, 0f);
        yield return new WaitForSeconds(slidingTime);
        rb.gravityScale = gravity;
        isSliding = false;
        animator.SetBool("isSliding", false);
        yield return new WaitForSeconds(slidingCooldown);
        canSlide = true;
    }
}
