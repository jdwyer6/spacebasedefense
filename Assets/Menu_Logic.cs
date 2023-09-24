using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Logic : MonoBehaviour
{
    private AudioManager am;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }

    public void HoverSound() {
        // am.Play();
    }

    public void ClickSound() {

    }
}
