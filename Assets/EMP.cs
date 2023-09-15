using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
    public GameObject empParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            Instantiate(empParticles, transform.position, Quaternion.identity);
        }

    }

    private void LaunchEMP() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if(rb != null){
                rb.gravityScale = 1;
            }
        }
    }
}

// add timer bar
// start coroutine for enemies fall and die
// clear all projectiles
// add sound
// hook up with upgrades