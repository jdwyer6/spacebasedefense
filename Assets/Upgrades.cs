using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


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

    [Header("Upgrade Booleans")]
    public Dictionary<string, bool> upgradeAcquired = new Dictionary<string, bool>{
        {"speedAcquired", false},
        {"healthAcquired", false},
        {"arsenAcquired", false},
        {"autoAcquired", false},
        {"empAcquired", false},
        {"orbitalAcquired", false},
        {"dashAcquired", false}
    };

    public GameObject speedBoostParticles;

    private Button[] upgradeButtons;
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
        SetUpgrades();
        upgradeButtons = upgradeGroup.GetComponentsInChildren<Button>();
        eventSystem = EventSystem.current;

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

    void SetUpgrades() {
        foreach (var upgrade in upgrades)
        {
            GameObject newUpgrade = Instantiate(upgradeButton, upgradeGroup.transform);
            newUpgrade.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = upgrade.title;
            newUpgrade.transform.Find("Icon").GetComponent<Image>().sprite = upgrade.icon;
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
                case UpgradeLogicType.emp:
                    newUpgrade.GetComponent<Button>().onClick.AddListener(() => emp(newUpgrade));
                    break;
                case UpgradeLogicType.orbit:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => orbit(newUpgrade));
                    break;
                case UpgradeLogicType.dash:
                newUpgrade.GetComponent<Button>().onClick.AddListener(() => dash(newUpgrade));
                    break;
            }
        }
    }

    void OpenUpgradeMenu() {
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

    private void HandleUpgradeSelectionUI(GameObject button, string acquiredValueToChange) {
        upgradeAcquired[acquiredValueToChange] = true;
        button.GetComponent<Image>().color = new Color(1.0f, 0.8627f, 0.3216f);
        button.GetComponent<Button>().interactable = false;
        am.Play("UI_Select");
        am.Play("Upgrade_UI");
    }

    public bool AllUpgradesHaveBeenAcquired() {
        foreach (var hasUpgrade in upgradeAcquired)
        {
            if(!hasUpgrade.Value) {
                return false;
            }
            
        }
        pickupsParentUI.SetActive(false);
        return true;
    }

    public void speed(GameObject button){
        if(!upgradeAcquired["speedAcquired"]) {
            HandleUpgradeSelectionUI(button, "speedAcquired");
            GetComponent<Player_Movement>().moveSpeed *= 1.5f;
            speedBoostParticles.SetActive(true);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void health(GameObject button) {
        if(!upgradeAcquired["healthAcquired"]) {
            HandleUpgradeSelectionUI(button, "healthAcquired");
            GetComponent<Player_Health>().Heal(100);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void arsen(GameObject button) {
        if(!upgradeAcquired["arsenAcquired"]) {
            HandleUpgradeSelectionUI(button, "arsenAcquired");
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void auto(GameObject button) {
        if(!upgradeAcquired["autoAcquired"]) {
            HandleUpgradeSelectionUI(button, "autoAcquired");
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void emp(GameObject button) {
        if(!upgradeAcquired["empAcquired"]) {
            HandleUpgradeSelectionUI(button, "empAcquired");
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void orbit(GameObject button) {
        if(!upgradeAcquired["orbitalAcquired"]) {
            HandleUpgradeSelectionUI(button, "orbitalAcquired");
            orbitalAssassin.SetActive(true);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

    public void dash(GameObject button) {
        if(!upgradeAcquired["dashAcquired"]) {
            HandleUpgradeSelectionUI(button, "dashAcquired");
            dashToolTip.SetActive(true);
            ResetAndUpdatePickups();
            CloseUpgradesMenu();
        }else{
            am.Play("UI_Disabled");
        }
    }

}
