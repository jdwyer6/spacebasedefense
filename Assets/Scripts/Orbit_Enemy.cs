using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit_Enemy : MonoBehaviour
{
    private GameObject player;  

    public float orbitSpeed = 30f;  
    public float moveSpeed = 5f;   
    public float minDistance = 5f; 

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null) return;  

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > minDistance)
        {
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        }


        Vector3 orbitDirection = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0);
        transform.position += orbitDirection * orbitSpeed * Time.deltaTime;
        
    }
}
