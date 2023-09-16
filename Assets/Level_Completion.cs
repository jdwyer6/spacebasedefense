using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level_Completion : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Assign this via Inspector
    public TextMeshProUGUI levelCompletionText; // Assign this via Inspector

    private float countdownDuration = 20f * 60f; // 20 minutes in seconds

    private void Start()
    {
        levelCompletionText.gameObject.SetActive(false);
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        float remainingTime = countdownDuration;

        while (remainingTime > 0)
        {
            // Calculate minutes and seconds from the remaining time
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Display the time
            countdownText.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // Once the countdown is done
        countdownText.text = "00:00";
        levelCompletionText.gameObject.SetActive(true);
    }
}
