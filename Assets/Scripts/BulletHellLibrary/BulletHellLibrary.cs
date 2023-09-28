using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellLibrary : MonoBehaviour
{
    private GameObject player;
    private AudioManager am;

    private void Awake() {
        am = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public IEnumerator Spiral(
        Transform barrelPosition, 
        float shootingDuration, 
        float shootInterval, 
        float restDuration, 
        GameObject projectilePrefab, 
        string sound, 
        float speed, 
        bool aimAtPlayer, 
        float rotationSpeed, 
        float size,
        float distanceFromPlayerToStartShooting,
        GameObject flashParticles, 
        float damageToPlayer
        ) {
        while (true)
        {
            float rotation = 0;
            for (float t = 0; t < shootingDuration; t += shootInterval)
            {
                ShootProjectile(barrelPosition, projectilePrefab, sound, speed, aimAtPlayer, rotation, size, distanceFromPlayerToStartShooting, flashParticles, damageToPlayer);
                yield return new WaitForSeconds(shootInterval);
                rotation += rotationSpeed;
            }
            yield return new WaitForSeconds(restDuration);
        }
    }




    void ShootProjectile(Transform barrelPosition, GameObject projectilePrefab, string sound, float speed, bool aimAtPlayer, float rotation, float size, float distanceFromPlayerToStartShooting, GameObject flashParticles, float damageToPlayer)
    {
        // if(Vector3.Distance(barrelPosition.position, player.transform.position) > distanceFromPlayerToStartShooting) return;
        GameObject projectile = Instantiate(projectilePrefab, barrelPosition.position, barrelPosition.rotation);
        IgnoreCollisionsWithSelf(projectile);
        projectile.GetComponent<Projectile>().flash = true;
        projectile.transform.localScale = new Vector3(size, size, 1f);
        projectile.GetComponent<Projectile>().damage = damageToPlayer;
        am.Play(sound);
        SpriteRenderer sr = projectile.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
        }
        
        barrelPosition.Rotate(0f, 0f, -rotation);
        Vector2 direction = barrelPosition.up;

        if(aimAtPlayer && player != null) 
        {
            direction = (player.transform.position - barrelPosition.position).normalized;
        }

        if(flashParticles != null) {
            Vector3 flashPosition = barrelPosition.position + new Vector3(direction.x, direction.y, 0);
            var particles = Instantiate(flashParticles, flashPosition, Quaternion.identity);
            float rotationDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SetFlashRotation(particles, rotationDegree);
        }
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        rb.velocity = direction * speed;
    }

    private void SetFlashRotation(GameObject particles, float rotationDegree)
    {
        Transform flashTransform = particles.transform.Find("Flash");
        if (flashTransform != null)
        {
            ParticleSystem flashParticles = flashTransform.GetComponent<ParticleSystem>();
            if (flashParticles != null)
            {
                var main = flashParticles.main;
                main.startRotation = rotationDegree * Mathf.Deg2Rad;
            }
        }
    }

    private void IgnoreCollisionsWithSelf(GameObject projectile) {
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        StartCoroutine(EnableColliderAfterDelay(projectileCollider, 0.1f));
        projectile.GetComponent<Projectile>().ignoreList = Projectile.IgnoreList.Enemy;
        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (projectileCollider != null && enemyCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }
    }

    private IEnumerator EnableColliderAfterDelay(Collider2D col, float delay) 
    {
        col.enabled = false;
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }

}