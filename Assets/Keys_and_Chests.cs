using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys_and_Chests : MonoBehaviour
{
    public int keys;
    private AudioManager am;
    private ToolTipManager toolTipManager;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        toolTipManager = GameObject.FindGameObjectWithTag("ToolTipManager").GetComponent<ToolTipManager>();
        keys = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Chest_Door") {
            if(keys > 0) {
                am.Play("Door_Open");
                keys--;
                Destroy(other.gameObject);
            }else{
                toolTipManager.ShowToolTip("Missing Key", "You need a key to enter this area");
            }
        }
    }
}
