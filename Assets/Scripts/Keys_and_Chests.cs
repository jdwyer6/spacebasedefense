using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys_and_Chests : MonoBehaviour
{
    public int keys;
    private AudioManager am;
    private GameObject gm;
    private ToolTipManager toolTipManager;
    bool insideChestRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        toolTipManager = GameObject.FindGameObjectWithTag("ToolTipManager").GetComponent<ToolTipManager>();
        keys = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Locked_Door") {
            if(keys > 0) {
                am.Play("Door_Open");
                keys--;
                Destroy(other.gameObject);
            }else{
                toolTipManager.ShowToolTip("Missing Key", "You need a key to enter this area");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Chest_Room") {
            insideChestRoom = true;
            gm.GetComponent<Level_Completion>().timerPaused = true;
            StartCoroutine(PeriodicallyCheckForActiveEnemies());
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Chest_Room") {
            insideChestRoom = false;
            gm.GetComponent<Level_Completion>().timerPaused = false;
            HandleEnemyMovement(true);
        }
    }

    private void HandleEnemyMovement(bool activeState) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Enemy_Movement movement = enemy.GetComponent<Enemy_Movement>();
            Enemy_Shooting shooting = enemy.GetComponent<Enemy_Shooting>();
            Orbit_Enemy orbit = enemy.GetComponent<Orbit_Enemy>();
            Enemy_Random_Quick_AddOn addOn = enemy.GetComponent<Enemy_Random_Quick_AddOn>();
            UbhShotCtrl shotController = enemy.GetComponent<UbhShotCtrl>();
            Enemy_Laser laser = enemy.GetComponent<Enemy_Laser>();

            if(movement != null) movement.enabled = activeState;
            if(shooting != null) shooting.enabled = activeState;
            if(orbit != null) orbit.enabled = activeState;
            if(addOn != null) addOn.enabled = activeState;
            if(shotController != null) shotController.enabled = activeState;
            if(laser != null) laser.enabled = activeState;

        }
    }

    private IEnumerator PeriodicallyCheckForActiveEnemies(){
        while(insideChestRoom) {
            HandleEnemyMovement(false);
            yield return new WaitForSeconds(1);
        }
    }
}
