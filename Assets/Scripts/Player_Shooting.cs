using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.UI;

public class Player_Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject defaultPlayerProjectile;
    private string gunshotSound;

    public float projectileSpeedOriginal = 10f;
    public float projectileSpeed = 10f; 
    public Vector2 shootingOffset = new Vector2(0, 0.5f); 
    private AudioManager am;
    private GameObject gm;
    public float autoShootingIntervalOriginal = .2f;
    public float autoShootingInterval = .2f;
    public GameObject flamingProjectiles;
    public CinemachineImpulseSource impulseSource;
    public GameObject muzzleParticles;
    private bool[] shootIdx = new bool[] {false, false, false, false};
    private bool isShooting;
    public bool instakillActive;
    // UpgradeLogicType auto = UpgradeLogicType.auto;
    UpgradeLogicType arsen = UpgradeLogicType.arsen;
    UpgradeLogicType spread = UpgradeLogicType.spread;

    [Header("Reloading")]
    private int originalStartingAmmo = 10;
    public int currentAmmo;
    public int magazineCapacity;
    public Slider ammoAndReloadIndicator;

    public GameObject laser;
    public bool laserActive;

    private void Start() {
        SetProjectile(defaultPlayerProjectile);
        projectileSpeed = projectileSpeedOriginal;
        autoShootingInterval = autoShootingIntervalOriginal;
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");

        magazineCapacity = originalStartingAmmo;
        currentAmmo = magazineCapacity;
    }

    void Update()
    {
        if(GameGlobals.Instance.globalMenuOpen == true) return;
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ResetShootIndex(0);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ResetShootIndex(1);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            ResetShootIndex(2);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            ResetShootIndex(3);
        }

        // Controller support
        // if (Input.GetAxis("Fire1") > .1f ) {
        //     float flashRotation = transform.eulerAngles.z;
        //     isShooting = true;
        //     Debug.Log("Fire!");
        //     am.Play(gunshotSound);
        //     // StartCoroutine(Fire(Vector2.up, KeyCode.UpArrow, flashRotation, new Vector3(0, 1, 0)));
        // }


        if(shootIdx[0] && !isShooting) {
            isShooting = true;
            float flashRotation = -90f;
            StartCoroutine(Fire(Vector2.up, KeyCode.UpArrow, flashRotation, new Vector3(0, 1, 0)));
        }else if(shootIdx[1] && !isShooting) {
            float flashRotation = 180f;
            StartCoroutine(Fire(Vector2.right, KeyCode.RightArrow, flashRotation, new Vector3(1, 0, 0)));
        }else if(shootIdx[2] && !isShooting) {
            float flashRotation = 90f;  
            StartCoroutine(Fire(Vector2.down, KeyCode.DownArrow, flashRotation, new Vector3(0, -1, 0)));
        }else if(shootIdx[3] && !isShooting) {
            float flashRotation = -180f;
            StartCoroutine(Fire(Vector2.left, KeyCode.LeftArrow, flashRotation, new Vector3(-1, 0, 0)));
        }
    }

    void LaunchProjectile(Vector2 direction)
    {
        am.Play(gunshotSound);
        if(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == spread).acquired){
            ShootSpread(direction);
        }

        impulseSource.GenerateImpulse();
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, (Vector2)transform.position, Quaternion.identity);
        if(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == arsen).acquired) {
            GameObject particles = Instantiate(flamingProjectiles, projectile.transform.position, Quaternion.identity);
            particles.transform.SetParent(projectile.transform);
            projectile.GetComponent<Projectile>().damage*=1.5f;
        }

        if(instakillActive) {
            projectile.GetComponent<Projectile>().damage = 100;
        }

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.ignoreList = Projectile.IgnoreList.Destructible_Environment;

            Collider2D playerCollider = this.GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();

            // Ensure collisions between the player and their projectiles are ignored
            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Projectile script component!");
        }

        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody2D component!");
        }

    }

    IEnumerator Fire(Vector2 direction, KeyCode key, float flashRotation, Vector3 offset){
        currentAmmo -= 1;
        isShooting = true;
        while(Input.GetKey(key)){
            LaunchProjectile(direction);
            var particles = Instantiate(muzzleParticles, transform.position + offset, Quaternion.identity);
            SetFlashRotation(particles, flashRotation);
            if(laserActive) {
                StartCoroutine(FireLaser(flashRotation));
            }
            yield return new WaitForSeconds(autoShootingInterval);
        }
        isShooting = false;
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

    void ResetShootIndex(int idx) {
        for (int i = 0; i < shootIdx.Length; i++)
        {
            shootIdx[i] = false;
        }
        shootIdx[idx] = true;
    }

    private Upgrade[] GetUpgrades() {
        return gm.GetComponent<Data>().upgrades;

    }

    void ShootSpread(Vector2 mainDirection)
    {
        Vector2 leftDirection = Quaternion.Euler(0, 0, 20) * mainDirection;
        Vector2 rightDirection = Quaternion.Euler(0, 0, -20) * mainDirection;
        
        // Directly instantiate the projectiles for the left and right directions
        InstantiateProjectile(leftDirection, 3.5f);
        InstantiateProjectile(rightDirection, 3.5f);
    }

    void InstantiateProjectile(Vector2 direction, float damage)
    {
        // Your logic to instantiate the projectile and set its properties
        GameObject projectile = Instantiate(projectilePrefab, (Vector2)transform.position + shootingOffset, Quaternion.identity);
        
        if(instakillActive) {
            projectile.GetComponent<Projectile>().damage = 100;
        }else{
            projectile.GetComponent<Projectile>().damage = damage;
        }
        if(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == arsen).acquired) {
            GameObject particles = Instantiate(flamingProjectiles, projectile.transform.position, Quaternion.identity);
            particles.transform.SetParent(projectile.transform);
            projectile.GetComponent<Projectile>().damage*=1.5f;
        }
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = direction * projectileSpeed;
        }
        else {
            Debug.LogError("Projectile prefab does not have a Rigidbody2D component!");
        }
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.ignoreList = Projectile.IgnoreList.Destructible_Environment;

            Collider2D playerCollider = this.GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();

            // Ensure collisions between the player and their projectiles are ignored
            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }
        }
    }

    IEnumerator FireLaser(float rotation) {
        laser.SetActive(true);
        switch (rotation)
        {
            case 90:
                rotation = -180;
                break;
            case 180:
                rotation = -90;
                break;
            case -90:
                rotation = 0;
                break;
            case -180:
                rotation = 90;
                break;
        }
        am.Play("Laser_Projectile");
        laser.transform.rotation = Quaternion.Euler(0, 0, rotation);
        yield return new WaitForSeconds(.05f);
        laser.SetActive(false);
    }

    public void SetProjectile(GameObject projectile) {
        projectilePrefab = projectile;
        gunshotSound = projectile.GetComponent<Projectile>().gunshotSound;
    }
}