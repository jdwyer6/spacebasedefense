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
                gm.GetComponent<Data>().drops.Remove(drop);
            }
            if (drop.titleCode == "rateOfFire") {
                am.Play(drop.sound);
                player.GetComponent<Player_Shooting>().autoShootingInterval *= .7f;
                gm.GetComponent<Data>().drops.Remove(drop);
            }

            if (drop.titleCode == "instakill") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateInstakill();
                SetDropUI(drop);
                gm.GetComponent<Data>().drops.Remove(drop);
            }

            if (drop.titleCode == "homing") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateHoming();
                SetDropUI(drop);
                gm.GetComponent<Data>().drops.Remove(drop);
            }

            if (drop.titleCode == "laser") {
                am.Play(drop.sound);
                gm.GetComponent<DropManager>().InitiateLaser();
                SetDropUI(drop);
                gm.GetComponent<Data>().drops.Remove(drop);
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
