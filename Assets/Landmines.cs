using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmines : MonoBehaviour
{
    public GameObject mine;
    private AudioManager am;

    private void Start() {
        am = FindObjectOfType<AudioManager>();
    }

    public void InitiateLandmines() {
        StartCoroutine(LayMines());
    }

    IEnumerator LayMines() {
        while(true) {
            Instantiate(mine, transform.position, Quaternion.identity);
            am.Play("Beep");
            yield return new WaitForSeconds(20);
        }
    }
}
