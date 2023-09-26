using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloat : MonoBehaviour
{
    public float enlargementSpeed = 1f;
    public float maxSize = 2f;
    public GameObject explosionParticles;
    public GameObject projectilePrefab;
    public int numberOfProjectiles = 8;
    private GameObject player;

    public float shakeMagnitude = 0.1f;  // The magnitude of the shaking effect
    public float shakeFrequency = 1f;    // How fast the object shakes

    private bool isEnlarging = false;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= 10 && !isEnlarging)
        {
            StartEnlarging();
        }

        // Shake logic
        if (transform.localScale.x > maxSize * 0.5f)
        {
            Shake();
        }
        else
        {
            transform.position = initialPosition;
        }
    }

    private void StartEnlarging()
    {
        isEnlarging = true;
        StartCoroutine(Enlarge());
    }

    private IEnumerator Enlarge()
    {
        while (transform.localScale.x < maxSize)
        {
            float scaleValue = enlargementSpeed * Time.deltaTime;
            transform.localScale += new Vector3(scaleValue, scaleValue, 0);
            yield return null;
        }

        Explode();
    }

    private void Shake()
    {
        float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
        float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
        transform.position = initialPosition + new Vector3(offsetX, offsetY, 0);
    }

    private void Explode()
    {
        if (explosionParticles)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            projectile.GetComponent<Projectile>().flash = true;
            projectile.transform.localScale = new Vector3(.2f, .2f, 1f);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.AddForce(projectile.transform.up * 30f, ForceMode2D.Impulse);
            }
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            Collider2D enemyCollider = GetComponent<Collider2D>();

            if (projectileCollider != null && enemyCollider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
            }

            //Ignore collisions with designated objects
            projectile.GetComponent<Projectile>().ignoreList = Projectile.IgnoreList.Enemy;
            }

        Destroy(gameObject);
    }
}
