using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Michsky.MUIP;
using TMPro;

public class Boss_Drop : MonoBehaviour
{
    public bool isRateOfFire;
    UpgradeLogicType rateOfFire = UpgradeLogicType.rateOfFire;
    private AudioManager am;
    public GameObject toolTip;
    private Transform canvas;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        Debug.Log(canvas.name);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            if (isRateOfFire) {
                Upgrade targetUpgrade = Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == rateOfFire);
                if (targetUpgrade != null) {
                    targetUpgrade.acquired = true;
                }
                GameObject tooltipInstance = Instantiate(toolTip, canvas);
                toolTip.SetActive(true);
                toolTip.GetComponent<TextMeshProUGUI>().text = "Increased rate of fire.";
                am.Play("MajorUpgrade");
                other.gameObject.GetComponent<Player_Shooting>().autoShootingInterval = .08f;
                
            }

            am.Play("UI_Select");
            Destroy(gameObject);
        }

    }

}
