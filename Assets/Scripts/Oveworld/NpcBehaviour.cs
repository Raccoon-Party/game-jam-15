using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcBehaviour : MonoBehaviour, Interactables
{
    [SerializeField] Dialog dialog;

    private Animator animator;

    bool isDone;
    public void Interact()
    {
        isDone = DialogManager.Instance.HandleUpdate(dialog);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDone)
        {
            FaceIdleDown();
        }
    }

    public void FacePlayer(Vector2 playerPosition)
    {
        Vector2 direction = playerPosition - (Vector2)transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetFloat("moveX", direction.x > 0 ? 1 : -1);
            animator.SetFloat("moveY", 0);
        }
        else
        {
            animator.SetFloat("moveY", direction.y > 0 ? 1 : -1);
            animator.SetFloat("moveX", 0);
        }
    }

    public void FaceIdleDown()
    {
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }
}
