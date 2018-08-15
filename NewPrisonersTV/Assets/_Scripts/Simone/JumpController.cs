using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class JumpController : MonoBehaviour {

    private PlayerController player;                                                                // Get PlayerController script
    private Rigidbody2D rb;                                                                         // Rigidbody components

    [BoxGroup("Components")] public GameObject groundCheck;                                         // Player ground collider
    [BoxGroup("Player Inputs")] public string jumpInput;                                            // Player1_Button A || Player2_Button A

    [BoxGroup("Ground")] public LayerMask groundMask;                                               // Ground mask
    [BoxGroup("Ground")] public float groundRadius;                                                 // Ground collider radius

    [BoxGroup("Controls")] public float jump;                                                       // Player jump value
    [BoxGroup("Controls")] public int extraJumpValue;                                               // How many double jumps

    private bool isGrounded;                                                                        // Is the Player on ground?   
    private int extraJumps;                                                                         // Double jump

    void Awake()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update () {

        // Check groundmask
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundRadius, groundMask);

        // Swich between ground bool (Jump)
        player.playerAnim.SetBool("Grounded", isGrounded);

        // This animate the arm without the weapon
        player.armAnim.SetBool("Grounded", isGrounded);

        if (isGrounded)
            extraJumps = extraJumpValue;

        if (player.isActive)
        {
            // Jump Input
            if (Input.GetButtonDown(jumpInput) && extraJumps > 0)
            {
                extraJumps--;
                rb.velocity = Vector2.up * jump;
            }
            else if (Input.GetButtonDown(jumpInput) && extraJumps == 0 && isGrounded)
                rb.velocity = Vector2.up * jump;
        }
    }
}
