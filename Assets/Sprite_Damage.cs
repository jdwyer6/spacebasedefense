using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Damage : MonoBehaviour
{
    public Sprite[][] damagedSpritesCollection;
    public Sprite[] damagedSpritesOption_1;
    public Sprite[] damagedSpritesOption_2;
    public Sprite[] damagedSpritesOption_3;

    private Sprite[] selectedSpriteArray;


    private Enemy_Health enemyHealth;
    private SpriteRenderer spriteRenderer;
    private int currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        damagedSpritesCollection = new Sprite[][] 
        {
            damagedSpritesOption_1, 
            damagedSpritesOption_2, 
            damagedSpritesOption_3
        };

        int randomNum = UnityEngine.Random.Range(0, damagedSpritesCollection.Length);
        selectedSpriteArray = damagedSpritesCollection[randomNum];

        enemyHealth = GetComponent<Enemy_Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth.currentHealth <= enemyHealth.totalHealth * .66666f && currentSprite < 1) {
            SwapSprite();
        }else if(enemyHealth.currentHealth <= enemyHealth.totalHealth * .33333f && currentSprite < 2) {
            SwapSprite();
        }
    }

    private void SwapSprite() {
        spriteRenderer.sprite = selectedSpriteArray[currentSprite];
        currentSprite++;
    }
}
