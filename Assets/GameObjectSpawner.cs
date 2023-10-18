using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(!player) {
            Instantiate(playerPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
