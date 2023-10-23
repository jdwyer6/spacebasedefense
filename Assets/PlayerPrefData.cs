using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefs", menuName = "PlayerPrefs")]
public class Player : ScriptableObject
{
    public Sprite selectedCharacter;
    public int cash;
}
