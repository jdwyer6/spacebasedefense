using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacles_Controller : MonoBehaviour
{
    public GameObject[] hooks;
    public GameObject[] hookFollowLocations;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hooks.Length; i++)
        {
            hooks[i].transform.position = hookFollowLocations[i].transform.position;
        }
    }
}
