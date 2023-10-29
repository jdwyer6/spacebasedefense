using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Upgrade : MonoBehaviour
{
    private ParticleSystem.Particle[] particles;
    private ParticleSystem _particleSystem;
    public float damageToEnemies = 30;
    private AudioManager am;
    public float timeBetweenStrikes = 15;


    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        _particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        StartCoroutine(InitiateLightning());
    }

    void LateUpdate()
    {
        Transform targetTransform = GetTarget();
        if (targetTransform == null) return; // Don't process if there's no target

        int numParticlesAlive = _particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            float lifeProgress = 1.0f - (particles[i].remainingLifetime / particles[i].startLifetime); // Progress from 0 to 1 over particle's lifetime
            particles[i].position = Vector3.Lerp(transform.position, targetTransform.position, lifeProgress);
        }

        _particleSystem.SetParticles(particles, numParticlesAlive);
    }

    IEnumerator InitiateLightning() {
        while(true) {
            am.Play("Lightning");
            _particleSystem.Play();
            StartCoroutine(DamageEnemy());
            yield return new WaitForSeconds(timeBetweenStrikes); 
        }

    }

    Transform GetTarget() {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyObjects.Length == 0) return null;  // Return null if no enemies exist.

        Transform closestEnemy = enemyObjects[0].transform;
        float closestDistance = Vector3.Distance(transform.position, closestEnemy.position);

        foreach (var enemyObject in enemyObjects)
        {
            Transform enemyTransform = enemyObject.transform;
            float currentDistance = Vector3.Distance(transform.position, enemyTransform.position);

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestEnemy = enemyTransform;
            }
        }

        return closestEnemy;
    }

    IEnumerator DamageEnemy() {
        Transform targetTransform = GetTarget();
        yield return new WaitForSeconds(.2f);
        if (targetTransform != null) {
            targetTransform.GetComponent<Enemy_Health>().TakeDamage(damageToEnemies);
        }

    }
}
