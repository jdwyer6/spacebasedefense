using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float originalMoveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator anim;

    // private Alteruna.Avatar avatar;

    private void Awake() {
        Time.timeScale = 1;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = originalMoveSpeed;
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

        // Set the movement vector
        movement = new Vector2(moveX, moveY).normalized;

        if (movement != Vector2.zero) 
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            anim.SetBool("Move", true);
            anim.SetTrigger("StartMove");
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }else{
            anim.SetBool("Move", false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

