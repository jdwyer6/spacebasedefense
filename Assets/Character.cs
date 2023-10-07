using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite sprite;               
    public string unlockableDescription;
    public bool unlocked;
}
