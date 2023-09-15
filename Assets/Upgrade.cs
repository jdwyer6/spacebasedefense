using UnityEngine;

public enum UpgradeLogicType
{
    speed,
    health,
    arsen,
    auto,
    orbit,
    emp
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrade/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;          
    public float value; 
    public string title;         
    public string description;
    public UpgradeLogicType upgradeLogic;
}
