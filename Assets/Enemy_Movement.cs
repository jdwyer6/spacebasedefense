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

        // speed = originalSpeed * gm.GetComponent<Enemy_Spawner>().enemyMovementSpeedMultiplier;
    }

    void Update()
    {
        if (player != null)
        {
            if(!isStationary) {
                Vector2 direction = (player.position - transform.position).normalized;
                rb2d.velocity = direction * speed;
            }else{
                    float distance = Vector2.Distance(transform.position, player.position);

                    // If enemy is within 5 units from the player
                    if (distance <= 15f)
                    {
                        rb2d.velocity = Vector2.zero;
                    }else{
                        Vector2 direction = (player.position - transform.position).normalized;
                        rb2d.velocity = direction * speed;
                    }

                

            }

        }
    }

}
