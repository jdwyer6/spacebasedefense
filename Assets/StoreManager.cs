using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public TextMeshProUGUI currentCashText;

    // Start is called before the first frame update
    void Start()
    {
        int coins = PlayerPrefs.GetInt("coins");
        currentCashText.text = coins.ToString();
        // updateCoinText(); TODO

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectHealth(int cost) {
        if(!CheckHasEnoughCoins(cost)) return;
        int coins = PlayerPrefs.GetInt("coins");
        Debug.Log("SelectHealth()");
        float initialHealthMultiplier = PlayerPrefs.GetFloat("InitialHealthMultiplier");
        PlayerPrefs.SetFloat("InitialHealthMultiplier", initialHealthMultiplier += .2f);
        coins -= cost;
        PlayerPrefs.SetFloat("coins", coins);
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
        int coins = PlayerPrefs.GetInt("coins");
        currentCashText.text = coins.ToString();
    }
}
