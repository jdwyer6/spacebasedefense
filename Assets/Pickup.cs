using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float gravity = -0.005f; // Simulated gravity for scaling
    public float fallSpeed = 0;   // Current scaling speed
    public float elasticity = 0.6f; // Percentage of scaling retained after a bounce

    private bool isBouncing = false; // To check if the item is in the bounce phase
    private Vector3 initialScale;

    void Awake()
    {
        initialScale = transform.localScale;
        transform.localScale += Vector3.one * 0.2f; // Start a bit larger to simulate being "closer" to the camera

        StartCoroutine(FallAndBounceAnimation());
    }

    IEnumerator FallAndBounceAnimation()
    {
        float bounceThreshold = -0.002f; // Speed threshold to stop the bouncing

        while (true)
        {
            // Update the fall speed (scaling speed)
            fallSpeed += gravity * Time.deltaTime;

            transform.localScale += Vector3.one * fallSpeed;

            // When the item "hits the ground" (reaches initial scale)
            if (transform.localScale.x <= initialScale.x && !isBouncing)
            {
                transform.localScale = initialScale; // Correct any overshoot
                fallSpeed = -fallSpeed * elasticity; // Reflect the speed and decrease it based on elasticity
                isBouncing = true;
            }
            else if (transform.localScale.x >= initialScale.x && isBouncing)
            {
                transform.localScale = initialScale; // Correct any overshoot
                if (Mathf.Abs(fallSpeed) <= bounceThreshold)
                    break; // If the speed is below the threshold, stop bouncing

                fallSpeed = -fallSpeed * elasticity; // Reflect the speed and decrease it for the next bounce
            }

            yield return null;
        }

        transform.localScale = initialScale; // Ensure it settles at the initial scale
    }
}
