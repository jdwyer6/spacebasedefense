using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

public class ToolTipManager : MonoBehaviour
{
    private GameObject gm;
    bool buildToolTipHasOpened;
    public float timeToShowToolTip = 4;
    int numberOfTimesToShowBuildTip = 4;

    public GameObject build;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");
    }


    public void ShowToolTip(string toolTipName) {
        StartCoroutine(InitiateShowToolTip(ProcessParameters(toolTipName)));
    }

    public IEnumerator InitiateShowToolTip(GameObject toolTip) {
        toolTip.SetActive(true);
        yield return new WaitForSeconds(timeToShowToolTip);
        toolTip.SetActive(false);
    }

    private GameObject ProcessParameters(string toolTip) {
        switch (toolTip)
        {
            case "build":
                return build;

            default:
                Debug.Log("Unknown day.");
                return build;
        }

    }

}
