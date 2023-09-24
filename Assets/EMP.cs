using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

public class EMP : MonoBehaviour
{
    public GameObject empParticles;
    bool canUseEMP;
    bool runTimer;
    public float timer;
    public float empStartTime = 10;
    public Slider empTimerSlider;
    public GameObject empSlider;
    private AudioManager am;
    UpgradeLogicType emp = UpgradeLogicType.emp;

    // Start is called before the first frame update
    void Start()
    {
        timer = empStartTime;
        empTimerSlider.minValue = 0;
        empTimerSlider.maxValue = empStartTime;
        empTimerSlider.value = timer;
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canUseEMP) {
            if(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == emp).acquired){
                StartCoroutine(LaunchEMP());
            }
        }

        if(runTimer) {
            timer += Time.deltaTime;
        }

        if(timer >= empStartTime){
            canUseEMP = true;
            runTimer = false;
        }else{
            canUseEMP = false;
        }
        empTimerSlider.value = timer;

        if(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == emp).acquired){
            empSlider.SetActive(true);
        }else{
            empSlider.SetActive(false);
        }
    }

    IEnumerator LaunchEMP() {
        Instantiate(empParticles, transform.position, Quaternion.identity);
        am.Play("EMP");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        canUseEMP = false;
        timer = 0;
        runTimer = true;

        CinemachineImpulseSource impulseSource = GetComponent<CinemachineImpulseSource>();
        impulseSource.GenerateImpulse();

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile_Destructible");
        foreach (var projectile in projectiles)
        {
            Destroy(projectile);
        }

        foreach (var enemy in enemies)
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if(rb != null){
                rb.gravityScale = 1;
                enemy.GetComponent<Enemy_Movement>().enabled = false;
            }
        }

        yield return new WaitForSeconds(2);

        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        
    }

}