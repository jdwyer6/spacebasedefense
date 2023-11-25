using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalHits : MonoBehaviour
{
    public int originalCriticalHitPercent = 2;
    public int criticalHitPercent;
    public float criticalHitDamageAmount = 100;
    public GameObject criticalHitParticles;

    // Start is called before the first frame update
    void Start()
    {
        criticalHitPercent = originalCriticalHitPercent;
    }

    public bool isCriticalHit() {
        int randomNum = UnityEngine.Random.Range(0, 100);
        if (randomNum <= criticalHitPercent) {
            return true;
        } else {
            return false;
        }
    }

    public void UpdateCriticalHitAmount(int amountToAdjust) {
        criticalHitPercent += amountToAdjust;
    }
}
