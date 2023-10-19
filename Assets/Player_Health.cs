using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public Slider healthBar; // Reference to the health bar UI slider
    
    public float currentHealth; // Current health of the player
    public bool canTakeDamage = true;
    private AudioManager am;
    private GameObject gm;
    private Data data;
    public float targetHealthValue; // The target value for the health slider
    public float healthSliderSpeed = 5f;
    public bool isDead;
    bool isChangingColor = false;
    public Animator anim;

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM");
        data = gm.GetComponent<Data>();
        maxHealth = 100f;
        currentHealth = maxHealth; // Initialize current health to max health
        am = FindObjectOfType<AudioManager>();
        targetHealthValue = currentHealth; // Initialize the target value
        UpdateHealthBar();
        isDead = false;
    }

    private void Update() {
        if (healthBar)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, targetHealthValue / maxHealth, Time.deltaTime * healthSliderSpeed);
        }

        if(currentHealth <= 0){
            isDead = true;
        }
    }

    // Method to make the player take damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        targetHealthValue = currentHealth; // Set the new target value
        anim.SetTrigger("Hurt");
        StartCoroutine(ChangeColor());
        am.Play(data.playerDamageSounds[Random.Range(0, data.playerDamageSounds.Length)]);
        
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateSpriteDamage();
        StartCoroutine(InitiateInvulnerabilityAfterTakingHit());
    }

    // Method to heal the player
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        targetHealthValue = currentHealth; 
        UpdateSpriteDamage();
    }

    // Update the health bar to match current health
    private void UpdateHealthBar()
    {
        if (healthBar != null) // Check if healthBar reference is set
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(canTakeDamage) {
            if(other.gameObject.tag == "Enemy_Projectile") {
                TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
            }
            if(other.gameObject.tag == "Enemy") {
                TakeDamage(other.gameObject.GetComponent<Enemy_Data>().damage);
            }
        }
    }

    IEnumerator ChangeColor() {
        if(isChangingColor)
            yield break;

        isChangingColor = true;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        if(spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1.0f, 0.286f, 0.286f);
            StartCoroutine(gm.GetComponent<Juicer>().ApplyHitStop(.2f));
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = currentColor;
        }
        
        isChangingColor = false;
    }

    private void UpdateSpriteDamage() {

        if(currentHealth >= 60 && currentHealth < 100) {
            spriteRenderer.sprite = sprites[1];
        }else if(currentHealth >= 30 && currentHealth < 60) {
            spriteRenderer.sprite = sprites[2];
        }else if(currentHealth < 30){
            spriteRenderer.sprite = sprites[3];
        }else{
            spriteRenderer.sprite = sprites[0];
        }
    }

    IEnumerator InitiateInvulnerabilityAfterTakingHit() {
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
    }
}
