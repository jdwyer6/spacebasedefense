using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Reflection;


public class Upgrades : MonoBehaviour
{
    private float pickups = 0;
    float pickupsNeededForNextUpgrade = 10;
    float pickupLevelMultiplier = 1.2f;
    AudioManager am;
    private GameObject gm;
    public GameObject pickupParticles;
    public Slider pickupSlider; 
    public GameObject pickupsParentUI;

    private float targetSliderValue;
    public float sliderSpeed = 5f;

    public GameObject upgradeMenu;
    public bool menuOpen = false;

    public GameObject dashToolTip;

    //Upgrade Options
    public GameObject upgradeButton;
    Upgrade[] upgrades; 
    public GameObject upgradeGroup;

    UpgradeLogicType speed = UpgradeLogicType.speed;
    UpgradeLogicType health = UpgradeLogicType.health;
    UpgradeLogicType arsen = UpgradeLogicType.arsen;
    UpgradeLogicType rateOfFire = UpgradeLogicType.rateOfFire;
    UpgradeLogicType emp = UpgradeLogicType.emp;
    UpgradeLogicType shield = UpgradeLogicType.shield;
    UpgradeLogicType deadlyDash = UpgradeLogicType.deadlyDash;
    UpgradeLogicType healthyHabits = UpgradeLogicType.healthyHabits;
    UpgradeLogicType spread = UpgradeLogicType.spread;
    UpgradeLogicType lightning = UpgradeLogicType.lightning;
    UpgradeLogicType omnishot = UpgradeLogicType.omnishot;
    UpgradeLogicType radialRay = UpgradeLogicType.radialRay;
    UpgradeLogicType landmine = UpgradeLogicType.landmine;

    public GameObject speedBoostParticles;

    private Button[] upgradeButtons;
    bool firstButtonOutlineInitialized = false;
    private EventSystem eventSystem;
    int currentMenuHovered = 0;

    public GameObject shieldPrefab;
    public GameObject notEnoughXPToolTip;

    public GameObject lightningUpgrade;
    public GameObject omnishotPrefab;
    public GameObject radialRayPrefab;


    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        upgrades = gm.GetComponent<Data>().upgrades;
        if (pickupSlider)
        {
            pickupSlider.minValue = 0;
            pickupSlider.maxValue = pickupsNeededForNextUpgrade;
            pickupSlider.value = 0;
            targetSliderValue = pickups;
        }
        eventSystem = EventSystem.current;

