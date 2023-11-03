using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Manager : MonoBehaviour
{
    private float timer;
    public float timerStart;
    private AudioManager am;

    public float pulseMag;
    public float pulseSpeed;

    private Juicer juicer;

    public GameObject enemy;
    public Transform enemySpawnPosition1;
    public Transform enemySpawnPosition2;
    public GameObject dustParticles;

    bool hasTriggeredSpawn;
    bool generateEnvironment;

    // Start is called before the first frame update
    void Start()
    {
        juicer = GameObject.FindGameObjectWithTag("GM").GetComponent<Juicer>();
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D)) {
            timer = timerStart;
        } else {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && hasTriggeredSpawn == false) {
            Instantiate(enemy, enemySpawnPosition1.position, Quaternion.identity);
            Instantiate(enemy, enemySpawnPosition2.position, Quaternion.identity);
            am.Play("Spawn");
            Instantiate(dustParticles, enemySpawnPosition1.position, Quaternion.identity);
            Instantiate(dustParticles, enemySpawnPosition2.position, Quaternion.identity);
            hasTriggeredSpawn = true;
        }
    }

}
