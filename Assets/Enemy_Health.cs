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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();
        am = FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        ShowBloodHit();
        StartCoroutine(ChangeColor());
    }

    void Die() {
        if(hasBossDrop) {
            GetComponent<Boss_Health_Bar>().bossSliderContainer.SetActive(false);
            GameObject randomDrop = data.bossDrops[UnityEngine.Random.Range(0, data.bossDrops.Length)];
            Instantiate(randomDrop, transform.position, Quaternion.identity);
        }
        player.GetComponent<Building>().bricks += 2;
        if(WillDropPickup()) Instantiate(pickup, transform.position, Quaternion.identity);
        if(GetComponent<Enemy_Eye>()) {
            Instantiate(eyeDeathParticles, transform.position, Quaternion.identity);
        }else{
            Instantiate(bloodParticlesDeath, transform.position, Quaternion.identity);
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

    void ShowBloodHit() 
    {
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
        List<GameObject> bloodHitsList = new List<GameObject>();
        
        foreach (Transform child in allChildren)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (child.CompareTag("Blood_Hit") && sr != null && !sr.enabled)
            {
                bloodHitsList.Add(child.gameObject);
            }
        }

        if(bloodHitsList.Count > 0) 
        {
            int randomIndex = Random.Range(0, bloodHitsList.Count);
            SpriteRenderer selectedSR = bloodHitsList[randomIndex].GetComponent<SpriteRenderer>();
            if(selectedSR != null) 
            {
                selectedSR.enabled = true;

                // Check for the "Blood_Drops" child and activate it
                foreach (Transform childOfSelected in bloodHitsList[randomIndex].transform)
                {
                    if (childOfSelected.CompareTag("Blood_Drops"))
                    {
                        childOfSelected.gameObject.SetActive(true);
                    }
                }
            }
        }
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
