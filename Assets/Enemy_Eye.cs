using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eye : MonoBehaviour
{
    private Transform player; 
    public Transform pupil; 
    private AudioManager am;

    public float lookRadius = 0.3f; 
    bool screamPlayed = false;

    public GameObject deathParticles;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        am = FindObjectOfType<AudioManager>();
        StartCoroutine(DieAfterSeconds());
    }

    private void Update()
    {
        LookAtPlayer();
        if(transform.position.y < player.position.y + 15 && !screamPlayed) {
            screamPlayed = true;
            am.Play("Eye_Scream");
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();

        pupil.position = transform.position + directionToPlayer * lookRadius;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Destructible_Environment") {
            am.Play("Eye_Break");
            am.Stop("Eye_Scream");
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator DieAfterSeconds() {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
