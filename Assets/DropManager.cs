using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public float timeActive = 20;
    private GameObject dropsContainer;
    private GameObject player;

    public GameObject playerProjectile;

    private void Start() {
        dropsContainer = GameObject.FindGameObjectWithTag("DropsUIContainer");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void InitiateInstakill() {
        StartCoroutine(StartInstakill());
    }

    public IEnumerator StartInstakill() {
        player.GetComponent<Player_Shooting>().instakillActive = true;
        yield return new WaitForSeconds(timeActive);
        RemoveDropUI("instakill");
        player.GetComponent<Player_Shooting>().instakillActive = false;
    }

    // ----------------------------------------------------------------------

    public void InitiateHoming() {
        StartCoroutine(StartHoming());
    }

    public IEnumerator StartHoming() {
        playerProjectile.GetComponent<Projectile_Homing>().isHoming = true;
        yield return new WaitForSeconds(timeActive);
        playerProjectile.GetComponent<Projectile_Homing>().isHoming = false;
        RemoveDropUI("homing");
    }

    // ----------------------------------------------------------------------

    public void InitiateLaser() {
        StartCoroutine(StartLaser());
    }

    private IEnumerator StartLaser() {
        player.GetComponent<Player_Shooting>().laserActive = true;
        yield return new WaitForSeconds(timeActive);
        player.GetComponent<Player_Shooting>().laserActive = false;
        RemoveDropUI("laser");
    }

    // ----------------------------------------------------------------------

    private void RemoveDropUI(string code) {
        Transform childTransform = dropsContainer.transform.Find(code);
        if (childTransform != null) {
            childTransform.gameObject.SetActive(false);
        } else {
            Debug.LogWarning("Couldn't find " + code + " child in 'DropsUIContainer'.");
        }
    }
}
