using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenAttack : MonoBehaviour
{
    public GameObject indicator;
    public GameObject attack;

    public float timeToIndicate;
    public float timeToAttack;
    public float timeBetweenAttacks;
    public string indicationSound;
    public string attackSound;
    private Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        // startColor = Color.red;
        // startColor.a = 0;
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Attack() {
        while(true) {
        indicator.SetActive(true);
        // StartCoroutine(FadeInSprite());
        yield return new WaitForSeconds(timeToIndicate);
        //play sound
        indicator.SetActive(false);
        attack.SetActive(true);
        yield return new WaitForSeconds(timeToAttack);
        attack.SetActive(false);
        yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    IEnumerator FadeInSprite()
    {
        while (startColor.a < 1.0f)
        {
            SpriteRenderer[] childSprites = indicator.GetComponentsInChildren<SpriteRenderer>(); 
            foreach (var spriteRenderer in childSprites)
            {
                startColor.a += Time.deltaTime / timeToIndicate;
                spriteRenderer.color = startColor;
            }

            yield return null;
        }
    }
}
