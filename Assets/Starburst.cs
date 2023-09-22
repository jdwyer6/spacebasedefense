using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starburst : MonoBehaviour
{
    public float burstInterval = 5f;
    public int numberPerBurst = 5;
    public float projectileSpeed = 10f;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitiateStarburst());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InitiateStarburst() {
        while(true) {
            ShootStarburst();
            yield return new WaitForSeconds(burstInterval);
        }

        
    }

    void ShootStarburst()
    {
        int numberOfProjectiles = 5;
        float angleStep = 360f / numberOfProjectiles;
        float currentAngle = 0f;

        for(int i = 0; i < numberOfProjectiles; i++)
        {
            float projectileDirX = transform.position.x + Mathf.Sin((currentAngle * Mathf.PI) / 180f);
            float projectileDirY = transform.position.y + Mathf.Cos((currentAngle * Mathf.PI) / 180f);
            
            Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);
            Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized * projectileSpeed;
            
            GameObject tmpObj = Instantiate(projectile, transform.position, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            // Physics2D.IgnoreCollision(tmpObj.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            currentAngle += angleStep;
        }
    }
}
