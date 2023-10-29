using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowToolTip(GameObject toolTip, float timeToShow) {
        toolTip.SetActive(true);
        yield return new WaitForSeconds(timeToShow);
        toolTip.SetActive(false);
    }
}
