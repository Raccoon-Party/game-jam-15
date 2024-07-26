using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] float firingSpeed = 3f;
    Rigidbody2D rb;
    ShadowEnemyBehavior shadowEnemy;
    float xDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shadowEnemy = FindObjectOfType<ShadowEnemyBehavior>();
        xDirection = shadowEnemy.transform.localScale.x;
    }

    void Update()
    {
        rb.velocity = new Vector2(Mathf.Sign(xDirection) * Random.Range(0f * firingSpeed, 1f * firingSpeed), Random.Range(0f * firingSpeed, 1f * firingSpeed));
        FlipSprite();
    }


    private void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBehavior>().Die();
        }
        Destroy(gameObject);
    }
}
