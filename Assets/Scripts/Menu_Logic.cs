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
    private bool amSet;
    private int currentlyActiveButton;

    private bool pauseMenuOpen;
    public bool menuOpen = false;
    private GameObject currentMenuOpen;

    public Button[] buttons;
    public Character[] characters;
    public bool animate;

    [Header("Menus")]
    public GameObject[] menus;

    public GameObject firstSelected_PauseMenu;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
        currentlyActiveButton = 0;
        if(buttons != null && menuOpen) {
            SetButtonActive();
        }
    }

    private void Update() {
        if(am == null) {
            amSet = false;
        }

        if(amSet == false) {
            amSet = true;
            am = FindObjectOfType<AudioManager>();
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) {
            if(pauseMenuOpen) {
                CloseMenu(menus[0]);
                pauseMenuOpen = false;
                ResumeGame();
                
            }else{
                OpenMenu(menus[0]);
                SetFirstSelected(firstSelected_PauseMenu);
                pauseMenuOpen = true;
                Pause();

            }
        }
    }

    public void MouseEnter() {
        am.Play("UI_Hover");
    }

    public void MouseExit() {
        
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
        GetCurrentMenu();
        if(GameGlobals.Instance != null){
            GameGlobals.Instance.globalMenuOpen = true;
        }
        StartCoroutine(AnimateMenuChange(.5f, menu, currentMenuOpen));
    }

    public void SetFirstSelected(GameObject firstSelected) {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void CloseMenu(GameObject menu) {
        if(GameGlobals.Instance != null){
            GameGlobals.Instance.globalMenuOpen = true;
        }
        menu.SetActive(false);
    }

    IEnumerator AnimateMenuChange(float animationDuration, GameObject menu, GameObject currentMenu) {
        if(currentMenu != null && animate) {
            currentMenu.transform.LeanMoveLocal(new Vector2(-1000, 0), animationDuration).setEaseOutQuart();
        }
        am.Play("Swoosh");
        menu.SetActive(true);
        if(animate) {
            menu.transform.position = new Vector2(1000, 0);
            menu.transform.LeanMoveLocal(new Vector2(0, 0), animationDuration).setEaseOutQuart();
        }

        yield return new WaitForSeconds(animationDuration);
        if(currentMenuOpen != null){
            currentMenuOpen.SetActive(false);
        }
        
        currentMenuOpen = menu;
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


    public void Pause() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        GameGlobals.Instance.globalMenuOpen = false;
    }

    public void setCurrentMenuOpen(GameObject menu) {
        currentMenuOpen = menu;
    }

    public void MainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void LoadScene(int sceneNumber) {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNumber);
    }

    public void SelectCharacter() {
        Character_Button characterButton = EventSystem.current.currentSelectedGameObject.GetComponent<Character_Button>();
        Run_Data runData = GameObject.FindGameObjectWithTag("RunData").GetComponent<Run_Data>();
        am.Play("UI_Select");
        if(characterButton != null) {
            runData.selectedCharacter = characterButton.character;
            foreach (var character in characters)
            {
                character.selected = false;
            }
            characterButton.character.selected = true;
        } else {
            Debug.Log("No Character_Button component found on the selected object.");
        }
    }

    private void GetCurrentMenu() {
        foreach (var menu in menus)
        {
            if(menu.activeInHierarchy) {
                currentMenuOpen = menu;
            }
        }
    }

    public void ToTutorialOrStartGame() {
        int hasSeenTutorial = PlayerPrefs.GetInt("hasSeenTutorial");
        if(hasSeenTutorial == 1) {
            SceneManager.LoadScene(3); 
        }else{
            SceneManager.LoadScene(1);
        }
    }
}
