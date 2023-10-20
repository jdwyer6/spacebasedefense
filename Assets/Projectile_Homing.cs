using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Homing : MonoBehaviour
{
    private GameObject closestEnemy;
    private Rigidbody2D rb;

    public float homingForce = 5.0f;
    public bool isHoming = false;

    private Vector2 initialVelocity;
    public float decelerationDuration = 1.0f;
    public float targetSpeed = 5.0f;
    private float timeSinceLaunch = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isHoming) {
            GetClosestEnemy();
            initialVelocity = rb.velocity;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(isHoming) {
            timeSinceLaunch += Time.deltaTime;
            GetClosestEnemy();
            ApplyHomingForce();
            DecelerateOverTime();
        }

    
    }

    private void GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Make sure there's at least one enemy to compare distances with.
        if (enemies.Length > 0)
        {
            closestEnemy = enemies[0];
            float closestDistance = Vector2.Distance(transform.position, closestEnemy.transform.position);

            foreach (var enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }
        }
    }

    private void ApplyHomingForce()
    {
        if (closestEnemy != null)
        {
            Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;
            rb.AddForce(direction * homingForce);
        }
    }

    private void DecelerateOverTime()
    {
        if (timeSinceLaunch <= decelerationDuration)
        {
            float percentageOfDecelerationComplete = timeSinceLaunch / decelerationDuration;
            float desiredMagnitude = Mathf.Lerp(initialVelocity.magnitude, targetSpeed, percentageOfDecelerationComplete);
            rb.velocity = rb.velocity.normalized * desiredMagnitude;
        }
    }
}
