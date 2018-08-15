using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour {

    // *** ALWAYS REMEMBER TO DON'T RENAME THE PLAYER COMPONENT (Hand_Player1 || Hand_Player2)
    // *** THE WEAPON SCRIPT WORKS WITH THAT

    private Rigidbody2D rb;                                                                         // Rigidbody component
    private Weapon weapon;                                                                          // Weapon component

    [BoxGroup("Animator")] public Animator playerAnim, armAnim;                                     // Get the Player Animators

    [BoxGroup("Components")] public GameObject playerArm;                                           // Player's arm

    [BoxGroup("Player Inputs")] public string Horizontal;                                           // Get Horiziontal
    [BoxGroup("Player Inputs")] public string Vertical;                                             // Get Vertical

    [BoxGroup("Controls")] public float speed;                                                      // Player speed

    [HideInInspector] public bool facingRight;                                                      // Player flip facing
    [HideInInspector] public bool isInDash;                                                         // Check if the player is in dash
    [HideInInspector] public bool isActive;                                                         // Check if the player is active

    private float joypadDeathZone = 0.2f;                                                           // Movement death zone

    void Start () {

        rb = GetComponent<Rigidbody2D>();

        // Disable 360° arm sprite
        playerArm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnEnable()
    {
        playerAnim.Play("Idle");
        armAnim.Play("Idle");

        // Set the Arm_sprite sprite to true
        playerArm.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

        // Set the Arm_Anim sprite to true
        playerArm.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;

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
            else if (!isInDash)                                                                         // If is not in Dash
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
    #endregion

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // When players triggers each others
        if (collision.gameObject.CompareTag("Player_1") || collision.gameObject.CompareTag("Player_2"))
            Debug.Log("Trigger Player");
    }
}
