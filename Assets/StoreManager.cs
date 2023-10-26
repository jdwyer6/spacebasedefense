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
        currentCashText.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
