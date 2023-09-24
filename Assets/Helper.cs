using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static Upgrade[] GetUpgrades() 
    {
        return GameObject.FindGameObjectWithTag("GM").GetComponent<Data>().upgrades;
    }
}

