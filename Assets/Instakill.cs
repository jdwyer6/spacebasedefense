using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instakill : MonoBehaviour
{
    public float timeActive = 30;
    private GameObject dropsContainer;

    private void Start() {
        dropsContainer = GameObject.FindGameObjectWithTag("DropsUIContainer");
    }

    public void InitiateInstakill() {
        StartCoroutine(StartInstakill());
    }

    public IEnumerator StartInstakill() {
        GetComponent<Player_Shooting>().instakillActive = true;
        yield return new WaitForSeconds(timeActive);

        Transform childTransform = dropsContainer.transform.Find("instakill");
        if (childTransform != null) {
            childTransform.gameObject.SetActive(false);
        } else {
            Debug.LogWarning("Couldn't find 'instakill' child in 'DropsUIContainer'.");
        }
        
        GetComponent<Player_Shooting>().instakillActive = false;
    }
}
