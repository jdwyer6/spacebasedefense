using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    private bool wasActive;

    // Start is called before the first frame update
    void Start()
    {
        wasActive = gameObject.activeSelf;
        StartCoroutine(ShowToolTip());
    }

   private void Update()
    {
        if (wasActive != gameObject.activeSelf)
        {
            wasActive = gameObject.activeSelf;

            if (wasActive)
            {
                OnSetActive();
            }
            else
            {
                OnSetInactive();
            }
        }
    }

    private void OnSetActive()
    {
        StartCoroutine(ShowToolTip());
    }

    private void OnSetInactive()
    {

    }


    IEnumerator ShowToolTip() {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
    }
}
