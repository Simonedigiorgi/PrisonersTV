using System.Collections;
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

    private float moveInput;                                                                    // Player movements input

    [BoxGroup("Controls")] public float speed;                                                  // Player speed
    [BoxGroup("Controls")] public float jump;                                                   // Player jump value
    [BoxGroup("Controls")] public float gravity;                                                // Player gravity value
    [BoxGroup("Controls")] public int extraJumpValue;                                           // How many double jumps
    private int extraJumps;                                                                     // Double jump

    [HideInInspector] public bool facingRight;                                                  // Player flip facing

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
	
	void FixedUpdate () {

        // Move inputs
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Set Run animation
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the player face direction
        if (facingRight == false && moveInput < 0)
            Flip();
        else if (facingRight && moveInput > 0)
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

        // Swich between ground bool (Jump)
        anim.SetBool("Grounded", isGrounded);
            
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
