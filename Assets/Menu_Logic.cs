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
    public bool isCyclingHorizontalOptions;
    private int currentlyActiveButton;
    public bool isHorizontalMenu;
    public Button[] horizontalOptions;
    public int currentlyActiveHorizontalButton = 0;
    public GameObject horizontalRow;

    private bool pauseMenuOpen;
    public bool menuOpen = false;
    public bool isMainMenu;
    private GameObject currentMenuOpen;

    public Button[] buttons;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject characterSelectMenu;
    public GameObject settingsMenu;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
        currentlyActiveButton = 0;
        if(buttons != null && menuOpen) {
            SetButtonActive();
        }
        if(isMainMenu) {
            setCurrentMenuOpen(mainMenu);
        }

    }

    private void Update() {

        if(buttons != null && menuOpen) {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeActiveButton(1);  // Increment
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeActiveButton(-1);  // Decrement
            }

            if(isCyclingHorizontalOptions) {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    ChangeActiveHorizontalButton(1);  // Increment
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ChangeActiveHorizontalButton(-1);  // Decrement
                }
            }

            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                buttons[currentlyActiveButton].onClick.Invoke(); 
            }

            if(horizontalRow) {
                if(isHorizontalMenu) {
                    isCyclingHorizontalOptions = true;
                    horizontalRow.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    SetHorizontalButtonActive();
                }else{
                    isCyclingHorizontalOptions = false;
                    horizontalRow.GetComponent<RectTransform>().localScale = new Vector3(.5f, .5f, 1);
                }
            }
        }



        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(pauseMenuOpen) {
    
                CloseMenu(pauseMenu);
                pauseMenuOpen = false;
                ResumeGame();
                
            }else{
                OpenMenu(pauseMenu);
                pauseMenuOpen = true;
                Pause();

            }
        }
    }

    public void HoverSound() {
        am.Play("UI_Hover");
    }

    public void ClickSound() {
        am.Play("UI_Select");
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
        GameGlobals.Instance.globalMenuOpen = true;
        menu.SetActive(true);
        currentMenuOpen.SetActive(false);
        currentMenuOpen = menu;
    }

    public void CloseMenu(GameObject menu) {
        GameGlobals.Instance.globalMenuOpen = false;
        menu.SetActive(false);
    }

    public void EngageCycle() {
        isCyclingHorizontalOptions = true;
    }

    public void DisengageCycle() {
        isCyclingHorizontalOptions = false;
    }

    public void CycleRight() {

    }

    public void CycleLeft() {

    }

    public void Select() {
        am.Play("UI_Select");
        foreach (var button in buttons)
        {
            if(EventSystem.current.currentSelectedGameObject == button.gameObject)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(255/255f, 200/255f, 62/255f, 1f);
            }
        }
    }

    public void SetButtonActive() {
        buttons[currentlyActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = new Color(255/255f, 220/255f, 82/255f, 1f);  
    }

    public void SetHorizontalButtonActive() {
        GameObject activeImage = horizontalOptions[currentlyActiveHorizontalButton].transform.Find("Outline").gameObject;
        if(activeImage != null) {
            activeImage.SetActive(true);
        }
    }

    private void ChangeActiveButton(int change) {
        // Reset the color of the previously active button to default
        buttons[currentlyActiveButton].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;  
        am.Play("UI_Hover");

        currentlyActiveButton += change;

        // Wrap-around the value if necessary
        if (currentlyActiveButton >= horizontalOptions.Length)
        {
            currentlyActiveButton = 0;
        }
        else if (currentlyActiveButton < 0)
        {
            currentlyActiveButton = horizontalOptions.Length - 1;
        }

        SetButtonActive();
    }

    private void ChangeActiveHorizontalButton(int change) {
        foreach (var option in horizontalOptions)
        {
            option.transform.Find("Outline").gameObject.SetActive(false);
        }

        am.Play("UI_Hover");

        currentlyActiveHorizontalButton += change;

        // Wrap-around the value if necessary
        if (currentlyActiveHorizontalButton >= horizontalOptions.Length)
        {
            currentlyActiveHorizontalButton = 0;
        }
        else if (currentlyActiveHorizontalButton < 0)
        {
            currentlyActiveHorizontalButton = buttons.Length - 1;
        }

        SetHorizontalButtonActive();
    }

    public void Pause() {
        // Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }

    public void setCurrentMenuOpen(GameObject menu) {
        currentMenuOpen = menu;
    }
}
