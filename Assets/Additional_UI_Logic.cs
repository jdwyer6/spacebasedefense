using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Additional_UI_Logic : MonoBehaviour
{
    public GameObject keyBindingText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutKeyBindings());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOutKeyBindings() {
        yield return new WaitForSeconds(5f);
        keyBindingText.SetActive(false);

    }
}
