using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainReaction : MonoBehaviour
{
    public GameObject playerProjectile;
    private AudioManager am;
    public int numberOfProjectiles = 5;
    public float ricochetSpeed = 25;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    public void InitiateChainReaction(GameObject other) {
        if(am != null) {
            am.Play("Chain_Reaction");
        }
        
        for (int i = 0; i < numberOfProjectiles; i++) {
            float randomAngle = Random.Range(0f, 360f);

            Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
            
            GameObject projectile = Instantiate(playerProjectile, transform.position, Quaternion.Euler(0, 0, randomAngle));
            projectile.GetComponent<Projectile>().enabled = true;
            projectile.GetComponent<CircleCollider2D>().enabled = true;

            Collider2D projectileCol = projectile.GetComponent<Collider2D>();
            Collider2D enemyCol = other.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(projectileCol, enemyCol);


            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null) {
                 rb.velocity = direction * ricochetSpeed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy") {
            InitiateChainReaction(other.gameObject);
        }
    }
}
