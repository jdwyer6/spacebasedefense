using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMotionsController : MonoBehaviour
{
    public GameObject wing1;
    public GameObject wing2;

    public bool flapWings;
    public float flapSpeed = 5f; // Speed at which the wings flap
    public float flapAmount = 45f; // Maximum angle of wing flap

    private float wingFlapTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flapWings)
        {
            // Increment the timer by the flap speed multiplied by delta time
            wingFlapTimer += flapSpeed * Time.deltaTime;

            // Calculate the rotation using a sine wave
            float angle = Mathf.Sin(wingFlapTimer) * flapAmount;

            if (wing1 != null)
            {
                wing1.transform.localRotation = Quaternion.Euler(0, 0, angle);
            }

            if (wing2 != null)
            {
                wing2.transform.localRotation = Quaternion.Euler(0, 0, -angle); // Opposite direction for the other wing
            }
        }
    }
}
