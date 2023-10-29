using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Laser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy_Health>().TakeDamage(50f);
        }
    }
}
