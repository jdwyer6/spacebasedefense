using UnityEngine;

public enum Difficulty
{
    Opening,
    Easy,
    Medium,
    Hard,
    VeryHard
}

[CreateAssetMenu(fileName = "NewWave", menuName = "Wave/Wave")]
public class Wave : ScriptableObject
{
    [Header("Wave Settings")]
    public string title;
    
    [Tooltip("List of enemy prefabs to spawn in this wave.")]
    public GameObject[] enemies;

    [Header("Spawn Settings")]
    public int numberOfSpawnCycles;
    public int minimumLevelToSpawn;
    
    [Header("Difficulty")]
    public Difficulty difficulty;

    [Space]

    [Tooltip("Is this wave a boss wave?")]
    public bool isBoss;
}
