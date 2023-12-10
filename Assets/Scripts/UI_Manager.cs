using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public IEnumerator ShowToolTip(GameObject toolTip, float timeToShow) {
        toolTip.SetActive(true);
        yield return new WaitForSeconds(timeToShow);
        toolTip.SetActive(false);
    }
}
