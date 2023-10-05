using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Damage_Dash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy_Health>().currentHealth -= 50;
        }
    }
}
