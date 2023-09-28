using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Menu_Logic : MonoBehaviour
{
    private AudioManager am;
    public bool isCyclingOptions;

    public Button[] buttons;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }

    public void HoverSound() {
        // am.Play();
    }

    public void ClickSound() {

    }

    public void StartGame() {
        Scene currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("You are on the last scene.");
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void OpenMenu(GameObject menu) {

    }

    public void CloseMenu(GameObject menu) {

    }

    public void EngageCycle() {
        isCyclingOptions = true;
    }

    public void DisengageCycle() {
        isCyclingOptions = false;
    }

    public void CycleRight() {

    }

    public void CycleLeft() {

    }

    public void Select() {
        foreach (var button in buttons)
        {
            if(EventSystem.current.currentSelectedGameObject == button.gameObject)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
        }
    }
}
