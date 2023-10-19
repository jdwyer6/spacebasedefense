using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;
using TMPro;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    private GameObject gm;
    bool buildToolTipHasOpened;
    public float timeToShowToolTip = 4;

    public GameObject templatePrefab;
    public Transform toolTipContainer;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");
    }


    public void ShowToolTip(string title, string description) {
        var newToolTip = Instantiate(templatePrefab, toolTipContainer.transform);

        Transform templateTitle = newToolTip.transform.Find("Title");
        Transform templateDescription = newToolTip.transform.Find("Description");

        templateTitle.GetComponent<TextMeshProUGUI>().text = title;
        templateDescription.GetComponent<TextMeshProUGUI>().text = description;
        StartCoroutine(InitiateShowToolTip(newToolTip));
    }

    public IEnumerator InitiateShowToolTip(GameObject toolTip) {
        toolTip.SetActive(true);
        yield return new WaitForSeconds(timeToShowToolTip);
        toolTip.SetActive(false);
    }

}
