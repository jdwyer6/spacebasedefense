using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Random_Quick_AddOn : MonoBehaviour
{
    private float startingSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveFast());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveFast() {
        while(true) {
            GetComponent<Enemy_Movement>().speed = startingSpeed;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, 10));
            GetComponent<Enemy_Movement>().speed *= 5;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.2f, 1f));
        }
    }
}
