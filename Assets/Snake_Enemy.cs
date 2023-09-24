using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Enemy : MonoBehaviour
{
    public int tailLength = 50; // Number of segments in the tail.
    public float segmentSpacing = 0.1f; // Distance before a new segment is added.
    private GameObject player;

    private Queue<Vector3> tailPositions = new Queue<Vector3>();
    private LineRenderer lineRenderer;
    private Vector3 lastPosition;

    public float speed = 5f; // Speed of horizontal movement.

    private Vector2 direction;
    bool ignoreCollisions = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lastPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        direction = Vector2.right;
        SpawnSnake();
        StartCoroutine(DieAfterSeconds());
    }

    private void Update()
    {
        // If the player moved a certain distance, add a new tail segment
        if (Vector3.Distance(transform.position, lastPosition) > segmentSpacing)
        {
            lastPosition = transform.position;
            tailPositions.Enqueue(transform.position);

            // If the tail is too long, remove an old segment
            if (tailPositions.Count > tailLength)
            {
                tailPositions.Dequeue();
            }

            UpdateLineRenderer();
        }

        transform.Translate(direction * speed * Time.deltaTime);

        // if (movingRight)
        // {
        //     direction = Vector2.right;
        // }
        // else
        // {
        //     direction = Vector2.left;
        // }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = tailPositions.Count;
        lineRenderer.SetPositions(tailPositions.ToArray());
    }

    void SpawnSnake() {
        transform.position = new Vector2(player.transform.position.x - 30, player.transform.position.y);
    }

    IEnumerator DieAfterSeconds() {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Destructible_Environment") {
            if(!ignoreCollisions) {
                StartCoroutine(DivertPath());
            }
            
        }
    }

    IEnumerator DivertPath() {
        ignoreCollisions = true;
        Vector2[] pathOptions = { Vector2.down, Vector2.up };
        direction = pathOptions[UnityEngine.Random.Range(0, pathOptions.Length)];
        yield return new WaitForSeconds(1);
        direction = Vector2.right;
        ignoreCollisions = false;
    }
}
