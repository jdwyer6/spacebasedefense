using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityStrength = -9.81f; // Negative value to move down
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Apply the custom gravity force
        rb2d.AddForce(new Vector3(0, 0, gravityStrength));
    }
}

