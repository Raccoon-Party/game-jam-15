using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEnemyBehavior : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawner;
    [SerializeField] float fireCooldown = 1.5f;

    Animator animator;

    private bool isFiring = false;

    void Start()
    {
        animator = GetComponent<Animator>();
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

        Instantiate(projectile, projectileSpawner.position, transform.rotation);
        yield return new WaitForSeconds(fireCooldown);
        isFiring = false;
    }
}
