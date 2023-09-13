using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Enemy_Laser : MonoBehaviour
{
    public float interval = 5f; // Time in seconds between each laser shot.
    public float laserLength = 50f; // Length of the laser
    public Transform laser_start_location; // Starting location of the laser

    private Transform player;
    private Vector3 targetDirection; // The direction in which the laser will be shot.
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private LineRenderer lineRenderer;
    private bool isFiring = false; // Flag to track if the laser is active.
    public float laserDamage = 25;
    private AudioManager am;
    public float distanceFromPlayerToStartShooting = 5;
    bool canFire = false;

    public EdgeCollider2D edgeCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        am = FindObjectOfType<AudioManager>();
        originalColor = spriteRenderer.color; // Store the original color.
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player is tagged "Player".
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Initially, the laser is not visible.
        // edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.enabled = false;
        StartCoroutine(ShootLaser());
    }

    void Update()
    {
        if (isFiring)
        {
            lineRenderer.SetPosition(0, laser_start_location.position);
            UpdateLaserCollider();
        }

        if(Vector3.Distance(transform.position, player.position) < distanceFromPlayerToStartShooting)
        {
            canFire = true;
        }else{
            canFire = false;
        }
    }

    IEnumerator ShootLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            am.Play("Laser_Charge");

            // Determine the direction of the laser based on the player's position at the start of the telegraph period.
            targetDirection = (player.position - laser_start_location.position).normalized;

            // Telegraph for 1 second.
            float telegraphDuration = 1f;
            for (float t = 0; t <= 1; t += Time.deltaTime / telegraphDuration)
            {
                spriteRenderer.color = Color.Lerp(originalColor, new Color(.5f, 0f, 0f), t);
                yield return null;
            }
            am.Stop("Laser_Charge");
            spriteRenderer.color = originalColor;

            if(canFire){
                am.Play("Laser_Fire");

                // Reset color and fire the laser.
                spriteRenderer.color = originalColor;

                // Setting up the LineRenderer to show the laser.
                lineRenderer.SetPosition(0, laser_start_location.position);
                lineRenderer.SetPosition(1, laser_start_location.position + targetDirection * laserLength);
                lineRenderer.enabled = true;
                isFiring = true; // Set the firing flag to true.
                
                ActivateLaserCollider();

                yield return new WaitForSeconds(1f); // Laser lasts for 1 second.

                // Hide the laser.
                lineRenderer.enabled = false;
                DeactivateLaserCollider();
                isFiring = false; // Reset the firing flag.
                am.Stop("Laser_Fire");
            }

        }
    }


    void ActivateLaserCollider() 
    {
        edgeCollider.enabled = true;

        Vector3 start = lineRenderer.GetPosition(0);
        Vector3 end = lineRenderer.GetPosition(1);

        Vector2 startPoint = new Vector2(start.x, start.y);
        Vector2 endPoint = new Vector2(end.x, end.y);

        edgeCollider.points = new Vector2[] { transform.InverseTransformPoint(startPoint), transform.InverseTransformPoint(endPoint) };
    }

    void UpdateLaserCollider()
    {
        Vector3 start = lineRenderer.GetPosition(0);
        Vector3 end = lineRenderer.GetPosition(1);

        Vector2 startPoint = new Vector2(start.x, start.y);
        Vector2 endPoint = new Vector2(end.x, end.y);

        edgeCollider.points = new Vector2[] { transform.InverseTransformPoint(startPoint), transform.InverseTransformPoint(endPoint) };
    }

    private void OnDestroy() {
        am.Stop("Laser_Charge");    
    }

    void DeactivateLaserCollider() {
        edgeCollider.enabled = false;
    }

}
