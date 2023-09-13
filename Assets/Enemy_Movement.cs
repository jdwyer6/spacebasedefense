using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float originalSpeed;
    private float speed;
    private Transform player; 
    private Rigidbody2D rb2d; 
    private GameObject gm;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GM");
        
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb2d = GetComponent<Rigidbody2D>(); 

        speed = originalSpeed * gm.GetComponent<Enemy_Spawner>().enemyMovementSpeedMultiplier;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb2d.velocity = direction * speed;
        }
    }
}
