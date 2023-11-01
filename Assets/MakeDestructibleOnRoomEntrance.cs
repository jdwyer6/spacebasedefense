using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDestructibleOnRoomEntrance : MonoBehaviour
{
    public GameObject nonDestructibleSection;
    private AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            nonDestructibleSection.SetActive(false);
            am.Play("RemoveNonDestructibleEnvironment");
        }
    }

}
