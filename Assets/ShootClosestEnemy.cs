using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootClosestEnemy : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public float rateOfFire = .1f;
    public float projectileSpeed = 25f; 
    public float timeBetweenShots = 10;
    private AudioManager am;
    bool amFound;
    public string gunshotSound;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        StartCoroutine(ShootEnemy());
    }

    private void Update() {
        if(am == null && amFound == false) {
            am = FindObjectOfType<AudioManager>();
        }else{
            amFound = true;
        }
    }

    IEnumerator ShootEnemy() {
        while(true) {
            StartCoroutine(ThreeRoundBurst());
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    IEnumerator ThreeRoundBurst() {
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            yield return new WaitForSeconds(rateOfFire);
        }
        
    }

    private void Shoot() {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector2 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
            if (am != null) {
                am.Play(gunshotSound);
            }else{
                Debug.Log("audio manager is null");
            }
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = directionToEnemy * projectileSpeed;
            }
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector2 position = transform.position;
        
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, position);
            if (distance < minDistance)
            {
                closest = enemy;
                minDistance = distance;
            }
        }
        
        return closest;
    }
}
