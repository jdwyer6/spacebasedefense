using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameObjects : MonoBehaviour
{
    public GameObject playerPrefab;

    void Awake()
    {
        GameObject[] playersInScene = GameObject.FindGameObjectsWithTag("Player");

        if(playersInScene.Length == 0)
        {
            Instantiate(playerPrefab);
        }

    }
}
