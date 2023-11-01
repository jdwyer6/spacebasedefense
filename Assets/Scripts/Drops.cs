using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    private GameObject gm;
    private List<Drop> drops;
    private int probabilityToDrop = 5;
    public GameObject dropPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");
        drops = gm.GetComponent<Data>().drops;
        probabilityToDrop = gm.GetComponent<Data>().probabilityEnemiesWillDrop;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool ShouldDrop() {
        int randomNum = UnityEngine.Random.Range(0, 100);
        if(randomNum <= probabilityToDrop) {
            return true;
        }
        return false;
    }

    public void DropItem(Vector2 pos) {
        if(!ShouldDrop()) {
            return;
        }

        var randomDrop = drops[UnityEngine.Random.Range(0, drops.Count)];
        var drop = Instantiate(dropPrefab, pos, Quaternion.identity);

        drop.GetComponent<DroppedItem>().drop = randomDrop;
        drop.GetComponent<SpriteRenderer>().sprite = randomDrop.sprite;
        gm.GetComponent<Data>().drops.Remove(randomDrop);

    }
}