using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Character_Button : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI name;
    public TextMeshProUGUI unlockableDescription;
    private bool unlocked;
    public GameObject padlock;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        name.text = character.characterName;
        unlockableDescription.text = character.unlockableDescription;
        unlocked = character.unlocked;
        image.sprite = character.sprite;

        if(!character.unlocked) {
            padlock.SetActive(true);
            unlockableDescription.enabled = true;
        }else{
            unlockableDescription.enabled = false;
            padlock.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
