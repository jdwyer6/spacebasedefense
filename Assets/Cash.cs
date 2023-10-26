using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Cash : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private GameObject gm;
   

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");
        coinText = GameObject.FindGameObjectWithTag("CashText").GetComponent<TextMeshProUGUI>();
        coinText.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }

}
