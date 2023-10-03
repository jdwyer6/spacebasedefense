using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss_Drop : MonoBehaviour
{
    public bool isSpeedBoost;
    UpgradeLogicType auto = UpgradeLogicType.auto;
    private AudioManager am;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            if (isSpeedBoost) {
                Upgrade targetUpgrade = Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == auto);
                if (targetUpgrade != null) {
                    targetUpgrade.acquired = true;
                }

                other.gameObject.GetComponent<Player_Shooting>().autoShootingInterval = .08f;
            }
        }

        am.Play("UI_Select");
        Destroy(gameObject);

    }

}
