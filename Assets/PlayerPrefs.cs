using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefData", menuName = "PlayerPrefData")]
public class PlayerPrefData : ScriptableObject
{
    public Sprite selectedCharacter;
    public int cash;
}
