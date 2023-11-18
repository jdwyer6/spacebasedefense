using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameObjects : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject gameManagerPrefab;
    public AudioManager amPrefab;

    void Awake()
    {
        GameObject[] playersInScene = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] gameManagerInScene = GameObject.FindGameObjectsWithTag("GM");
        AudioManager audiomanagerInScene = FindObjectOfType<AudioManager>();

        if(gameManagerInScene.Length == 0)
        {
            Instantiate(gameManagerPrefab);
        }

        if(audiomanagerInScene == null)
        {
            Instantiate(amPrefab);
        }

        if(playersInScene.Length == 0)
        {
            Instantiate(playerPrefab);
        }


    }
}
