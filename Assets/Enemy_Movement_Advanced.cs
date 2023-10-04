using UnityEngine;

public class Enemy_Movement_Advanced : MonoBehaviour
{
    public float speed = 5.0f;
    public float dodgeSpeed = 7.0f;
    public Transform player;
    private Vector3 targetPosition;
    private bool isDodging = false;

    private void Update()
    {
        if(!isDodging && CheckForIncomingProjectile())
        {
            StartDodging();
            return;
        }

        if(isDodging)
        {
            MoveTowardsTarget();
            return;
        }

        // Determine approach or backup
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer < 5.0f) // example value, adjust as needed
        {
            BackUpFromPlayer();
        }
        else if(distanceToPlayer > 10.0f) // another example value
        {
            ApproachPlayer();
        }
        else
        {
            UnpredictableMovement();
        }
    }

    bool CheckForIncomingProjectile()
    {
        // You can expand on this with raycasts, or colliders or however you detect incoming projectiles
        // This is a placeholder for that logic
        return false;
    }

    void StartDodging()
    {
        isDodging = true;
        Vector2 dodgeDirection2D = (Random.insideUnitCircle * 3.0f).normalized;
        Vector3 dodgeDirection3D = new Vector3(dodgeDirection2D.x, dodgeDirection2D.y, 0);
        targetPosition = transform.position + dodgeDirection3D;// dodge to a random direction
        Invoke("StopDodging", 0.5f); // dodging duration
    }

    void StopDodging()
    {
        isDodging = false;
    }

    void MoveTowardsTarget()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        transform.position += moveDir * dodgeSpeed * Time.deltaTime;
    }

    void ApproachPlayer()
    {
        Vector3 flankDirection = (player.position - transform.position).normalized;
        flankDirection += new Vector3(flankDirection.y, 0, -flankDirection.x).normalized; // this will make the enemy move in a slightly sideways manner to "flank"
        transform.position += flankDirection * speed * Time.deltaTime;
    }

    void BackUpFromPlayer()
    {
        Vector3 backOffDirection = (transform.position - player.position).normalized;
        transform.position += backOffDirection * speed * Time.deltaTime;
    }

    void UnpredictableMovement()
    {
        // An example might be random waypoints, wandering behavior, or some noise function. For simplicity:
        transform.position += new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * speed * Time.deltaTime;
    }
}
