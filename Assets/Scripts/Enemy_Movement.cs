using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float originalSpeed;
    public float speed;
    private Transform player; 
    private Rigidbody2D rb2d; 
    private GameObject gm;

    public bool isStationary;
    public bool followsVertically;
    public bool charges;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GM");
        
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb2d = GetComponent<Rigidbody2D>(); 
        speed = originalSpeed;

        if(charges) {
            StartCoroutine(Charge());
        }
    }

    void Update()
    {
        
        if (player != null && !charges)
        {
            if(!isStationary) {
                Vector2 direction = (player.position - transform.position).normalized;
                rb2d.velocity = direction * speed;

            }else if(followsVertically) {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // If the player is more than 25 units away, move towards the player.
                if(distanceToPlayer > 25)
                {
                    Vector2 direction = (player.position - transform.position).normalized;
                    rb2d.velocity = direction * speed;
                }
                // If the player is within 25 units, only follow the player vertically.
                else
                {
                    float yDifference = player.position.y - transform.position.y;
                    float yDirection = Mathf.Sign(yDifference); // This will give -1, 0, or 1
                    rb2d.velocity = new Vector2(0, yDirection * speed);
                }
            }else{
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance <= 15f)
                {
                    rb2d.velocity = Vector2.zero;
                }else{
                    Vector2 direction = (player.position - transform.position).normalized;
                    rb2d.velocity = direction * speed;
                }
            }

            if(Vector2.Distance(transform.position, player.position) >= 25) 
            {
                // The direction from the GameObject to the player.
                Vector2 fromPlayerToEnemy = (transform.position - player.position).normalized;

                // Multiply the direction by 25 to get the distance we want the enemy to teleport from the player.
                Vector2 teleportPosition = (Vector2)player.position + fromPlayerToEnemy * 25;

                // Assign the teleport position.
                transform.position = teleportPosition;
            }
        }
    }

    IEnumerator Charge() {
        while (true)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb2d.velocity = direction * speed;

            float chargeDuration = UnityEngine.Random.Range(2, 4);
            float elapsedTime = 0f;

            while (elapsedTime < chargeDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
