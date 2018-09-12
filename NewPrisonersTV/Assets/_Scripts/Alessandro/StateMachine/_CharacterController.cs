using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Character
{
    public class _CharacterController : MonoBehaviour
    {
        // *** ALWAYS REMEMBER TO DON'T RENAME THE PLAYER COMPONENT (Hand_Player1 || Hand_Player2)
        // *** THE WEAPON SCRIPT WORKS WITH THAT

        [BoxGroup("Animator")] public Animator playerAnim;                                     // Get the Player Animators

        [BoxGroup("Components")] public GameObject playerArm;                                       // Player's arm
        [BoxGroup("Components")] public Rigidbody2D rb;                                            // Rigidbody component
        [BoxGroup("Components")] public Weapon3D currentWeapon;
        [BoxGroup("Components")] public GameObject groundCheck;                                         // Player ground collider
        [BoxGroup("Components")] public CharacterStats m_CharStats;

        [BoxGroup("Player Inputs")] public HORIZONTAL LeftHorizontal;                                           // Get Horiziontal
        [BoxGroup("Player Inputs")] public VERTICAL LeftVertical;                                             // Get Vertical
        [BoxGroup("Player Inputs")] public HORIZONTAL RightHorizontal;                                           // Get Horiziontal
        [BoxGroup("Player Inputs")] public VERTICAL RightVertical;                                             // Get Vertical
        [BoxGroup("Player Inputs")] public BUTTONS shootInput;                         // Player1_Button X || Player2_Button X (Shoot with you weapon)
        [BoxGroup("Player Inputs")] public BUTTONS jumpInput;                          // Player1_Button A || Player2_Button A
        [BoxGroup("Player Inputs")] public bool moveArmWithRight;

        [HideInInspector] public bool facingRight;                                                      // Player flip facing
        [HideInInspector] public bool isInDash;                                                         // Check if the player is in dash
        [HideInInspector] public bool isActive;                                                         // Check if the player is active
        [HideInInspector] public bool isGrounded;                                                                        // Is the Player on ground?   
        [HideInInspector] public int extraJumps;

        void Start()
        {

            rb = GetComponent<Rigidbody2D>();

        }

        public void OnEnable()
        {    
            isActive = true;
        }

        #region Methods

        public void armRotation(HORIZONTAL h, VERTICAL v)
        {
            // Get the arm component on weapon
            Transform armObject = playerArm.transform.GetChild(0).GetChild(0).GetChild(2);

            Vector3 position = new Vector3(Input.GetAxis(h.ToString()), Input.GetAxis(v.ToString()), 0);           

            float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;

            if (angle == 0 && facingRight)
                angle = 180;

            armObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // This avoid the arm having a glitchy position
            //if (facingRight)
            //    armObject.GetComponent<SpriteRenderer>().flipY = true;
            //else
            //    armObject.GetComponent<SpriteRenderer>().flipY = false;
        }

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
        public void JoyRotation(HORIZONTAL h, VERTICAL v)
        {
            Vector3 joyPosition = new Vector3(Input.GetAxis(h.ToString()), Input.GetAxis(v.ToString()), 0);

            float angle = Mathf.Atan2(joyPosition.y, joyPosition.x) * Mathf.Rad2Deg;

            if (angle == 0 && facingRight)
                angle = 180;

            playerArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        
        #endregion
    }
}
