using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Gun[] chestItems;
    private AudioManager am;
    private GameObject gm;
    private Gun randomItem;
    private ToolTipManager toolTipManager;

    public GameObject kissOfDeathUI;
    private GameObject inventoryContainer;
    public GameObject chestParticles;

    public Environment_Mover mover;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        chestItems = gm.GetComponent<Data>().guns;
        randomItem = chestItems[UnityEngine.Random.Range(0, chestItems.Length)];
        inventoryContainer = GameObject.Find("Inventory");
        toolTipManager = GameObject.FindGameObjectWithTag("ToolTipManager").GetComponent<ToolTipManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            am.Play("Open_Chest");
            Instantiate(chestParticles, transform.position, Quaternion.identity);
            if(randomItem.code == "kiss") {
                Debug.Log("isKiss");
                other.gameObject.transform.Find("KissOfDeath").gameObject.SetActive(true);
                Instantiate(kissOfDeathUI, inventoryContainer.transform);
            }
            mover.chestItemCollected = true;
            toolTipManager.ShowToolTip(randomItem.title, randomItem.description);


            Destroy(gameObject);
        }
    }
}
