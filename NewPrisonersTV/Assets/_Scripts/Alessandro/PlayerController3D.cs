using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum HORIZONTAL {Player1_Horizontal, Player2_Horizontal };
public enum VERTICAL { Player1_Vertical, Player2_Vertical };
public enum BUTTONS { Player1_Button_A, Player1_Button_B,Player1_Button_X,Player1_Button_Y,
                      Player2_Button_A, Player2_Button_B, Player2_Button_X, Player2_Button_Y };

public class PlayerController3D : MonoBehaviour {

    // *** ALWAYS REMEMBER TO DON'T RENAME THE PLAYER COMPONENT (Hand_Player1 || Hand_Player2)
    // *** THE WEAPON SCRIPT WORKS WITH THAT

    private Rigidbody2D rb;                                                                         // Rigidbody component
    private Weapon weapon;                                                                          // Weapon component

    [BoxGroup("Animator")] public Animator playerAnim;                                     // Get the Player Animators

    [BoxGroup("Components")] public GameObject playerArm;                                           // Player's arm

    [BoxGroup("Player Inputs")] public HORIZONTAL Horizontal;                                           // Get Horiziontal
    [BoxGroup("Player Inputs")] public VERTICAL Vertical;                                             // Get Vertical

    [BoxGroup("Controls")] public float speed;                                                      // Player speed

    [HideInInspector] public bool facingRight;                                                      // Player flip facing
    [HideInInspector] public bool isInDash;                                                         // Check if the player is in dash
    [HideInInspector] public bool isActive;                                                         // Check if the player is active

    private float joypadDeathZone = 0.2f;                                                           // Movement death zone

    void Start () {

        rb = GetComponent<Rigidbody2D>();

    }

    public void OnEnable()
    {
       // playerAnim.Play("Idle");      

        isActive = true;
    }

    private void FixedUpdate()
    {
        // *** LEAVE ALL OF THIS ON FIXED UPDATE TO AVOID PROBLEM OCCURING DURING THE FLIP

        // Move inputs
        float moveInput = Input.GetAxis(Horizontal.ToString());

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
            //playerAnim.SetFloat("Speed", Mathf.Abs(moveInput));

            // Flip the player direction
            if (!facingRight && moveInput < 0)
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
            rotation.y = 180;                                                                       // Rotate the player
        }

        else
        {
            rotation.y = 0;                                                                         // Rotate the player
        }
            
        transform.localEulerAngles = rotation;
    }

    // Rotate the Joystick of 360°
    public void JoyRotation()
    {
        Vector3 joyPosition = new Vector3(Input.GetAxis(Horizontal.ToString()), Input.GetAxis(Vertical.ToString()), 0);

        float angle = Mathf.Atan2(joyPosition.y, joyPosition.x) * Mathf.Rad2Deg;

        if (angle == 0 && facingRight)
            angle = 180;

        playerArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    #endregion
}
