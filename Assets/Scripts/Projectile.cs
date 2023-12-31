using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;
    private AudioManager am;
    private GameObject gm;
    private Data data;
    private Collider2D thisCollider;
    public bool flash = false;
    public Color color1 = Color.red;
    public Color color2 = Color.white;
    public string gunshotSound;

    private string[] barrierImpactSounds = new string[] {"Barrier_Impact_1", "Barrier_Impact_2", "Barrier_Impact_3"};
    public GameObject barrierParticles;
    public GameObject barrierHit;
    public bool rotate;
    public bool dieOnImpact = true;
    public float rotationSpeed = 200f;

    public enum IgnoreList
    {
        Player,
        Destructible_Environment,
        Player_AND_Environment,
        Enemy,
        Default
    }
    public IgnoreList ignoreList;

    private void Start()
    {
        thisCollider = GetComponent<Collider2D>();
        Invoke("DestroySelf", 5f);
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();
        IgnoreOtherProjectiles();
        if(flash) StartCoroutine(Flash(color1, color2));

        switch (ignoreList)
        {
            case IgnoreList.Enemy:
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    Physics2D.IgnoreCollision(thisCollider, enemy.GetComponent<Collider2D>(), true);
                }
                break;
            case IgnoreList.Player:
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player)
                    Physics2D.IgnoreCollision(thisCollider, player.GetComponent<Collider2D>(), true);
                break;
            case IgnoreList.Destructible_Environment:
                foreach (GameObject env in GameObject.FindGameObjectsWithTag("Destructible_Environment"))
                {
                    Physics2D.IgnoreCollision(thisCollider, env.GetComponent<Collider2D>(), true);
                }
                break;
            case IgnoreList.Player_AND_Environment:
                foreach (GameObject env in GameObject.FindGameObjectsWithTag("Destructible_Environment"))
                {
                    Physics2D.IgnoreCollision(thisCollider, env.GetComponent<Collider2D>(), true);
                }
                Physics2D.IgnoreCollision(thisCollider, GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), true);
                break;
            default:
                // Do nothing
                break;
        }
    }

    private void Update() {
        if(rotate) {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "Destructible_Environment") {
            string clip_1 = data.wallImpactSounds[Random.Range(0, data.wallImpactSounds.Length)];
            string clip_2 = data.ricochetSounds[Random.Range(0, data.ricochetSounds.Length)];
            am.Play(clip_1);
            am.Play(clip_2);
        }

        if(other.gameObject.tag == "Destructible_Environment" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            if(dieOnImpact) {
                Destroy(gameObject);
            }
        }

        if(other.gameObject.tag == "Barrier") {
            int randomNum = UnityEngine.Random.Range(0, barrierImpactSounds.Length);
            am.Play(barrierImpactSounds[randomNum]);
            Instantiate(barrierParticles, transform.position, Quaternion.identity);
            Instantiate(barrierHit, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
            Destroy(gameObject);
        }
    }

    private IEnumerator Flash(Color color1, Color color2)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        while (this.gameObject != null)  // Checking if the gameObject exists
        {
            if (sr == null)
            {
                // In case SpriteRenderer gets destroyed or removed
                yield break;
            }

            sr.color = color1;
            yield return new WaitForSeconds(0.05f);

            if (this.gameObject == null) // Check again before changing color
            {
                yield break;
            }

            sr.color = color2;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void IgnoreOtherProjectiles()
    {
        // Get the collider of the current game object
        Collider2D myCollider = GetComponent<Collider2D>();

        // Check if the current game object has a collider
        if (myCollider == null)
        {
            Debug.LogWarning("Current game object does not have a Collider2D component.");
            return;
        }

        // Find all game objects with the "Destructible_Environment" tag
        List<GameObject> allProjectiles = new List<GameObject>();

        GameObject[] enemyProjectiles = GameObject.FindGameObjectsWithTag("Enemy_Projectile");
        GameObject[] playerProjectiles = GameObject.FindGameObjectsWithTag("Player_Projectile");

        allProjectiles.AddRange(enemyProjectiles);
        allProjectiles.AddRange(playerProjectiles);


        // Iterate over each destructible and ignore their collisions with the current game object
        foreach (GameObject projectile in allProjectiles)
        {
            Collider2D collider = projectile.GetComponent<Collider2D>();
            
            if (collider != null)
            {
                Physics2D.IgnoreCollision(myCollider, collider);
            }
        }
    }
}
