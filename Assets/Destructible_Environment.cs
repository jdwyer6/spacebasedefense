using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Destructible_Environment : MonoBehaviour
{
    private GameObject gm;
    private Data data;

    public enum DestructionLevel
    {
        full,
        damaged
    }

    //todo get game manager

    public DestructionLevel level;
    // private GameObject piece;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Projectile_Destructible" || other.gameObject.GetComponent<Enemy_Data>().hasLaser) {
            Vector2 contactPoint = other.contacts[0].point;
            Vector2 directionFromProjectile = (Vector2)transform.position - contactPoint;
            
            float rotationAngle = Mathf.Atan2(directionFromProjectile.y, directionFromProjectile.x) * Mathf.Rad2Deg;

            rotationAngle = GetClosestRotation(rotationAngle + 90);

            Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
            if(level == DestructionLevel.damaged) {
                Instantiate(data.destructionParticles, transform.position, quaternion.identity);
                Instantiate(data.dustParticles, transform.position, quaternion.identity);
                Destroy(gameObject);
            } else {
                Instantiate(data.destructionParticles, transform.position, quaternion.identity);
                GameObject piece = data.environmentDamaged[UnityEngine.Random.Range(0, data.environmentDamaged.Length)];
                Instantiate(piece, transform.position, rotation);
                Destroy(gameObject);
            }
        }
    }

    float GetClosestRotation(float angle)
    {
        float[] rotations = { 0, 90, -90, 180 };
        float closestRotation = rotations[0];
        float closestDifference = Mathf.Abs(angle - closestRotation);

        for (int i = 1; i < rotations.Length; i++)
        {
            float difference = Mathf.Abs(angle - rotations[i]);
            if (difference < closestDifference)
            {
                closestDifference = difference;
                closestRotation = rotations[i];
            }
        }

        return closestRotation;
    }
}
