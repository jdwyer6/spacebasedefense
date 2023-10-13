using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instakill : MonoBehaviour
{
    public float timeActive = 30;

    public void InitiateInstakill() {
        StartCoroutine(StartInstakill());
    }

    public IEnumerator StartInstakill() {
        GetComponent<Player_Shooting>().instakillActive = true;
        yield return new WaitForSeconds(timeActive);
        GetComponent<Player_Shooting>().instakillActive = false;
    }
}
