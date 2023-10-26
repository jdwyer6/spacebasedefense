using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Image imageRadialFill;
    private float timer;
    public float totalTime;
    bool sceneLoaded = false;
    bool runTimer;
    public int sceneToLoad;

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        if(timer >= totalTime && !sceneLoaded) {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2(0, 0);
            SceneManager.LoadScene(sceneToLoad);
            sceneLoaded = true;
        }

        imageRadialFill.fillAmount = timer / totalTime;

        if(runTimer) {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            runTimer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            timer = 0;
            runTimer = false;
            sceneLoaded = false;
        }
    }


}
