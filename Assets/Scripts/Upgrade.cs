using UnityEngine;
using System.Reflection;

public enum UpgradeLogicType
{
    speed,
    health,
    arsen,
    rateOfFire,
    shield,
    emp,
    deadlyDash,
    healthyHabits,
    spread,
    lightning,
    omnishot,
    radialRay,
    landmine,
    magnetism,
    drone
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrade/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;          
    public float value; 
    public string title;         
    public string description;
    public int price;
    public UpgradeLogicType upgradeLogic;
    public string methodName;
    public bool acquired;
    public bool purchased;
}
