using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float currentShieldHealth;
    public float startingShieldHealth = 40;

    // Start is called before the first frame update
    void Start()
    {
        currentShieldHealth = startingShieldHealth;
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
                //Play Sound
                //Instantiate Destroy Particles
                Destroy(gameObject);
            }
            // Todo: play sound
            // todo: instantiate shield impact particles
        }
    }
}
