using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Self_Destruct : MonoBehaviour
{
    public float selfDestructTime = 8;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }
}
