﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;                                                                     // Rigidbody components

    public Transform groundCheck;                                                               // Player ground collider
    public LayerMask groundMask;                                                                // Ground mask

    private bool isGrounded;                                                                    // Is the Player on ground?                                                                        
    public float checkRadius;                                                                   // Ground collider radius

    private float moveInput;                                                                    // Player movements input

    public float speed;                                                                         // Player speed
    public float jump;                                                                          // Player jump value
    public float gravity;                                                                       // Player gravity value

    [HideInInspector] public bool facingRight;                                                  // Player flip facing

    private int extraJumps;                                                                     // Double jump
    public int extraJumpValue;                                                                  // How many double jumps

	void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate () {

        // Move inputs
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Flip the player face direction
        if(facingRight == false && moveInput < 0)
            Flip();
        else if(facingRight && moveInput > 0)
            Flip();
    }

    private void Update()
    {
        // Check groundmask
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);

        // Jump && Double Jump
        if (isGrounded)
            extraJumps = extraJumpValue;

        if (Input.GetButtonDown("Fire1") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jump;
            extraJumps--;
        }
        else if(Input.GetButtonDown("Fire1") && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jump;
        }
    }

    // Flip the player face method
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
