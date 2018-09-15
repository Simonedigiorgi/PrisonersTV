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
        [BoxGroup("Components")] public CharacterControlConfig m_ControlConfig;     

        [BoxGroup("Rules")] public float respawnTime;
        

        [HideInInspector] public bool facingRight;                                                      // Player flip facing
        [HideInInspector] public bool isInDash;                                                         // Check if the player is in dash
        [HideInInspector] public bool isGrounded;                                                       // Is the Player on ground?   
        [HideInInspector] public int extraJumps;
        [HideInInspector] public bool isAlive;
        [HideInInspector] public int currentLife;
        [HideInInspector] public bool canRespawn;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            currentLife = m_CharStats.life;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            // When player trigger an enemy
            if (collision.gameObject.CompareTag("Enemy"))
                currentLife--;
        }

        #region Methods
        public IEnumerator Death()
        {
            // Stop the player and set active to false            
            rb.velocity = new Vector2(0, 0);           

            // Disable object after
            yield return new WaitForSeconds(respawnTime);

            // Set the player on the center of the screen (this fix the CameraView when a Player die)
            transform.position = new Vector2(0, 0);

            // Destroy the weapon
            if (playerArm.transform.GetChild(0).childCount > 0)
            {
                GameObject first = playerArm.transform.GetChild(0).transform.GetChild(0).gameObject;
                Destroy(first.gameObject);
            }

            rb.isKinematic = false;
            canRespawn = true;
        }

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
