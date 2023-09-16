using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Handle_Game_Over : MonoBehaviour
{
    GameObject player;
    bool gameOver;
    public GameObject gameOverScreen;
    // public GameObject playerDeathParticles;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player_Health>().isDead && !gameOver) {
            gameOver = true;
            player.SetActive(false);
            GameGlobals.Instance.globalMenuOpen = true;
            // Instantiate(playerDeathParticles, player.transform.position, Quaternion.identity);
            gameOverScreen.SetActive(true);
        }
    }

    public void RestartGame() 
    {
        // Reloads the current scene, thus restarting the game
        GameGlobals.Instance.globalMenuOpen = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void QuitGame()
    {
        #if UNITY_EDITOR
            // If running in the Unity editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running in a build
            Application.Quit();
        #endif
    }
}
