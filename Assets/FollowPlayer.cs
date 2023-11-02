using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayer : MonoBehaviour
{
    public Transform player; 
    public float followDistance = 5f;
    public float maxSpeed = 5f;
    public float accelerationForce = 10f; 

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Vector2 normalizedDirection = directionToPlayer.normalized;

        float targetSpeed = 0f;

        if (distanceToPlayer > followDistance)
        {
            targetSpeed = maxSpeed;
        }

        // Calculate the desired velocity and the required force
        Vector2 desiredVelocity = normalizedDirection * targetSpeed;
        Vector2 requiredForce = (desiredVelocity - rb.velocity) * accelerationForce;

        rb.AddForce(requiredForce, ForceMode2D.Force);
    }
}
