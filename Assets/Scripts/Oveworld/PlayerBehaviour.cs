
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
    private LayerMask solidObjectsLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += new Vector3(moveDirection.x * speed, moveDirection.y * speed) * Time.deltaTime;
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
        if (moveDirection.x != 0) moveDirection.y = 0;
        animator.SetFloat("moveY", moveDirection.y);
        animator.SetBool("isMoving", isMoving);
        Debug.Log(moveDirection);
    }
}
