using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int numberOfKills = 0;
    public TextMeshProUGUI numberOfKillsText;

    // Start is called before the first frame update
    void Start()
    {
        numberOfKills = 0;

        if(numberOfKillsText != null) {
            numberOfKillsText.text = PlayerPrefs.GetInt("HighestNumberOfKills").ToString();
        }
    }

    public void SetNumberOfKills() {
        numberOfKills++;
        UpdateHighScore();
    }

    public void UpdateHighScore() {
        if(numberOfKills > PlayerPrefs.GetInt("HighestNumberOfKills")) {
            PlayerPrefs.SetInt("HighestNumberOfKills", numberOfKills);
        }
    } 
}
