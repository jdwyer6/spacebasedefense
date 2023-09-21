using UnityEngine;

[CreateAssetMenu(fileName = "NewVarietyLevel", menuName = "VarietyLevel/VarietyLevel")]
public class VarietyLevel : ScriptableObject
{
    public string levelTitle;
    public int amountOfEnemiesToSpawnPerCycle;
    public GameObject[] enemies;
}
