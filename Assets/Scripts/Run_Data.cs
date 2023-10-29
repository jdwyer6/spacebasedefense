using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Data : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public Character selectedCharacter;
}
