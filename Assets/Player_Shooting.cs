using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f; 
    public Vector2 shootingOffset = new Vector2(0, 0.5f); 
    private AudioManager am;
    public float autoShootingInterval = .2f;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(GetComponent<Upgrades>().autoAcquired){
                StartCoroutine(ShootAutomatic(Vector2.up, KeyCode.UpArrow));
            }else{
                Shoot(Vector2.up);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(GetComponent<Upgrades>().autoAcquired){
                StartCoroutine(ShootAutomatic(Vector2.down, KeyCode.DownArrow));
            }else{
                Shoot(Vector2.down);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(GetComponent<Upgrades>().autoAcquired){
                StartCoroutine(ShootAutomatic(Vector2.left, KeyCode.LeftArrow));
            }else{
                Shoot(Vector2.left);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(GetComponent<Upgrades>().autoAcquired){
                StartCoroutine(ShootAutomatic(Vector2.right, KeyCode.RightArrow));
            }else{
                Shoot(Vector2.right);
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        am.Play("Shot");

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, (Vector2)transform.position, Quaternion.identity);
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

    IEnumerator ShootAutomatic(Vector2 direction, KeyCode key){
        while(Input.GetKey(key)){
            Shoot(direction);
            yield return new WaitForSeconds(autoShootingInterval);
        }

    }
}
