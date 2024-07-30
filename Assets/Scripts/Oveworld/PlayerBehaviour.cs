using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBehaviour : MonoBehaviour
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

        var gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null && gameSession != default)
            transform.position = gameSession.GetSavedOverworldPosition();
    }

    // Update is called once per frame
    public void Update()
    {
        Move();
    }

    void OnInteract(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            Interact();
        }
    }

    void Interact()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, interactablesLayer);
        if (collider != null)
        {
            var interactable = collider.GetComponent<Interactables>();
            interactable?.Interact();

            var npcBehaviour = collider.GetComponent<NpcBehaviour>();
            if (npcBehaviour != null)
            {
                npcBehaviour.FacePlayer(transform.position);
            }
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
    }
}
