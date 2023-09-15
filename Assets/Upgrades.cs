using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class Upgrades : MonoBehaviour
{
    private float pickups = 0;
    float pickupsNeededForNextUpgrade = 7;
    float pickupLevelMultiplier = 1.2f;
    AudioManager am;
    private GameObject gm;
    public GameObject pickupParticles;
    public Slider pickupSlider; 

    private float targetSliderValue;
    public float sliderSpeed = 5f;

    public GameObject upgradeMenu;
    public bool menuOpen = false;

    //Upgrade Options
    public GameObject upgradeButton;
    Upgrade[] upgrades; 
    public GameObject upgradeGroup;

    [Header("Upgrade Booleans")]
    public bool speedAcquired;
    public bool healthAcquired;
    public bool arsenAcquired;
    public bool autoAcquired;

    public GameObject speedBoostParticles;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        upgrades = gm.GetComponent<Data>().upgrades;

        speedAcquired = false;
        healthAcquired = false;
        arsenAcquired = false;
        autoAcquired = false;
        
        if (pickupSlider)
        {
            pickupSlider.minValue = 0;
            pickupSlider.maxValue = pickupsNeededForNextUpgrade;
            pickupSlider.value = 0;
            targetSliderValue = pickups;
        }
        SetUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        if(pickups >= pickupsNeededForNextUpgrade && !menuOpen) {
            menuOpen = true;
            OpenUpgradeMenu();
        }

        pickupSlider.value = Mathf.Lerp(pickupSlider.value, targetSliderValue, Time.deltaTime * sliderSpeed);

        // if (Input.GetKeyDown(KeyCode.Tab)){
        //     if(menuOpen){
        //         CloseUpgradesMenu();
        //     }else{
        //         OpenUpgradeMenu();
        //     }
        // }

        // if(upgradeMenu.activeInHierarchy){
        //     menuOpen = true;
        // }else{
        //     menuOpen = false;
        // }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Pickup") {
            pickups++;
            am.Play("Pickup");
            Instantiate(pickupParticles, other.transform.position, Quaternion.identity);
            targetSliderValue = pickups;
            Destroy(other.gameObject);
        
        }
    }

    void SetUpgrades() {
        foreach (var upgrade in upgrades)
        {
            GameObject newUpgrade = Instantiate(upgradeButton, upgradeGroup.transform);
            newUpgrade.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = upgrade.title;
            if(upgrade.description != null) newUpgrade.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = upgrade.description;
            switch(upgrade.upgradeLogic)
            {
                case UpgradeLogicType.speed:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => speed(newUpgrade));
                    break;
                case UpgradeLogicType.health:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => health(newUpgrade));
                    break;
                case UpgradeLogicType.arsen:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => arsen(newUpgrade));
                    break;
                case UpgradeLogicType.auto:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => auto(newUpgrade));
                    break;
            }
        }
    }

    public void speed(GameObject button){
        if(!speedAcquired) {
            speedAcquired = true;
            GetComponent<Player_Movement>().moveSpeed *= 1.5f;
            button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
            button.GetComponent<Button>().interactable = false;
            am.Play("UI_Select");
            am.Play("Upgrade_UI");
            speedBoostParticles.SetActive(true);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void health(GameObject button) {
        if(!healthAcquired) {
            healthAcquired = true;
            button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
            button.GetComponent<Button>().interactable = false;
            am.Play("UI_Select");
            am.Play("Upgrade_UI");
            GetComponent<Player_Health>().Heal(100);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void arsen(GameObject button) {
        if(!arsenAcquired) {
            arsenAcquired = true;
            button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
            button.GetComponent<Button>().interactable = false;
            am.Play("UI_Select");
            am.Play("Upgrade_UI");
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void auto(GameObject button) {
        if(!autoAcquired) {
            autoAcquired = true;
            button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
            button.GetComponent<Button>().interactable = false;
            am.Play("UI_Select");
            am.Play("Upgrade_UI");
            //TODO Add new gun?
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    void OpenUpgradeMenu() {
        upgradeMenu.SetActive(true);
        menuOpen = true;
        am.Play("Upgrade_UI");
        Time.timeScale = 0;
        pickupSlider.maxValue = pickupsNeededForNextUpgrade;
        pickupSlider.value = pickups;  
    }

    void CloseUpgradesMenu() {
        
         Time.timeScale = 1;
        upgradeMenu.SetActive(false);
        Debug.Log("time is called!");
        menuOpen = false;
        foreach (var projectile in GameObject.FindGameObjectsWithTag("Projectile_Destructible"))
        {
            projectile.SetActive(false);
        }
    }

    public void hover() {
        am.Play("UI_Hover");
    }

    
    void ResetAndUpdatePickups(){
        pickupsNeededForNextUpgrade *= pickupLevelMultiplier;
        pickups = 0;
    }
}
