using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public TextMeshProUGUI currentCashText;
    public Transform scrollContainer;
    public GameObject btn;
    public Upgrade[] storeUpgrades;
    private AudioManager am;
    public GameObject messageText;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        updateCoinText();
        SetupButtons();

    }

    public void SelectUpgrade(int cost, Upgrade upgrade, Button button) {
        if(am == null) am = FindObjectOfType<AudioManager>();
        if(!CheckHasEnoughCoins(cost)) {
            StartCoroutine(ShowMessage("NOT ENOUGH CREDITS"));
            am.Play("UI_Disabled");
            return;
        } 
        am.Play("Open_Chest");
        button.interactable = false;
        upgrade.purchased = true;
        int coins = PlayerPrefs.GetInt("coins");
        coins -= cost;
        PlayerPrefs.SetInt("coins", coins);
        updateCoinText();
    }

    private bool CheckHasEnoughCoins(int cost) {
        int coins = PlayerPrefs.GetInt("coins");
        if(cost > coins) {
            return false;
        }
        return true;
    }

    private void updateCoinText() {
        Debug.Log("update coin text");
        int coins = PlayerPrefs.GetInt("coins");
        currentCashText.text = coins.ToString();
    }

    private void SetupButtons() {
        foreach (var upgrade in storeUpgrades)
        {
            GameObject button = Instantiate(btn, scrollContainer);
            button.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = upgrade.title;
            button.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = upgrade.description;
            button.transform.Find("Price/PriceText").GetComponent<TextMeshProUGUI>().text = upgrade.price.ToString();
            button.GetComponent<Button>().onClick.AddListener(() => SelectUpgrade(upgrade.price, upgrade, button.GetComponent<Button>()));
            if(upgrade.purchased) {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }

    IEnumerator ShowMessage(string text) {
        messageText.SetActive(true);
        messageText.GetComponent<TextMeshProUGUI>().text = text;
        yield return new WaitForSeconds(4);
        messageText.SetActive(false);
    }
}
