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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        am = FindObjectOfType<AudioManager>();
        toolTipManager = GameObject.FindGameObjectWithTag("ToolTipManager").GetComponent<ToolTipManager>();
        dropUIContainer = GameObject.FindGameObjectWithTag("DropsUIContainer");
    }

    // Update is called once per frame
    void Update()
    {
        
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
                player.GetComponent<Instakill>().InitiateInstakill();
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
