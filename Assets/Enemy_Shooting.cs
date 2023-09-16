using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy_Shooting : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab to be shot
    public float shootInterval = 2.0f;  // How often the enemy shoots (in seconds)
    public float projectileSpeed = 5.0f; // Speed of the projectile

    private Transform player; // Reference to the player's transform
    private float shootTimer = 0.0f; // Timer to handle shooting intervals
    private AudioManager am;
    public float distanceFromPlayerToStartShooting = 5;

    void Start()
    {
        // Find the player GameObject in the scene by its tag (assuming it's tagged "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        am = FindObjectOfType<AudioManager>();

        // If the player was found, set our reference
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Initialize the shoot timer with the interval
        shootTimer = shootInterval;
    }

    void Update()
    {
        // If we have a reference to the player
        if (player != null)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0 && Vector3.Distance(transform.position, player.position) < distanceFromPlayerToStartShooting)
            {
                ShootAtPlayer();
                shootTimer = shootInterval;
            }
        }
    }

    void ShootAtPlayer()
    {
        am.Play("Enemy_Shot");
        if(gameObject.name == "Googley_Eyes") {
            Debug.Log("Googley");
            am.Play("Fast_Projectile");
        }
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        // Set the velocity of the projectile
        rb.velocity = direction * projectileSpeed;

        // Ignore collision between the enemy and the projectile
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();

        if (projectileCollider != null && enemyCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

        //Ignore collisions with designated objects
        projectile.GetComponent<Projectile>().ignoreList = Projectile.IgnoreList.Enemy;

    }
}
