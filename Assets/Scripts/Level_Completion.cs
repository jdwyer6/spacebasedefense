using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level_Completion : MonoBehaviour
{
    public TextMeshProUGUI countdownText; 

    private float countdownDuration = 12f * 60f; 
    public float remainingTime;
    public GameObject levelCompleteScreen;
    public bool timerPaused = false;

    private void Start()
    {
        StartCoroutine(Countdown());
    }


    IEnumerator Countdown()
    {
        remainingTime = countdownDuration;

        while (remainingTime > 0)
        {
            if(timerPaused) {
                yield return null;
                continue;
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Display the time
            countdownText.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }


        // Once the countdown is done
        countdownText.text = "00:00";
        EndLevel();
    }

    void EndLevel() {
        Time.timeScale = 0;
        levelCompleteScreen.SetActive(true);
    }
}
