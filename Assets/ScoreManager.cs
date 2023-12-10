using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int numberOfKills = 0;

    // Start is called before the first frame update
    void Start()
    {
        numberOfKills = 0;
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
