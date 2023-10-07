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
    private int currentlyActiveButton;

    private bool pauseMenuOpen;
    public bool menuOpen = false;
    private GameObject currentMenuOpen;

    public Button[] buttons;
    public Character[] characters;

    [Header("Menus")]
    public GameObject[] menus;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
        currentlyActiveButton = 0;
        if(buttons != null && menuOpen) {
            SetButtonActive();
        }
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(pauseMenuOpen) {
    
                CloseMenu(menus[0]);
                pauseMenuOpen = false;
                ResumeGame();
                
            }else{
                OpenMenu(menus[0]);
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
        GetCurrentMenu();
        if(GameGlobals.Instance != null){
            GameGlobals.Instance.globalMenuOpen = true;
        }
        StartCoroutine(AnimateMenuChange(.5f, menu, currentMenuOpen));
    }

    public void CloseMenu(GameObject menu) {
        if(GameGlobals.Instance != null){
            GameGlobals.Instance.globalMenuOpen = true;
        }
        menu.SetActive(false);
    }

    IEnumerator AnimateMenuChange(float animationDuration, GameObject menu, GameObject currentMenu) {
        currentMenu.transform.LeanMoveLocal(new Vector2(-1000, 0), animationDuration).setEaseOutQuart();
        am.Play("Swoosh");
        menu.SetActive(true);
        menu.transform.position = new Vector2(1000, 0);
        menu.transform.LeanMoveLocal(new Vector2(0, 0), animationDuration).setEaseOutQuart();
        yield return new WaitForSeconds(animationDuration);
        currentMenuOpen.SetActive(false);
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
        // Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }

    public void setCurrentMenuOpen(GameObject menu) {
        currentMenuOpen = menu;
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
}
