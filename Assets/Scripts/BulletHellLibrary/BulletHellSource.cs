using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingStyle
{
    Spiral,
    Default,
    Sine,
    Explode
}

//Parameters: Transform barrelPosition, float shootingDuration, float shootInterval, float restDuration, GameObject projectilePrefab, string sound, float speed, bool aimAtPlayer, int rotationSpeed, float size
public class BulletHellSource : MonoBehaviour
{
    private GameObject gm;
    public ShootingStyle selectedStyle;

    [Header("Settings")]
    public Transform barrelPosition;
    public float shootingDuration;
    public float shootInterval;
    public float restDuration;
    public float speed;
    public float size;
    public GameObject projectilePrefab;
    public string sound;
    public bool aimAtPlayer;
    public int rotationSpeed;
    public float distanceFromPlayerToStartShooting;
    public GameObject flashParticles;
    public float damageToPlayer = 10;
    public bool isExplosion;
    public int numberOfProjectiles;
 

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");

        switch (selectedStyle)
        {
            case ShootingStyle.Spiral:
                StartCoroutine(gm.GetComponent<BulletHellLibrary>().Spiral(barrelPosition, shootingDuration, shootInterval, restDuration, projectilePrefab, sound, speed, aimAtPlayer, rotationSpeed, size, distanceFromPlayerToStartShooting, flashParticles, damageToPlayer, 1, false));
                break;
            case ShootingStyle.Default:
                StartCoroutine(gm.GetComponent<BulletHellLibrary>().Spiral(barrelPosition, shootingDuration, shootInterval, restDuration, projectilePrefab, sound, speed, aimAtPlayer, 0, size, distanceFromPlayerToStartShooting, flashParticles, damageToPlayer, 1, false));
                break;
            case ShootingStyle.Sine:
                // Start Sine method
                break;
            case ShootingStyle.Explode:
                StartCoroutine(gm.GetComponent<BulletHellLibrary>().Spiral(barrelPosition, shootingDuration, shootInterval, restDuration, projectilePrefab, sound, speed, aimAtPlayer, 0, size, distanceFromPlayerToStartShooting, flashParticles, damageToPlayer, numberOfProjectiles, true));
                break;
            default:
                Debug.LogWarning("Unrecognized shooting style.");
                break;
        }
    }
}
