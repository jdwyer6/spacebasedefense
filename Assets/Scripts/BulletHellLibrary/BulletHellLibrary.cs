using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellLibrary : MonoBehaviour
{
    private GameObject player;
    private GameObject gm;
    private AudioManager am;

    private void Awake() {
        am = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GM");
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
        float damageToPlayer,
        int numberOfProjectiles,
        bool isExplosion
        ) {
        while (true)
        {
            float rotation = 0;
            for (float t = 0; t < shootingDuration; t += shootInterval)
            {
                ShootProjectile(barrelPosition, projectilePrefab, sound, speed, aimAtPlayer, rotation, size, distanceFromPlayerToStartShooting, flashParticles, damageToPlayer, numberOfProjectiles, isExplosion);
                yield return new WaitForSeconds(shootInterval);
                rotation += rotationSpeed;
            }
            yield return new WaitForSeconds(restDuration);
        }
    }




    void ShootProjectile(
        Transform barrelPosition, 
        GameObject projectilePrefab, 
        string sound, 
        float speed, 
        bool aimAtPlayer, 
        float rotation, 
        float size, 
        float distanceFromPlayerToStartShooting, 
        GameObject flashParticles, 
        float damageToPlayer, 
        int numberOfProjectiles,
        bool isExplosion
        )
    {
        // if(Vector3.Distance(barrelPosition.position, player.transform.position) > distanceFromPlayerToStartShooting) return;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Quaternion tempRotation = Quaternion.Euler(0, 0, 0);
            if(isExplosion) {
                tempRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }else{
                tempRotation = barrelPosition.rotation;
            }
            
            GameObject projectile = Instantiate(projectilePrefab, barrelPosition.position, tempRotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if(isExplosion) {
                rb.AddForce(projectile.transform.up * speed, ForceMode2D.Impulse);
                Instantiate(flashParticles, barrelPosition.position, Quaternion.identity);
            }else{
                barrelPosition.Rotate(0f, 0f, -rotation);
                Vector2 direction = barrelPosition.up;

                if(aimAtPlayer && player != null) 
                {
                    direction = (player.transform.position - barrelPosition.position).normalized;
                }
                rb.velocity = direction * speed;

                if(flashParticles != null) {
                    Vector3 flashPosition = barrelPosition.position + new Vector3(direction.x, direction.y, 0);
                    float rotationDegree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var particles = Instantiate(flashParticles, flashPosition, Quaternion.identity);
                    SetFlashRotation(particles, rotationDegree);
                }
            }


            
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

        }
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