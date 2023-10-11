using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

public class ToolTipManager : MonoBehaviour
{
    private GameObject gm;
    bool buildToolTipHasOpened;
    int numberOfTimesToShowBuildTip = 4;
    // public GameObject buildToolTip;

    [SerializeField] private NotificationManager buildToolTip;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.GetComponent<Enemy_Spawner>().isBreak == false && !buildToolTipHasOpened && numberOfTimesToShowBuildTip > 0) {
            numberOfTimesToShowBuildTip -= 1;
            buildToolTipHasOpened = true;
            buildToolTip.Open();
        }   
        
        if(gm.GetComponent<Enemy_Spawner>().isBreak && buildToolTipHasOpened) {
            buildToolTipHasOpened = false;
        }

    }

}
