using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral_Shooting : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed at which the game object rotates.
    public GameObject projectilePrefab; // Assign the projectile prefab in the inspector.
    public Transform barrelPosition1; // Assign the first barrel position in the inspector.
    public Transform barrelPosition2; // Assign the second barrel position in the inspector.
    public float shootInterval = 0.1f; // Time between each shot.
    public float shootingDuration = 1f; // Duration for which the shooting happens.
    public float restDuration = 5f; // Duration to rest after shooting.
    private Transform playerTransform;
    public float projectileSpeed = 5f;
    public GameObject player;

    private void Update()
    {
        RotateObject();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ShootingRoutine());


    }

    void RotateObject()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    IEnumerator ShootingRoutine()
    {
        while (true)
        {
            for (float t = 0; t < shootingDuration; t += shootInterval)
            {
                ShootProjectile(barrelPosition1);
                ShootProjectile(barrelPosition2);
                yield return new WaitForSeconds(shootInterval);
            }
            yield return new WaitForSeconds(restDuration);
        }
    }

    void ShootProjectile(Transform barrelPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, barrelPos.position, barrelPos.rotation);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        // Set the velocity of the projectile
        rb.velocity = direction * projectileSpeed;


        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();

        if (projectileCollider != null && enemyCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }
    }
}
