
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;

    Vector2 moveDirection;

    [SerializeField] float speed = 3;

    private bool isMoving;

    private Animator animator;

    private Rigidbody2D rb;

    public LayerMask interactablesLayer;


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, interactablesLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactables>()?.Interact();
        }
    }

    void Move()
    {
        // Vector3 targetPosition = transform.position + new Vector3(moveDirection.x * speed, moveDirection.y * speed) * Time.deltaTime;
        // transform.position = targetPosition;
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();

        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            animator.SetBool("isMoving", false);
            return;
        }
        animator.SetFloat("moveX", moveDirection.x);
        if (moveDirection.y != 0) moveDirection.x = 0;
        animator.SetFloat("moveY", moveDirection.y);
        animator.SetBool("isMoving", isMoving);
        Debug.Log(moveDirection);
    }
}
