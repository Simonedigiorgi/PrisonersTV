﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;                                                                     // Rigidbody components
    private Animator anim;

    [BoxGroup("Ground")] public Transform groundCheck;                                          // Player ground collider
    [BoxGroup("Ground")] public LayerMask groundMask;                                           // Ground mask
    [BoxGroup("Ground")] public float checkRadius;                                              // Ground collider radius
    private bool isGrounded;                                                                    // Is the Player on ground?                                                                        

    [BoxGroup("Controls")] public float speed;                                                  // Player speed
    [BoxGroup("Controls")] public float jump;                                                   // Player jump value
    [BoxGroup("Controls")] public float gravity;                                                // Player gravity value
    [BoxGroup("Controls")] public int extraJumpValue;                                           // How many double jumps
    [BoxGroup("Controls")] public float joypadDeathZone;                                        // Movement death zone

    private int extraJumps;                                                                     // Double jump

    [HideInInspector] public bool facingRight;                                                  // Player flip facing

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
	
	void FixedUpdate () {

        #region Movements
        // Move inputs
        float moveInput = Input.GetAxis("Horizontal");

        // Movements
        if(moveInput >= joypadDeathZone)                                                        // Move right if "x" axis is over 0.2
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else if(moveInput <= -joypadDeathZone)                                                  // Move left if "x" axis is lover -0.2
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else                                                                                    // Stop the player 
            rb.velocity = new Vector2(0, rb.velocity.y);                                        

        // Set Run animation
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the player face direction
        if (facingRight == false && moveInput < 0)
            Flip();
        else if (facingRight && moveInput > 0)
            Flip();
        #endregion
    }

    private void Update()
    {
        #region Jump
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
            rb.velocity = Vector2.up * jump;

        // Swich between ground bool (Jump)
        anim.SetBool("Grounded", isGrounded);
        #endregion
    }

    // Flip the player face method
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 rotation = transform.localEulerAngles;

        if (facingRight)
            rotation.y = 180;                                                                       // Rotate the player
        else
            rotation.y = 0;                                                                         // Rotate the player

        transform.localEulerAngles = rotation;
    }
}
