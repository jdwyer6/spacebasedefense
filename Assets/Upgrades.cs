using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class Upgrades : MonoBehaviour
{
    private float pickups = 0;
    float pickupsNeededForNextUpgrade = 5;
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
    UpgradeLogicType auto = UpgradeLogicType.auto;
    UpgradeLogicType emp = UpgradeLogicType.emp;
    UpgradeLogicType orbit = UpgradeLogicType.orbit;
    UpgradeLogicType deadlyDash = UpgradeLogicType.deadlyDash;
    UpgradeLogicType healthyHabits = UpgradeLogicType.healthyHabits;
    UpgradeLogicType spread = UpgradeLogicType.spread;

    public GameObject speedBoostParticles;

    private Button[] upgradeButtons;
    bool firstButtonOutlineInitialized = false;
    private EventSystem eventSystem;
    int currentMenuHovered = 0;

    public GameObject orbitalAssassin;
    public GameObject notEnoughXPToolTip;


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

        foreach (var upgrade in upgrades)
        {
            upgrade.acquired = false;
        }

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
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Speed(newUpgrade));
                    break;
                case UpgradeLogicType.health:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Health(newUpgrade));
                    break;
                case UpgradeLogicType.arsen:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Arsen(newUpgrade));
                    break;
                case UpgradeLogicType.auto:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Auto(newUpgrade));
                    break;
                case UpgradeLogicType.emp:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => Emp(newUpgrade));
                    break;
                case UpgradeLogicType.orbit:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Orbit(newUpgrade));
                    break;
                case UpgradeLogicType.deadlyDash:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => DeadlyDash(newUpgrade));
                    break;
                case UpgradeLogicType.healthyHabits:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => HealthyHabits(newUpgrade));
                    break;
                case UpgradeLogicType.spread:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => Spread(newUpgrade));
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
        foreach (var projectile in GameObject.FindGameObjectsWithTag("Projectile_Destructible"))
        {
            projectile.SetActive(false);
        }
        firstButtonOutlineInitialized = false;
    }

    public void hover() {
        am.Play("UI_Hover");
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

    private void HandleUpgradeSelectionUI(GameObject button, Upgrade upgrade) {
        upgrade.acquired = true;
        button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
        button.GetComponent<Button>().interactable = false;
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

    public void Speed(GameObject button){
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == speed).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == speed));
            GetComponent<Player_Movement>().moveSpeed *= 1.5f;
            speedBoostParticles.SetActive(true);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void Health(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == health).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == health));
            GetComponent<Player_Health>().Heal(100);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void Arsen(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == arsen).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == arsen));
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void Auto(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == auto).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == auto));
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void Emp(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == emp).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == emp));
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void Orbit(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == orbit).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == orbit));
            orbitalAssassin.SetActive(true);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void DeadlyDash(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == deadlyDash).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == deadlyDash));
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void HealthyHabits(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == healthyHabits).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == healthyHabits));
            GetComponent<Player_Health>().maxHealth *= 2;
            GetComponent<Player_Health>().currentHealth = GetComponent<Player_Health>().maxHealth;
            GetComponent<Player_Health>().healthBar.value = GetComponent<Player_Health>().maxHealth;
            GetComponent<Player_Health>().targetHealthValue = GetComponent<Player_Health>().currentHealth;
            RectTransform healthBarRect = GetComponent<Player_Health>().healthBar.GetComponent<RectTransform>();
            healthBarRect.sizeDelta = new Vector2(healthBarRect.sizeDelta.x * 2, healthBarRect.sizeDelta.y);

            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void Spread(GameObject button) {
        if(!Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == spread).acquired) {
            HandleUpgradeSelectionUI(button, Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == spread));
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

}
