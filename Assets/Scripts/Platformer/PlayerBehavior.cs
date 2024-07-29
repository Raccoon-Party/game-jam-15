using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Animals : int
{
    Penguin = 1,
    Dog = 2
};

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

    [Header("Animators")]
    [SerializeField] RuntimeAnimatorController penguinController;
    [SerializeField] RuntimeAnimatorController dogController;

    [Header("Sprint")]
    [SerializeField] float sprintMaxSpeed = 10f;
    [SerializeField] float sprintTime = 2f;
    [SerializeField] float decelerateTime = 1f;

    #endregion

    #region Private Properties
    private float gravity;
    private PlatformEffector2D platformEffector;
    private bool isAlive = true;
    private bool canSlide = true;
    private bool isSliding;
    private bool isSprinting = false;
    private bool isJumping;
    private float coyoteTimeCounter;
    private bool hasJumped = false;
    private float timer = 0;
    private int jumpCounter = 2;

    private Animals currentAnimal = Animals.Penguin;

    #endregion

    #region Game Object Components

    Vector2 moveDirection;
    Rigidbody2D rb;
    Animator animator;
    PolygonCollider2D[] bodyColliders;
    PolygonCollider2D bodyCollider;
    BoxCollider2D[] feetColliders;
    BoxCollider2D feetCollider;
    PlayerInput playerInput;

    #endregion

    #region Checks

    private bool isGrounded => feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    private bool isOnThroughPlatform => feetCollider.IsTouchingLayers(LayerMask.GetMask("JumpThroughPlatform"));
    //private bool isChangingDirection => Mathf.Sign(rb.velocity.x) != Mathf.Sign(moveDirection.x);
    private bool hasSpeed => Mathf.Abs(rb.velocity.x) > 0.01;
    private bool hasHeight => Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;

    #endregion

    #region Behavior Methods

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyColliders = GetComponents<PolygonCollider2D>();
        bodyColliders[0].enabled = true;
        for (int i = 1; i < bodyColliders.Length; i++)
        {
            bodyColliders[i].enabled = false;
        }
        bodyCollider = bodyColliders[0];

        feetColliders = GetComponents<BoxCollider2D>();
        feetCollider = feetColliders[0];
        for (int i = 1; i < feetColliders.Length; i++)
        {
            feetColliders[i].enabled = false;
        }
        gravity = 3;
        platformEffector = GameObject.FindWithTag("JumpThroughPlatform").GetComponent<PlatformEffector2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
        animator.runtimeAnimatorController = penguinController;
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
        if (isGrounded || isOnThroughPlatform)
        {
            if (currentAnimal == Animals.Penguin)
            {
                canSlide = true;
            }
            if (currentAnimal == Animals.Dog)
            {
                jumpCounter = 2;
            }
        }


        Run();
        FlipSprite();
        AnimateJumping();
        Die();
    }

    private void FixedUpdate()
    {
        ApplyDrag();
        SprintTimer();
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
        if (isSprinting)
        {
            if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(moveDirection.x))
            {
                timer = 0;
            }
        }
    }

    void OnJump(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        isJumping = inputValue.isPressed;
        if (currentAnimal == Animals.Penguin)
        {
            if (coyoteTimeCounter > 0f && isJumping && !hasJumped)
            {
                hasJumped = true;
                rb.velocity += new Vector2(0, jumpHeight);
            }
        }
        if (currentAnimal == Animals.Dog)
        {
            if (coyoteTimeCounter > 0f && isJumping)
            {
                jumpCounter--;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            if (coyoteTimeCounter <= 0 && isJumping && jumpCounter > 0)
            {
                jumpCounter--;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
        }
    }

    void OnSpecialAction(InputValue inputValue)
    {
        if (currentAnimal == Animals.Penguin)
        {
            if (inputValue.isPressed && canSlide)
            {
                isSliding = true;
                canSlide = false;
                StartCoroutine(Dash());
            }
        }
        else if (currentAnimal == Animals.Dog)
        {
            isSprinting = inputValue.isPressed;
        }
    }

    void OnSwitchCharacter(InputValue inputValue)
    {
        if ((isGrounded || isOnThroughPlatform) && moveDirection == Vector2.zero)
        {
            int character = int.Parse(inputValue.Get().ToString());
            bool isUnlocked = FindObjectOfType<GameSession>().IsAnimalUnlocked(character);

            if (isUnlocked && (Animals)character != currentAnimal)
            {
                rb.velocity = new Vector2(0, 5);
                currentAnimal = (Animals)character;
                EnableAnimal();
            }
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

    public void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Danger")))
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        playerInput.DeactivateInput();

        animator.SetBool("isJumping", false);
        if (currentAnimal == Animals.Penguin)
        {
            animator.SetBool("isSliding", false);
        }
        animator.SetBool("isMoving", false);
        animator.SetTrigger("death");

        FindObjectOfType<GameSession>().ProcessPlayerDeath();
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
            if (isSprinting)
            {
                var speed = runSpeed + timer * (sprintMaxSpeed - runSpeed) / sprintTime;
                speed = speed > sprintMaxSpeed ? sprintMaxSpeed : speed;
                rb.velocity = new Vector2(moveDirection.x * speed, velocity_y);
            }
            else
            {
                if (rb.velocity.x > runSpeed)
                {
                    var speed = rb.velocity.x - timer * (sprintMaxSpeed - runSpeed) / decelerateTime;
                    rb.velocity = new Vector2(moveDirection.x * speed, velocity_y);
                }
                else
                {
                    rb.velocity = new Vector2(moveDirection.x * runSpeed, velocity_y);
                }
            }
        }
        if (hasSpeed)
        {
            animator.SetBool("isMoving", true);
            if (currentAnimal == Animals.Dog)
            {
                if (isSprinting)
                {
                    var speed = runSpeed + timer * (sprintMaxSpeed - runSpeed) / sprintTime;
                    speed = speed > sprintMaxSpeed ? sprintMaxSpeed : speed;
                    animator.SetFloat("runningSpeed", speed * 0.75f / runSpeed);
                }
                else
                {
                    animator.SetFloat("runningSpeed", 1);
                }
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }


    }

    private void SprintTimer()
    {
        if (currentAnimal == Animals.Dog)
        {
            if (isSprinting)
            {
                if (isGrounded || isOnThroughPlatform)
                {
                    timer += Time.deltaTime;
                }
            }
            else
            {
                timer = 0;
            }
        }
    }

    private void AnimateJumping()
    {
        if (isGrounded || isOnThroughPlatform)
        {
            hasJumped = false;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (coyoteTimeCounter < 0)
            {
                if (jumpCounter == 2)
                    jumpCounter = 1;
            }
        }

        if (currentAnimal == Animals.Penguin)
        {
            if (!isSliding)
            {
                if (!isJumping && hasHeight)
                {
                    rb.gravityScale = jumpGravity;
                }
                else
                {
                    rb.gravityScale = gravity;
                }
            }
        }
        if (currentAnimal == Animals.Dog)
        {
            if (!isJumping && hasHeight)
            {
                rb.gravityScale = jumpGravity;
            }
            else
            {
                rb.gravityScale = gravity;
            }
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
        rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * slideSpeed, 0f);
        yield return new WaitForSeconds(slidingTime);
        rb.gravityScale = gravity;
        isSliding = false;
        animator.SetBool("isSliding", false);
        rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * rb.velocity.x / slideSpeed, rb.velocity.y);
        yield return new WaitForSeconds(slidingCooldown);
        if (isGrounded || isOnThroughPlatform) canSlide = true;
    }


    private void EnablePenguin()
    {
        animator.runtimeAnimatorController = penguinController;
        currentAnimal = Animals.Penguin;
        bodyCollider = bodyColliders[0];
        feetCollider = feetColliders[0];
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
        isSprinting = false;
        foreach (var item in bodyColliders.Where(x => x != bodyCollider))
        {
            item.enabled = false;
        }
        foreach (var item in feetColliders.Where(x => x != feetCollider))
        {
            item.enabled = false;
        }
    }

    private void EnableDog()
    {
        animator.runtimeAnimatorController = dogController;
        currentAnimal = Animals.Dog;
        animator.SetFloat("runningSpeed", 1f);
        bodyCollider = bodyColliders[1];
        feetCollider = feetColliders[1];
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
        foreach (var item in bodyColliders.Where(x => x != bodyCollider))
        {
            item.enabled = false;
        }
        foreach (var item in feetColliders.Where(x => x != feetCollider))
        {
            item.enabled = false;
        }
    }

    private void EnableAnimal()
    {
        switch (currentAnimal)
        {
            case Animals.Penguin:
                EnablePenguin();
                break;
            case Animals.Dog:
                EnableDog();
                break;
            default:
                EnablePenguin();
                break;
        }
    }

    #endregion
}
