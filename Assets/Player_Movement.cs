using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float originalMoveSpeed = 5f;

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
    private float originalEmissionRate;
    private ParticleSystem.EmissionModule emissionModule; 


    // private Alteruna.Avatar avatar;

    private void Awake() {
        Time.timeScale = 1;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = originalMoveSpeed;
        am = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM");
        emissionModule = moveParticles.emission;
        originalEmissionRate = emissionModule.rateOverTime.constant;

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

        if(Input.GetKeyDown(KeyCode.Space) && GetComponent<Upgrades>().dashAcquired && dashRecharged) {
            StartCoroutine(Dash());
        }


    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator Dash() {
        dashRecharged = false;
        am.Play("Dash");
        gm.GetComponent<Juicer>().ApplyCameraShake();
        // Add Particles
        Vector2 direction = rb.velocity.normalized;
        float currentSpeed = moveSpeed;
        moveSpeed *= dashMultiplier;
        yield return new WaitForSeconds(.3f);
        moveSpeed = currentSpeed;
        StartCoroutine(RechargeDash());
    }

    IEnumerator RechargeDash() {
        yield return new WaitForSeconds(2);
        dashRecharged = true;
    }
}

