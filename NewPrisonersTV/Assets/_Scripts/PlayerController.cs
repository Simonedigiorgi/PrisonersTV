using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    public float speed;
    public float jump;
    public float gravity;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool facingRight = true;

    private int extraJumps;
    public int extraJumpValue;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(facingRight == false && moveInput > 0)
            Flip();
        else if(facingRight && moveInput < 0)
            Flip();
    }

    private void Update()
    {
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

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
