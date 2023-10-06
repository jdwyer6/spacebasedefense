using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float currentShieldHealth;
    public float startingShieldHealth = 40;
    private AudioManager am;
    public GameObject shieldHitParticles;
        public GameObject shieldDestroyedParticles;

    // Start is called before the first frame update
    void Start()
    {
        currentShieldHealth = startingShieldHealth;
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy_Projectile") {
            currentShieldHealth -= other.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);
            if(currentShieldHealth <= 0) {
                am.Play("Shield_Destroyed");
                Instantiate(shieldDestroyedParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            am.Play("Shield_Hit");
            Instantiate(shieldHitParticles, other.transform.position, Quaternion.identity);
        }
    }
}
