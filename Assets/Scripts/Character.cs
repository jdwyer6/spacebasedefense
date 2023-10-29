using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite sprite;               
    public Sprite gameSpriteBase;
    public Sprite gameSpriteEyes;
    public string unlockableDescription;
    public bool selected;
    public bool unlocked;
}
