﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;                                                                         // Rigidbody components
    private Animator anim;

    [BoxGroup("Components")] public GameObject arm;                                                 // Player's arm
    [BoxGroup("Components")] public GameObject groundCheck;                                         // Player ground collider

    [BoxGroup("Player Inputs")] public string Horizontal;                                           // Get Horiziontal
    [BoxGroup("Player Inputs")] public string Vertical;                                             // Get Vertical
    [BoxGroup("Player Inputs")] public string Shoot;                                                // Shoot with you weapon
    [BoxGroup("Player Inputs")] public string DoJump;                                               // Do a jump

    [BoxGroup("Ground")] public LayerMask groundMask;                                               // Ground mask
    [BoxGroup("Ground")] public float groundRadius;                                                 // Ground collider radius

    private bool isGrounded;                                                                        // Is the Player on ground?                                                                        

    [BoxGroup("Controls")] public float speed;                                                      // Player speed
    [BoxGroup("Controls")] public float jump;                                                       // Player jump value

    [BoxGroup("Power Ups")] public int extraJumpValue;                                              // How many double jumps

    private float joypadDeathZone = 0.2f;                                                           // Movement death zone
    private int extraJumps;                                                                         // Double jump

    [HideInInspector] public bool facingRight;                                                      // Player flip facing

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        // Disable 360° arm sprite
        arm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        // Move inputs
        float moveInput = Input.GetAxis(Horizontal);

        // Movements
        if (moveInput >= joypadDeathZone)                                                            // Move right if "x" axis is over 0.2
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else if (moveInput <= -joypadDeathZone)                                                      // Move left if "x" axis is lover -0.2
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else                                                                                         // Stop the player 
            rb.velocity = new Vector2(0, rb.velocity.y);

        // Set Run animation
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the player direction
        if (facingRight == false && moveInput < 0)
            PlayerFlip();
        else if (facingRight && moveInput > 0)
            PlayerFlip();
    }

    private void Update()
    {
        // If you've got the weapon
        if (arm.transform.GetChild(0).childCount == 1)
        {
            // Enable 360° arm sprite
            arm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;

            // Enable The rotation of joystick
            JoyRotation();

            // Get the weapon
            Weapon weapon = arm.transform.GetChild(0).GetChild(0).GetComponent<Weapon>();

            if (weapon.autoFire == false)
            {
                if (Input.GetButtonDown(Shoot) && weapon.isGrabbed)
                    weapon.Shoot();
            }
            if (weapon.autoFire)
            {
                if (Input.GetButton(Shoot) && weapon.isGrabbed)
                    weapon.Shoot();
            }

            // Rotate the weapon when equipped
            if (facingRight && weapon.isGrabbed)
                weapon.transform.localEulerAngles = new Vector3(180, 0, 0);
            else if (!facingRight && weapon.isGrabbed)
                weapon.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        Jump();
    }

    // Flip the player face method
    public void PlayerFlip()
    {
        facingRight = !facingRight;
        Vector3 rotation = transform.localEulerAngles;

        if (facingRight)
        {
            arm.transform.GetChild(1).localEulerAngles = new Vector3(180, 0, 20);                   // Rotate the arm
            rotation.y = 180;                                                                       // Rotate the player
        }

        else
        {
            arm.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 20);                     // Rotate the arm
            rotation.y = 0;                                                                         // Rotate the player
        }
            
        transform.localEulerAngles = rotation;
    }

    // Rotate the Joystick of 360°
    public void JoyRotation()
    {
        Vector3 joyPosition = new Vector3(Input.GetAxis(Horizontal), Input.GetAxis(Vertical), 0);

        float angle = Mathf.Atan2(joyPosition.y, joyPosition.x) * Mathf.Rad2Deg;

        if (angle == 0 && facingRight)
            angle = 180;

        arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Jump && Double jump
    public void Jump()
    {
        // Check groundmask
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundRadius, groundMask);

        // Swich between ground bool (Jump)
        anim.SetBool("Grounded", isGrounded);

        if (isGrounded)
            extraJumps = extraJumpValue;

        if (Input.GetButtonDown(DoJump) && extraJumps > 0)
        {
            extraJumps--;
            rb.velocity = Vector2.up * jump;
        }
        else if (Input.GetButtonDown(DoJump) && extraJumps == 0 && isGrounded)
            rb.velocity = Vector2.up * jump;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore colliders between players
        if(collision.gameObject.CompareTag("Player_1") || collision.gameObject.CompareTag("Player_2"))
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_1") || collision.gameObject.CompareTag("Player_2"))
            Debug.Log("Trigger");
    }
}