        SetAcquiredUpgrades();

    }

    // Update is called once per frame
    void Update()
    {
        if(pickups >= pickupsNeededForNextUpgrade && !menuOpen && !AllUpgradesHaveBeenAcquired()) {
            menuOpen = true;
            OpenUpgradeMenu();
        }

        // if(Mathf.Abs(pickupSlider.value - targetSliderValue) < 0.01f) {
        //     pickupSlider.value = targetSliderValue;
        // } else {
            pickupSlider.value = Mathf.Lerp(pickupSlider.value, targetSliderValue, Time.deltaTime * sliderSpeed);
        // }

        if (menuOpen)
        {
             upgradeButtons = upgradeGroup.GetComponentsInChildren<Button>();
             if(!firstButtonOutlineInitialized) {
                upgradeButtons[0].GetComponent<Outline>().enabled = true; 
                firstButtonOutlineInitialized = true;
             }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Reset color of previously hovered button
                if (currentMenuHovered >= 0)
                    upgradeButtons[currentMenuHovered].GetComponent<Outline>().enabled = false;

                currentMenuHovered--;
                if (currentMenuHovered < 0) currentMenuHovered = upgradeButtons.Length - 1; 

                // Set darker color for currently hovered button
                upgradeButtons[currentMenuHovered].GetComponent<Outline>().enabled = true; 

                eventSystem.SetSelectedGameObject(upgradeButtons[currentMenuHovered].gameObject);
                am.Play("UI_Hover");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Reset color of previously hovered button
                if (currentMenuHovered >= 0)
                    upgradeButtons[currentMenuHovered].GetComponent<Outline>().enabled = false;

                currentMenuHovered++;
                if (currentMenuHovered >= upgradeButtons.Length) currentMenuHovered = 0;

                // Set darker color for currently hovered button
                upgradeButtons[currentMenuHovered].GetComponent<Outline>().enabled = true;

                eventSystem.SetSelectedGameObject(upgradeButtons[currentMenuHovered].gameObject);
                am.Play("UI_Hover");
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                upgradeButtons[currentMenuHovered].onClick.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                SoftMenuClose();
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Pickup") {
            targetSliderValue = pickups;
            pickups++;
            if(pickups == 1){
                targetSliderValue = pickups;
            }
            am.Play("Pickup");
            Instantiate(pickupParticles, other.transform.position, Quaternion.identity);
            
            Destroy(other.gameObject);
        }
    }

    private List<Upgrade> GetNonAcquiredUpgrades() {
        List<Upgrade> tempList = new List<Upgrade>();
        foreach (var upgrade in upgrades)
        {
            if(!upgrade.acquired) {
                tempList.Add(upgrade);
            }
        }
        return tempList;
    }

    private List<Upgrade> SelectRandomUpgrades(int numberToSelect) {
        List<Upgrade> nonAcquiredUpgrades = GetNonAcquiredUpgrades();
        List<Upgrade> tempList = new List<Upgrade>();
        
        for(int i = 0; i < numberToSelect; i++) {
            Upgrade potentialUpgrade = null;
            
            // Ensure we aren't stuck in an infinite loop if there are fewer non-acquired upgrades than numberToSelect
            int safetyCounter = 0;

            do {
                potentialUpgrade = nonAcquiredUpgrades[UnityEngine.Random.Range(0, nonAcquiredUpgrades.Count)];
                safetyCounter++;
            } while(tempList.Contains(potentialUpgrade) && safetyCounter < 1000);
            
            if(!tempList.Contains(potentialUpgrade)) {
                tempList.Add(potentialUpgrade);
            }
        }
        return tempList;
    }

    void ClearUpgradeGroupChildren() {
        foreach(Transform child in upgradeGroup.transform) {
            Destroy(child.gameObject);
        }
    }


    void SetUpgrades() {
        ClearUpgradeGroupChildren();
        foreach (var upgrade in SelectRandomUpgrades(3))
        {
            GameObject newUpgrade = Instantiate(upgradeButton, upgradeGroup.transform);
            newUpgrade.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = upgrade.title;
            newUpgrade.transform.Find("Icon").GetComponent<Image>().sprite = upgrade.icon;
            if(upgrade.description != null) newUpgrade.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = upgrade.description;
            switch(upgrade.upgradeLogic)
            {
                case UpgradeLogicType.speed:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Speed());
                    break;
                case UpgradeLogicType.health:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Health());
                    break;
                case UpgradeLogicType.arsen:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Arsen());
                    break;
                case UpgradeLogicType.rateOfFire:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => RateOfFire());
                    break;
                case UpgradeLogicType.emp:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Emp());
                    break;
                case UpgradeLogicType.shield:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Shield());
                    break;
                case UpgradeLogicType.deadlyDash:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => DeadlyDash());
                    break;
                case UpgradeLogicType.healthyHabits:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => HealthyHabits());
                    break;
                case UpgradeLogicType.spread:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Spread());
                    break;
                case UpgradeLogicType.lightning:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Lightning());
                    break;
                case UpgradeLogicType.omnishot:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Omnishot());
                    break;
                case UpgradeLogicType.radialRay:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => RadialRay());
                    break;
                case UpgradeLogicType.landmine:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Landmine());
                    break;
            }
        }
    }

    void OpenUpgradeMenu() {
        SetUpgrades();
        upgradeMenu.SetActive(true);
        menuOpen = true;
        GameGlobals.Instance.globalMenuOpen = true;
        am.Play("Upgrade_UI");
        Time.timeScale = 0;
        pickupSlider.maxValue = pickupsNeededForNextUpgrade;
        pickupSlider.value = pickups;  
    }

    void CloseUpgradesMenu() {
        
        Time.timeScale = 1;
        upgradeMenu.SetActive(false);
        menuOpen = false;
        GameGlobals.Instance.globalMenuOpen = false;
        foreach (var projectile in GameObject.FindGameObjectsWithTag("Enemy_Projectile"))
        {
            projectile.SetActive(false);
        }
        firstButtonOutlineInitialized = false;
    }

    public void hover() {
        am.Play("UI_Hover");
    }

    private void SetAcquiredUpgrades() {
        foreach (var upgrade in upgrades)
        {
            if(upgrade.purchased) {
                upgrade.acquired = true;
                MethodInfo method = this.GetType().GetMethod(upgrade.methodName);
                if (method != null)
                {
                    method.Invoke(this, null);
                }
            }else{
                upgrade.acquired = false;
            }
            
        }
    }

    void ResetAndUpdatePickups(){
        pickupsNeededForNextUpgrade = Mathf.Floor(pickupsNeededForNextUpgrade * pickupLevelMultiplier);
        pickups = 0;
        pickupSlider.value = 0;
        targetSliderValue = pickups;
    }

    public void SoftMenuClose(){
        pickups *= .75f;
        Time.timeScale = 1;
        upgradeMenu.SetActive(false);
        menuOpen = false;
        GameGlobals.Instance.globalMenuOpen = false;
    }

    IEnumerator NotEnoughXP() {
        notEnoughXPToolTip.SetActive(true);
        am.Play("UI_Disabled");
        yield return new WaitForSeconds(2);
        notEnoughXPToolTip.SetActive(false);
    }

    private void HandleUpgradeSelectionUI(Upgrade upgrade) {
        upgrade.acquired = true;
        // button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
        // button.GetComponent<Button>().interactable = false;
        am.Play("UI_Select");
        am.Play("Upgrade_UI");
    }

    public bool AllUpgradesHaveBeenAcquired() {
        foreach (var upgrade in upgrades)
        {
            if(!upgrade.acquired) {
                return false;
            }
            
        }
        pickupsParentUI.SetActive(false);
        return true;
    }

    public void Speed(){
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == speed));
        GetComponent<Player_Movement>().moveSpeed *= 1.5f;
        speedBoostParticles.SetActive(true);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Health() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == health));
        GetComponent<Player_Health>().Heal(100);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Arsen() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == arsen));
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void RateOfFire() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == rateOfFire));
        GetComponent<Player_Shooting>().autoShootingInterval *= .7f;
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Emp() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == emp));
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Shield() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == shield));
        shieldPrefab.SetActive(true);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void DeadlyDash() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == deadlyDash));
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void HealthyHabits() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == healthyHabits));
        GetComponent<Player_Health>().maxHealth *= 1.2f;
        GetComponent<Player_Health>().currentHealth = GetComponent<Player_Health>().maxHealth;
        GetComponent<Player_Health>().healthBar.value = GetComponent<Player_Health>().maxHealth;
        GetComponent<Player_Health>().targetHealthValue = GetComponent<Player_Health>().currentHealth;
        RectTransform healthBarRect = GetComponent<Player_Health>().healthBar.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2(healthBarRect.sizeDelta.x * 1.2f, healthBarRect.sizeDelta.y);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Spread() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == spread));
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Lightning() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == lightning));
        lightningUpgrade.SetActive(true);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Omnishot() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == omnishot));
        omnishotPrefab.SetActive(true);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void RadialRay() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == radialRay));
        radialRayPrefab.SetActive(true);
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }

    public void Landmine() {
        HandleUpgradeSelectionUI(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == landmine));
        GetComponent<Landmines>().InitiateLandmines();
        ResetAndUpdatePickups();
        CloseUpgradesMenu();
    }
}
