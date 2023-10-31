using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public float damage = 100;
    private AudioManager am;
    public float damageRadius = 10f;
    public GameObject explosionParticles;

    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            StartCoroutine(InitiateExplosion());
        }
    }

    private IEnumerator InitiateExplosion() {
        am.Play("Beep");
        yield return new WaitForSeconds(.5f);
        Explode();
    }

    private void Explode() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if(distance <= damageRadius) {
                enemy.GetComponent<Enemy_Health>().TakeDamage(damage);
            }
        }
        Instantiate(explosionParticles, transform.position, Quaternion.identity);
        am.Play("Mine_Explosion");
        Destroy(gameObject);

    }
}
