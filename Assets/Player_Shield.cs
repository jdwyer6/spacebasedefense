using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Shield : MonoBehaviour
{
    public GameObject shieldPrefab;
    private Player_Health playerHealth;
    private Player_Movement playerMovement;
    public GameObject shieldImpactParticles;
    private AudioManager am;

    public float shieldActiveForTime;
    public float shieldRechargeTime;
    bool shieldRecharging;
    bool shieldActive = false;
    bool shieldRecharged = true;
    public GameObject shieldIndicator;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<Player_Health>();
        playerMovement = GetComponent<Player_Movement>();
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && shieldActiveForTime >= 0) {
            if(shieldActive == false) {
                am.Play("Shift_Shield_Activated");
            }
            timer = shieldActiveForTime;
            shieldActive = true;
            shieldPrefab.SetActive(true);
            playerHealth.canTakeDamage = false;
            playerMovement.canMove = false;
        } else {
            shieldActive = false;
            shieldPrefab.SetActive(false);
            playerHealth.canTakeDamage = true;
            playerMovement.canMove = true;
            
        }

        shieldIndicator.GetComponent<Image>().fillAmount = timer / shieldActiveForTime;

        if(shieldActive && shieldRecharged) {
            timer -= Time.deltaTime;
        } else {
            timer += Time.deltaTime;
        }

        if(timer >= shieldActiveForTime) {
            shieldRecharged = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy_Projectile") {
            am.Play("Shift_Shield_Impact");
            Instantiate(shieldImpactParticles, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
