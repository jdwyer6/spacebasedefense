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

    public bool isBreak;
    public bool spawningActive;
    private bool waitForBossToDie;

    public float extremumX = 40;
    public float extremumY = 25;



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
            wave.probabilityToSpawn = 100;
            waves.Add(wave);
        }

        if(GetComponent<Environment_Generator>().initialBaseEnvironmentRotation == 90 || GetComponent<Environment_Generator>().initialBaseEnvironmentRotation == -90) {
            extremumX = 25;
            extremumY = 40;
        }

        StartCoroutine(SpawnEnemy());
        
        waveText.text = "Wave " + level.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if(isBreak) {
            breakTimer -= Time.deltaTime;

            if(breakTimer <= 0 && !spawningActive) {
                isBreak = false;
                TriggerNextWave();
            }
        }
    }

    IEnumerator SpawnEnemy() {
        spawningActive = true;
        List<Wave> wavePool = GetCurrentPool();
        CheckWavesLeftAtDifficultyLevel(wavePool);
        Wave randomWave = wavePool[UnityEngine.Random.Range(0, wavePool.Count)];
        randomWave.hasSpawned = true;

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
            StartCoroutine(WaitForBossToDie());
        }

    }

    Vector2 GetRandomSpawnPos() {
        int infiniteLoopSafety = 0;
        while(infiniteLoopSafety < 100) {
            // Choose a random edge (top, bottom, left, right)
            int edge = Random.Range(0, 4);
            
            Vector2 pos = Vector2.zero;
            switch(edge) {
                case 0:  // top
                    pos = new Vector2(Random.Range(-extremumX, extremumX), extremumY);
                    break;
                case 1:  // bottom
                    pos = new Vector2(Random.Range(-extremumX, extremumX), -extremumY);
                    break;
                case 2:  // left
                    pos = new Vector2(-extremumX, Random.Range(-extremumY, extremumY));
                    break;
                case 3:  // right
                    pos = new Vector2(extremumX, Random.Range(-extremumY, extremumY));
                    break;
            }
            
            // Check distance from player to avoid spawning too close
            if (player != null && Vector2.Distance(pos, player.transform.position) >= 25) {

                bool tooCloseToEnvironment = Physics2D.OverlapCircle(pos, 2f, LayerMask.GetMask("Destructible_Environment")) != null;
                if (!tooCloseToEnvironment)
                {
                    return pos;
                }
            }

            if(infiniteLoopSafety == 99) {
                Debug.LogWarning("Difficulty finding enemy spawn position. Loop exceeding 99 iterations.");
            }

            infiniteLoopSafety++;
        }

        Debug.LogWarning("Infinite Loop Searching for enemy spawn position");
        return new Vector2(20, 20); 
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
        if(GetRemainingEnemies() < 20) {
            level++;
            StartCoroutine(SpawnEnemy());
        }else{
            StartCoroutine(WaitForPlayerToClearStage());
        }

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

    private IEnumerator WaitForBossToDie() {
        while(waitForBossToDie) {
            if(GetRemainingEnemies() <= 0) {
                TriggerNextWave();
                waitForBossToDie = false;
                break;
            }

            yield return new WaitForSeconds(5);
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

    IEnumerator WaitForPlayerToClearStage() {
        yield return new WaitForSeconds(15);
        TriggerNextWave();
    }
    
}
