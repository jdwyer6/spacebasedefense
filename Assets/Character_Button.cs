using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Character_Button : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI unlockableDescription;
    private bool unlocked;
    public GameObject padlock;
    public Image image;
    public GameObject outline;
    private AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        characterName.text = character.characterName;
        unlockableDescription.text = character.unlockableDescription;
        unlocked = character.unlocked;
        image.sprite = character.sprite;

        if(!character.unlocked) {
            padlock.SetActive(true);
            unlockableDescription.enabled = true;
            GetComponent<Button>().interactable = false; 
        }else{
            unlockableDescription.enabled = false;
            padlock.SetActive(false);
            GetComponent<Button>().interactable = true; 
        }

        GetComponent<Button>().onClick.AddListener(OnCharacterButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if(character.selected) {
            outline.SetActive(true);
        }else{
            outline.SetActive(false);
        }
    }

    void OnCharacterButtonClick()
    {
        if (!unlocked)
        {
            am.Play("UI_Disabled");
        }
    }
}
