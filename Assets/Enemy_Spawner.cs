using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy Properties")]
    public GameObject gm;
    public Data data;
    public GameObject[] enemies;
    public float distanceToSpawnAwayFromPlayer = 20;
    public bool waveActive = true;
    public bool spawnEnemy = true;

    [Header("Wave Properties")]
    public int level = 1;
    public TextMeshProUGUI waveText;

    [Header("Multipliers")]
    public float levelLengthMultiplier = 1.25f;
    public float enemyMovementSpeedMultiplier = 1;
    public float enemyMovementSpeedMultiplierAmountToAddEachLevel = 0.2f;
    public float spawnIntervalMultipler = .9f;

    [Header("Cooldown Settings")]
    public bool coolDown = false;
    public float coolDownPeriod = 10;

    [Header("Internal References")]
    private Transform player;
    public float timer;

    [Header("UI")]
    public GameObject buildTip;

    [Header("Variable Difficulty")]
    private float spawnInterval = 4;
    private float levelLength = 20;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();
        enemies = data.enemies;
        
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        StartCoroutine(SpawnEnemy());
        timer = levelLength;
        waveText.text = "Wave " + level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(waveActive && timer <= 0) {
            spawnEnemy = false;
            if(GetRemainingEnemies() <= 0) {
                waveActive = false;
                timer = coolDownPeriod;
                coolDown = true;
                waveText.text = "Cool Down";
            }
        }

        if(coolDown && timer <=0) {
            coolDown = false;
            levelLength = levelLength * levelLengthMultiplier;
            spawnInterval = spawnInterval * spawnIntervalMultipler;
            timer = levelLength;
            waveActive = true;
            spawnEnemy = true;
            level++;
            enemyMovementSpeedMultiplier += enemyMovementSpeedMultiplierAmountToAddEachLevel;
            waveText.text = "Wave " + level.ToString();
            StartCoroutine(SpawnEnemy());
        }

        if(coolDown){
            buildTip.SetActive(true);
        }else{
            buildTip.SetActive(false);
        }
    }

    IEnumerator SpawnEnemy() {
        while (spawnEnemy)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if(ShouldSpawnEnemy(enemies[i].GetComponent<Enemy_Data>().probabilityToSpawn, i)){
                    Instantiate(enemies[i], GetRandomSpawnPos(), Quaternion.identity);
                } 
            }

            yield return new WaitForSeconds(spawnInterval);
        }

    }

    Vector2 GetRandomSpawnPos() {
            float randomAngle = Random.Range(-180f, 180f) * Mathf.Deg2Rad; 
            Vector2 direction = new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)); 
            Vector2 pos = player.transform.position + (Vector3)(direction * 20f);
            return pos;
    }

    bool ShouldSpawnEnemy(int probability, int idx) {
        int randomNum = UnityEngine.Random.Range(0, 100);
        if(randomNum <= probability) {
            if(enemies[idx].GetComponent<Enemy_Data>().levelToSpawn <= level){
                return true;
            }  
        }
        return false;
    }

    int GetRemainingEnemies() {
        var remainingEnemies = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            remainingEnemies++;
        }
        return remainingEnemies;
    }
}
