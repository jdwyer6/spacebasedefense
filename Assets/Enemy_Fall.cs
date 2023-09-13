using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fall : MonoBehaviour
{
    private Transform player;
    public float heightToFallFrom = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector2(player.position.x, player.position.y + heightToFallFrom);
    }
}
