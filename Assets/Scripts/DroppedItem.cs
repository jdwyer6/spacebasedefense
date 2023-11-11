using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroppedItem : MonoBehaviour
{
    public Drop drop;
    private GameObject player;
    private AudioManager am;
    private ToolTipManager toolTipManager;
    public GameObject dropUIPrefab;
    private GameObject dropUIContainer;
    private GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GM");
        am = FindObjectOfType<AudioManager>();
        toolTipManager = GameObject.FindGameObjectWithTag("ToolTipManager").GetComponent<ToolTipManager>();
        dropUIContainer = GameObject.FindGameObjectWithTag("DropsUIContainer");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            if (drop.titleCode == "health") {
                am.Play(drop.sound);
                player.GetComponent<Player_Health>().Heal(20);
            }
            if (drop.titleCode == "rateOfFire") {
                am.Play(drop.sound);
                player.GetComponent<Player_Shooting>().autoShootingInterval *= .7f;
            }

            if (drop.titleCode == "instakill") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateInstakill();
                SetDropUI(drop);
            }

            if (drop.titleCode == "homing") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateHoming();
                SetDropUI(drop);
            }

            if (drop.titleCode == "laser") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateLaser();
                SetDropUI(drop);
            }

            if (drop.titleCode == "key") {
                am.Play(drop.sound);
                player.GetComponent<Keys_and_Chests>().keys++;
                SetDropUI(drop);
            }

            if (drop.titleCode == "arcticBlast") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateArcticBlast();
                SetDropUI(drop);
            }

            if (drop.titleCode == "chainReaction") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().IniateChainReaction();
                SetDropUI(drop);
            }

            if (drop.titleCode == "duck") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateDuck();
                SetDropUI(drop);
            }

            
            if (drop.titleCode == "saw") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateSaw();
                SetDropUI(drop);
            }

            toolTipManager.ShowToolTip(drop.title, drop.description);

            Destroy(gameObject);
        }
    }

    private void SetDropUI(Drop drop) {
        var newDropUI = Instantiate(dropUIPrefab, dropUIContainer.transform);
        newDropUI.GetComponent<Image>().sprite = drop.sprite;
        newDropUI.GetComponentInChildren<TextMeshProUGUI>().text = drop.title;
        newDropUI.name = drop.titleCode;
    }
}
