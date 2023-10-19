using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy Properties")]
    public GameObject gm;
    public Data data;
    public GameObject[] enemies;
    public float distanceToSpawnAwayFromPlayer = 20;
    // public bool waveActive = true;
    // public bool spawnEnemy = true;
    bool exploitTimerRunning = false;
    bool forceNextCoolDown = false;
    // bool isTutorial = false;
    // bool spawnInitialized = false;

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
    public float breakTimer;
    public float breakTimerStart = 10f;

    [Space]

    public int[] bossLevels;
    public List<Wave> waves = new List<Wave>();

    // [Header("UI")]
    // public GameObject buildTip;

    [Header("Variable Difficulty")]
    private float spawnInterval = 4;
    private float levelLength = 20;

    public bool isBreak;
    public bool spawningActive;
    private bool waitForBossToDie;



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

        foreach (var wave in GetComponent<Data>().waves)
        {
            wave.hasSpawned = false;
            waves.Add(wave);
        }
        // if(!isTutorial) {
            StartCoroutine(SpawnEnemy());
        // }

        waveText.text = "Wave " + level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // if(!isTutorial && !spawnInitialized) {
        //     StartCoroutine(SpawnEnemy());
        //     spawnInitialized = true;
        // }

        if(isBreak) {
            breakTimer -= Time.deltaTime;

            if(breakTimer <= 0 && !spawningActive) {
                isBreak = false;
                TriggerNextWave();
            }
        }

        if(waitForBossToDie) {
            WaitForBossToDie();
        }
    }

    IEnumerator SpawnEnemy() {
        spawningActive = true;
        List<Wave> wavePool = GetCurrentPool();
        CheckWavesLeftAtDifficultyLevel(wavePool);
        Wave randomWave = wavePool[UnityEngine.Random.Range(0, wavePool.Count)];
        Debug.Log(randomWave.title);

        for (int i = 0; i < randomWave.numberOfSpawnCycles; i++)
        {
            for (int j = 0; j < randomWave.enemies.Length; j++)
            {
                Instantiate(randomWave.enemies[j], GetRandomSpawnPos(), Quaternion.identity);
            }
            yield return new WaitForSeconds(randomWave.timeBetweenCycle);
        }

        spawningActive = false;

        if(!checkIfBossLevel()) {
            TriggerBreak();
        }else{
            waitForBossToDie = true;
        }

    }

    Vector2 GetRandomSpawnPos() {
        float randomAngle = Random.Range(-180f, 180f) * Mathf.Deg2Rad; 
        Vector2 direction = new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)); 
        if (player == null) return Vector3.zero + (Vector3)(direction * 25f); // fix null player on initial spawn at restart
        Vector2 pos = player.transform.position + (Vector3)(direction * 25f);
        return pos;
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

    private void TriggerBreak() {
        breakTimer = breakTimerStart;
        isBreak = true;
    }

    private void TriggerNextWave() {
        level++;
        StartCoroutine(SpawnEnemy());
    }

    private bool checkIfBossLevel() {
        foreach (var bossLevel in bossLevels)
        {
            if(bossLevel == level) {
                return true;
            }
        }
        return false;
    }

    private void WaitForBossToDie() {
        if(GetRemainingEnemies() <= 0) {
            TriggerNextWave();
            waitForBossToDie = false;
        }
    }

    private List<Wave> GetCurrentPool() {
        if(checkIfBossLevel()) {
            return waves.Where(w => w.isBoss == true).ToList();
        }
        switch (level)
        {
            case 1:
                return waves.Where(w => w.difficulty == Difficulty.Opening).ToList();
            case var n when n > 1 && n < 5:
                return waves.Where(w => w.difficulty == Difficulty.Easy).ToList();
            case var n when n > 4 && n < 9:
                return waves.Where(w => w.difficulty == Difficulty.Medium).ToList();
            case var n when n > 9:
                return waves.Where(w => w.difficulty == Difficulty.Hard).ToList();
            default:
                Debug.LogError("Unrecognized difficulty setting.");
                return waves;
        }
    }

    private void CheckWavesLeftAtDifficultyLevel(List<Wave> wavePool) {
        foreach (var wave in wavePool)
        {
            if(!wave.hasSpawned) {
                return;
            }
        }
        // Reset each wave to has not spawned if all waves have spawned
        foreach (var wave in wavePool)
        {
            wave.hasSpawned = false;
        }
    }
    
}
