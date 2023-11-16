using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{
    public float distanceToAttract = 10f;
    public float moveSpeed = 5f;
    private float checkInterval = 0.5f;

    private List<GameObject> pickupsToMove = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CheckPickups());
    }

    private void Update()
    {
        MovePickupsTowardsPlayer();
    }

    private IEnumerator CheckPickups()
    {
        while (true)
        {
            // Get all pickups
            GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");

            foreach (GameObject pickup in pickups)
            {
                if (!pickupsToMove.Contains(pickup) &&
                    Vector3.Distance(transform.position, pickup.transform.position) <= distanceToAttract)
                {
                    pickupsToMove.Add(pickup);
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void MovePickupsTowardsPlayer()
    {
        for (int i = pickupsToMove.Count - 1; i >= 0; i--)
        {
            GameObject pickup = pickupsToMove[i];
            if (pickup == null) 
            {
                pickupsToMove.RemoveAt(i);
                continue;
            }

            Vector3 moveDirection = (transform.position - pickup.transform.position).normalized;
            float distance = Vector3.Distance(transform.position, pickup.transform.position);

            if(distance > 0.01f) {
                // Increase speed as the pickup gets closer
                float speedFactor = (distanceToAttract - distance) / distanceToAttract;
                pickup.transform.position += moveDirection * moveSpeed * speedFactor * Time.deltaTime;
            }

            if (distance <= 0.1f)
            {
                pickupsToMove.RemoveAt(i);
            }
        }
    }
}
