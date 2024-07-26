using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatformBehavior : MonoBehaviour
{

    [SerializeField] float startTimeOffset = 0.0f;
    [SerializeField] float timeActive = 1.0f;

    private BoxCollider2D boxCollider;
    private Animator animator;
    private bool isActive = false;
    private bool isDelayed = true;

    private bool coroutineEnded = true;
    private int layer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        isDelayed = startTimeOffset > 0;
        layer = gameObject.layer;
    }

    void FixedUpdate()
    {
        if (isDelayed)
        {
            StartCoroutine(DelayActivation());

        }
        if (!isActive && !isDelayed && coroutineEnded)
        {
            StartCoroutine(WaitForActivation());
        }
    }

    private IEnumerator DelayActivation()
    {
        yield return new WaitForSeconds(startTimeOffset);
        isDelayed = false;
    }


    private IEnumerator WaitForActivation()
    {
        coroutineEnded = false;
        isActive = true;
        animator.SetBool("isActive", true);
        gameObject.layer = layer;
        boxCollider.isTrigger = false;
        yield return new WaitForSeconds(timeActive);
        isActive = false;
        animator.SetBool("isActive", false);
        boxCollider.isTrigger = true;
        gameObject.layer = 1;
        yield return new WaitForSeconds(timeActive);
        coroutineEnded = true;
    }
}
