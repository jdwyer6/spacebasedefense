using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniBulletHellHelper : MonoBehaviour
{
    private AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShotAuto1() {
        am.Play("Enemy_Shot_2");
    }
}
