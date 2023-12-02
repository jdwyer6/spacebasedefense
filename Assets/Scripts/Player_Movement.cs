using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float originalMoveSpeed = 5f;
    public bool canMove = true;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool dashRecharged = true;
    public float dashMultiplier = 2;
    private GameObject gm;
    private AudioManager am;

    [Header("Animation")]
    public Animator anim;
    bool isMoving = false;

    public ParticleSystem moveParticles;
    public ParticleSystem dashParticles;
    private float originalEmissionRate;
    private float dashEmissionRate = 1000;
    private ParticleSystem.EmissionModule emissionModule; 
    private ParticleSystem.EmissionModule dashEmissionModule; 
    UpgradeLogicType deadlyDash = UpgradeLogicType.deadlyDash;

    public GameObject deadlyDashCollider;
    public ParticleSystem deadlyDashParticles1;
    public ParticleSystem deadlyDashParticles2;
    private bool isDeadlyDashing;


    // private Alteruna.Avatar avatar;

    private void Awake() {
        Time.timeScale = 1;
        moveSpeed = originalMoveSpeed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        emissionModule = moveParticles.emission;
        dashEmissionModule = dashParticles.emission;
        originalEmissionRate = emissionModule.rateOverTime.constant;
        canMove = true;

        // avatar = GetComponent<Alteruna.Avatar>();
        // if(!avatar.IsOwner){
        //     return;
        // }
    }

    private void Update()
    {
        // if(!avatar.IsOwner){
        //     return;
        // }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX != 0 || moveY != 0) {
            emissionModule.rateOverTime = originalEmissionRate; 
            if(!isMoving) {
                isMoving = true;
                anim.SetTrigger("Move");
                anim.SetBool("isMoving", true);
            }
        }else{
            anim.SetBool("isMoving", false);
            isMoving = false;
            emissionModule.rateOverTime = 0f;
        }

        // Set the movement vector
        movement = new Vector2(moveX, moveY).normalized;

        if (movement != Vector2.zero) 
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if(Input.GetButtonDown("Jump") && dashRecharged) {
            StartCoroutine(Dash());
        }


        DamageEnemiesDuringDeadlyDash();
    }

    private void FixedUpdate()
    {
        if(canMove) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }else{
            rb.velocity = Vector2.zero;
        }
        
    }

    IEnumerator Dash() {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        dashEmissionModule.rateOverTime = dashEmissionRate; 
        dashRecharged = false;
        // GetComponent<PolygonCollider2D>().enabled = false;
        am.Play("Dash");
        gm.GetComponent<Juicer>().ApplyCameraShake();
        Vector2 direction = rb.velocity.normalized;
        float currentSpeed = moveSpeed;
        moveSpeed *= dashMultiplier;

        if(Array.Find(Helper.GetUpgrades(), upgrade => upgrade.upgradeLogic == deadlyDash).acquired) {
            StartCoroutine(StartDeadlyDash());
        }



        yield return new WaitForSeconds(.2f);
        dashEmissionModule.rateOverTime = 0f; 
        moveSpeed = currentSpeed;
        StartCoroutine(RechargeDash());
        // GetComponent<PolygonCollider2D>().enabled = true;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }

    IEnumerator RechargeDash() {
        GameObject dashToolTip = GetComponent<Upgrades>().dashToolTip;
        Image dashImage = dashToolTip.GetComponentInChildren<Image>();
        
        dashImage.color = new Color32(255, 73, 73, 255);
        yield return new WaitForSeconds(.75f);
        
        dashRecharged = true;
        dashImage.color = Color.white;
    }

    IEnumerator StartDeadlyDash() {
        isDeadlyDashing = true;
        deadlyDashParticles1.Play();
        deadlyDashParticles2.Play();
        yield return new WaitForSeconds(.2f);
        isDeadlyDashing = false;
        deadlyDashParticles1.Stop();
        deadlyDashParticles2.Stop();

    }

    private void DamageEnemiesDuringDeadlyDash() {
        if(isDeadlyDashing) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if(distanceToEnemy <= 2f) {
                    enemy.GetComponent<Enemy_Health>().TakeDamage(50f);
                }
            }
        }
    }

}

