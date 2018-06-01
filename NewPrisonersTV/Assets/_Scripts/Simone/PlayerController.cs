using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour {

    // *** ALWAYS REMEMBER TO DON'T RENAME THE PLAYER COMPONENT (Hand_Player1)
    // *** THE WEAPON SCRIPT WORKS WITH THAT

    private Rigidbody2D rb;                                                                         // Rigidbody components
    private GameManager gm;                                                                         // GameManager
    private Animator playerAnim, armAnim;                                                           // Get the Player Animators

    [BoxGroup("Components")] public GameObject playerArm;                                           // Player's arm
    [BoxGroup("Components")] public GameObject groundCheck;                                         // Player ground collider

    [BoxGroup("Player Inputs")] public string Horizontal;                                           // Get Horiziontal
    [BoxGroup("Player Inputs")] public string Vertical;                                             // Get Vertical
    [BoxGroup("Player Inputs")] public string Shoot;                                                // Shoot with you weapon
    [BoxGroup("Player Inputs")] public string DoJump;                                               // Do a jump
    [BoxGroup("Player Inputs")] public string DoDash;                                               // Do a dash

    [BoxGroup("Ground")] public LayerMask groundMask;                                               // Ground mask
    [BoxGroup("Ground")] public float groundRadius;                                                 // Ground collider radius

    private bool isGrounded;                                                                        // Is the Player on ground?                                                                        

    [Range(0, 3)]
    [BoxGroup("Controls")] public float life;                                                       // Player life
    [BoxGroup("Controls")] public float speed;                                                      // Player speed
    [BoxGroup("Controls")] public float jump;                                                       // Player jump value
    [BoxGroup("Controls")] public float dashPower;                                                  // Player dash power
    [BoxGroup("Controls")] public float dashTimer;                                                  // How many time in dash 
    [BoxGroup("Controls")] public float respawnTime;                                                // Time before respawning

    [BoxGroup("Power Ups")] public int extraJumpValue;                                              // How many double jumps

    [BoxGroup("Debug")] public bool isActive = true;                                                // Check if the player is active

    private float joypadDeathZone = 0.2f;                                                           // Movement death zone
    private int extraJumps;                                                                         // Double jump

    [HideInInspector] public bool facingRight;                                                      // Player flip facing
    [HideInInspector] public bool isInDash = false;                                                 // Check if the player is in dash

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponentInChildren<Animator>();
        gm = FindObjectOfType<GameManager>();

        // Get arm without weapon (Arm_Anim) child
        armAnim = transform.GetChild(1).GetChild(2).GetComponent<Animator>();

        // Disable 360° arm sprite
        playerArm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

        // Set player alive to false (need for the GameManager to respawn the players)
        if(gameObject.name == "Player 1" || gameObject.name == "Player 1(Clone)")
            gm.isPlayer1alive = false;
        else if(gameObject.name == "Player 2" || gameObject.name == "Player 2(Clone)")
            gm.isPlayer2alive = false;
    }

    public void OnEnable()
    {
        playerAnim = GetComponentInChildren<Animator>();
        playerAnim.Play("Idle");

        armAnim = transform.GetChild(1).GetChild(2).GetComponent<Animator>();
        armAnim.Play("Idle");

        // Set the Arm_sprite sprite to true
        playerArm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

        // Set the Arm_Anim sprite to true
        playerArm.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;

        life = 3;
        isActive = true;
    }

    private void FixedUpdate()
    {
        // *** LEAVE ALL OF THIS ON FIXED UPDATE TO AVOID PROBLEM OCCURING DURING THE FLIP

        // Move inputs
        float moveInput = Input.GetAxis(Horizontal);

        if (isActive)
        {
            // Movements
            if (moveInput >= joypadDeathZone && !isInDash)                                              // Move right if "x" axis is over 0.2
                rb.velocity = new Vector2(speed, rb.velocity.y);
            else if (moveInput <= -joypadDeathZone && !isInDash)                                        // Move left if "x" axis is lower -0.2
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            else if (!isInDash)
                rb.velocity = new Vector2(0, rb.velocity.y);

            // Set Run animation
            playerAnim.SetFloat("Speed", Mathf.Abs(moveInput));

            // This animate the arm without the weapon
            armAnim.SetFloat("Speed", Mathf.Abs(moveInput));

            // Flip the player direction
            if (facingRight == false && moveInput < 0)
                PlayerFlip();
            else if (facingRight && moveInput > 0)
                PlayerFlip();
        }

        // If you've got the weapon
        if (playerArm.transform.GetChild(0).childCount == 1)
        {
            // Enable 360° arm sprite
            playerArm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;

            // Disable arm without the weapon
            transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

            // Get the weapon component
            Weapon weapon = playerArm.transform.GetChild(0).GetChild(0).GetComponent<Weapon>();


            if (isActive)
            {
                // Enable The rotation of joystick
                JoyRotation();

                // Shoot condition
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

                // Flip the weapon when equipped
                if (facingRight && weapon.isGrabbed)
                    weapon.transform.localEulerAngles = new Vector3(180, 0, 0);
                else if (!facingRight && weapon.isGrabbed)
                    weapon.transform.localEulerAngles = new Vector3(0, 0, 0);

                // Rotation of Muzz effect
                MuzzRotation();
            }
        }
    }

    private void Update()
    {
        if (isActive)
        {
            // Player life
            if (life <= 0)
            {
                life = 0;
                StartCoroutine(Death());
            }

            Dash();
        }

        Jump();
    }

    #region Methods
    // Flip the player face method
    public void PlayerFlip()
    {
        facingRight = !facingRight;
        Vector3 rotation = transform.localEulerAngles;

        if (facingRight)
        {
            playerArm.transform.GetChild(1).localEulerAngles = new Vector3(180, 0, 20);             // Rotate the arm
            rotation.y = 180;                                                                       // Rotate the player
        }

        else
        {
            playerArm.transform.GetChild(1).localEulerAngles = new Vector3(0, 0, 20);               // Rotate the arm
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

        playerArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Muzz rotation
    public void MuzzRotation()
    {
        // Get the Muzz component on weapon
        Transform muzzObject = playerArm.transform.GetChild(0).GetChild(0).GetChild(3);

        Vector3 muzzPosition = new Vector3(Input.GetAxis(Horizontal), Input.GetAxis(Vertical), 0);

        float angle = Mathf.Atan2(muzzPosition.y, muzzPosition.x) * Mathf.Rad2Deg;

        if (angle == 0 && facingRight)
            angle = 180;

        muzzObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // This avoid the muzz to have a glitchy position
        if (facingRight)
            muzzObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            muzzObject.GetComponent<SpriteRenderer>().flipY = false;
    }

    // Jump && Double jump
    public void Jump()
    {
        // Check groundmask
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundRadius, groundMask);

        // Swich between ground bool (Jump)
        playerAnim.SetBool("Grounded", isGrounded);

        // This animate the arm without the weapon
        armAnim.SetBool("Grounded", isGrounded);

        if (isGrounded)
            extraJumps = extraJumpValue;

        if (isActive)
        {
            if (Input.GetButtonDown(DoJump) && extraJumps > 0)
            {
                extraJumps--;
                rb.velocity = Vector2.up * jump;
            }
            else if (Input.GetButtonDown(DoJump) && extraJumps == 0 && isGrounded)
                rb.velocity = Vector2.up * jump;
        }
    }

    public void Dash()
    {
        //Dash condition
        if (Input.GetButtonDown(DoDash) && !isInDash)
        {
            isInDash = true;
            playerAnim.SetTrigger("Dash");

            // Disable arm without the weapon
            transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

            if (facingRight)
                rb.velocity = Vector2.left * dashPower;
            else
                rb.velocity = Vector2.right * dashPower;

            StartCoroutine(ResetDash());
        }
    }
    #endregion

    #region Coroutines
    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashTimer);
        isInDash = false;

        // Enable arm without the weapon
        transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }

    public IEnumerator Death()
    {
        // Set the Arm_Anim sprite to false
        playerArm.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

        // Death animation
        playerAnim.SetBool("Death", true);

        // Stop the player and set active to false
        rb.velocity = new Vector2(0, 0);
        isActive = false;

        // Disable object after
        yield return new WaitForSeconds(respawnTime);

        // Destroy the weapon
        if (playerArm.transform.GetChild(0).childCount > 0)
        {
            GameObject first = playerArm.transform.GetChild(0).transform.GetChild(0).gameObject;
            Destroy(first.gameObject);
        }

        gameObject.SetActive(false);
    }
    #endregion

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // When players triggers each others
        if (collision.gameObject.CompareTag("Player_1") || collision.gameObject.CompareTag("Player_2"))
            Debug.Log("Trigger Player");

        // When player trigger an enemy
        if (collision.gameObject.CompareTag("Enemy"))
            life--;
    }
}
