using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    public GameObject outline;
    public GameObject missileEffect;
    private GameObject player;
    private AudioManager am;
    public string impactSound;

    public float intervalToSpawnMissiles;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(StartSpawningMissiles());
    }

    IEnumerator InitiateMissile() {
        Vector2 playerPos = player.transform.position;
        GameObject newOutline = Instantiate(outline, playerPos, Quaternion.identity);
        yield return new WaitForSeconds(1);
        newOutline.SetActive(false);
        am.Play(impactSound);
        var newMissile = Instantiate(missileEffect, playerPos, Quaternion.identity);
        yield return new WaitForSeconds(.2f);
        newMissile.gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        newMissile.SetActive(false);
    }

    IEnumerator StartSpawningMissiles() {
        while(true) {
            StartCoroutine(InitiateMissile());
            yield return new WaitForSeconds(intervalToSpawnMissiles);
        }
    }
}
