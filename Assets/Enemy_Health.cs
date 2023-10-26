using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public float totalHealth = 3;
    public float currentHealth;
    private GameObject gm;
    private Data data;
    public GameObject bloodParticlesExit;
    public GameObject bloodParticlesDeath;
    private AudioManager am;
    private GameObject player;
    public GameObject pickup;
    public int percentToDropPickups = 90;
    public GameObject eyeDeathParticles;
    bool isChangingColor = false;
    public bool hasBossDrop = false;

    public bool spawnMoreEnemiesAtDeath;
    public GameObject enemyToSpawnAtDeath;
    public int numberOfEnemiesToSpawnAtDeath;

    private GameObject[] bulletHolePrefabs;

    public GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();
        am = FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletHolePrefabs = data.bulletHolePrefabs;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player_Projectile") {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
            if(GetComponent<Enemy_Data>().showBloodHit) {
                ShowBloodHit(other.gameObject);
            }

            am.Play(data.bloodHits[Random.Range(0, data.bloodHits.Length)]);

            Vector2 incomingDirection = (other.transform.position - transform.position).normalized;
            Vector2 oppositeDirection = -incomingDirection;
            float rotationZ = Mathf.Atan2(oppositeDirection.y, oppositeDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, rotationZ -90); 
            Instantiate(bloodParticlesExit, transform.position, rotation);
        }
        if(other.gameObject.tag == "Projectile_NonDestructible") {
            am.Play(data.bloodHits[Random.Range(0, data.bloodHits.Length)]);
            Die();
        }
    }

    public void TakeDamage(float damageToTake) {
        currentHealth -= damageToTake;
        // ShowBloodHit();
        StartCoroutine(ChangeColor());
    }

    void Die() {
        if(hasBossDrop) {
            GetComponent<Boss_Health_Bar>().bossSliderContainer.SetActive(false);
            // GameObject randomDrop = data.bossDrops[UnityEngine.Random.Range(0, data.bossDrops.Length)];
            // Instantiate(randomDrop, transform.position, Quaternion.identity);
        }
        player.GetComponent<Building>().bricks += 2;

        for (int i = 0; i < GetComponent<Enemy_Data>().numOfCoins; i++)
        {
            Instantiate(coin, new Vector2(transform.position.x + UnityEngine.Random.Range(0, 1f), transform.position.y + UnityEngine.Random.Range(0, 1f)), Quaternion.identity);
        }
        

        var drops = gm.GetComponent<Data>().drops;
        if(drops.Count > 0 && !hasBossDrop) {
            GetComponent<Drops>().DropItem(transform.position);
        }

        if(WillDropPickup()) Instantiate(pickup, transform.position, Quaternion.identity);
        if(GetComponent<Enemy_Eye>()) {
            Instantiate(eyeDeathParticles, transform.position, Quaternion.identity);
        }else{
            Instantiate(bloodParticlesDeath, transform.position, Quaternion.identity);
        }

        if(spawnMoreEnemiesAtDeath) {
            for (int i = 0; i < numberOfEnemiesToSpawnAtDeath; i++)
            {
                float randomNum = UnityEngine.Random.Range(0, 5);
                Instantiate(enemyToSpawnAtDeath, new Vector2(transform.position.x + randomNum, transform.position.y + randomNum), Quaternion.identity);
            }
            
        }
        Destroy(gameObject);
    }

    IEnumerator ChangeColor() {
        if(isChangingColor)
            yield break;
        isChangingColor = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color currentColor = spriteRenderer.color;
        if(spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1.0f, 0.286f, 0.286f);
        }
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = currentColor;
        isChangingColor = false;
    }

    void ShowBloodHit(GameObject collisionObject) 
    {
        Vector2 hitPoint = collisionObject.transform.position;
        Vector2 centerPoint = transform.position;
        float distanceToCenter = Vector2.Distance(hitPoint, centerPoint);

        float minLerp = 0.5f / distanceToCenter;
        float randomLerpValue = UnityEngine.Random.Range(minLerp, 1f);

        Vector2 instantiatePosition = Vector2.Lerp(hitPoint, centerPoint, randomLerpValue);

        float randomZ = UnityEngine.Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);

        GameObject bloodHit = Instantiate(bulletHolePrefabs[UnityEngine.Random.Range(0, bulletHolePrefabs.Length)], instantiatePosition, randomRotation);
        bloodHit.transform.SetParent(this.transform);
    }

    bool WillDropPickup() {
        if(player.GetComponent<Upgrades>().AllUpgradesHaveBeenAcquired()) {
            return false;
        }
        int randomNum = UnityEngine.Random.Range(0, 100);
        if(randomNum <= percentToDropPickups) {
            return true;
        }
        return false;
    }
}
