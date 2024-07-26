using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowEnemyBehavior : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawner;
    [SerializeField] float fireCooldown = 1.5f;
    [Header("Potion")]
    [SerializeField] GameObject potion;
    Animator animator;

    private bool isFiring = false;
    PlayerBehavior player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerBehavior>();
    }

    void FixedUpdate()
    {
        if (!isFiring)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        isFiring = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.3f);
        projectile = Instantiate(projectile, projectileSpawner.position, transform.rotation);
        Vector2 direction = player.transform.position - projectileSpawner.position;
        direction.Normalize();
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile.GetComponent<ProjectileBehavior>().firingSpeed;
        yield return new WaitForSeconds(fireCooldown);
        animator.SetBool("isAttacking", false);
        isFiring = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (potion != default && potion != null)
            {
                potion.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
