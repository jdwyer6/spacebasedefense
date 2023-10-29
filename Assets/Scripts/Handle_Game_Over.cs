using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Handle_Game_Over : MonoBehaviour
{
    GameObject player;
    bool gameOver;
    public GameObject gameOverScreen;
    public GameObject playerDeathParticles;
    private AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
        am = FindObjectOfType<AudioManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null){
            if(player.GetComponent<Player_Health>().isDead && !gameOver) {
                gameOver = true;
                player.GetComponent<Player_Health>().healthBar.value = 0;
                player.SetActive(false);
                am.Stop("WaveSoundtrack");
                var particles = Instantiate(playerDeathParticles, player.transform.position, Quaternion.identity);
                var particleSystem = particles.GetComponentInChildren<ParticleSystem>();
                var mainModule = particleSystem.main;
                mainModule.startColor = Color.white;
                StartCoroutine(HandleGameOver());
            }
        }

    }

    IEnumerator HandleGameOver() {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1); 
        Time.timeScale = 0;
        GameGlobals.Instance.globalMenuOpen = true;
        gameOverScreen.SetActive(true);

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

    private void OnDestroy()
    {
        // Important to unsubscribe from events to prevent memory leaks or unwanted behavior
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        am.Play("WaveSoundtrack");
    }
}
