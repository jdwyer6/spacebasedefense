using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital_Assassin : MonoBehaviour
{
    public GameObject player;          // Reference to the player object
    public float orbitSpeed = 1f;      // Speed at which the object will orbit around the player

    private Vector3 relativePosition;  // The initial relative position of the object to the player

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set on the orbiting object.");
            return;
        }
        
        relativePosition = transform.position - player.transform.position;
    }

    private void Update()
    {
        // Orbit the object around the player
        transform.position = player.transform.position + relativePosition;
        transform.RotateAround(player.transform.position, Vector3.forward, orbitSpeed * Time.deltaTime);
        relativePosition = transform.position - player.transform.position;
    }
}
