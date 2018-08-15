﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DashController : MonoBehaviour {

    PlayerController player;                                                                        // Get PlayerController script
    private Rigidbody2D rb;                                                                         // Rigidbody components

    [BoxGroup("Player Inputs")] public string dashInput;                                            // Do a dash (Player1_Button B || Player2_Button B)

    [BoxGroup("Controls")] public float dashPower;                                                  // Player dash power
    [BoxGroup("Controls")] public float dashTimer;                                                  // How many time in dash 

    [BoxGroup("Power Up")] public bool powerDash;                                                   // PowerUp enabled? (You can perform the dash without gravityscale)

    void Awake()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update () {

        //Dash condition
        if (Input.GetButtonDown(dashInput) && player.isInDash == false)
        {
            player.isInDash = true;
            player.playerAnim.SetTrigger("Dash");

            // Disable arm without the weapon
            transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

            // perform dash without the gravity (PowerUp)
            if (powerDash)
            {
                rb.gravityScale = 0;
            }

            if (player.facingRight)
                rb.velocity = Vector2.left * dashPower;
            else
                rb.velocity = Vector2.right * dashPower;

            StartCoroutine(ResetDash());
        }
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashTimer);
        player.isInDash = false;

        // Reset the gravity
        rb.gravityScale = 10;

        // Enable arm without the weapon
        transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }
}
