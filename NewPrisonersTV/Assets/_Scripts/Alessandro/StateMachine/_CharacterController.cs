﻿using System.Collections;
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

        [BoxGroup("Components")] public GameObject playerRightArm;                                       // Player's arm
        [BoxGroup("Components")] public GameObject playerLeftArm;                                       // Player's arm
        [BoxGroup("Components")] public Weapon3D currentWeapon;
        [BoxGroup("Components")] public GameObject groundCheck;                                         // Player ground collider
        [BoxGroup("Components")] public Collider2D playerCollider;
        [BoxGroup("Components")] public CharacterStats m_CharStats;
        [BoxGroup("Components")] public CharacterControlConfig m_ControlConfig;     

        [BoxGroup("Rules")] public float respawnTime;
        
        [HideInInspector] public Rigidbody2D rb;                                            // Rigidbody component

        [HideInInspector] public bool facingRight;                                                      // Player flip facing
        [HideInInspector] public bool isInDash;                                                         // Check if the player is in dash
        [HideInInspector] public bool isGrounded;                                                       // Is the Player on ground?   
        [HideInInspector] public bool isAlive ;                                                          // Is player still Alive?
        [HideInInspector] public bool canRespawn;
        [HideInInspector] public bool startDeathCR;

        [HideInInspector] public int currentLife;
        [HideInInspector] public int playerNumber;                                                      // player identification number and index in the playerInfo list
        [HideInInspector] public int extraJumps;                                                        // How many double jumps can he make

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            currentLife = m_CharStats.life;
            isAlive = true;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            // When player trigger an enemy
            if (collision.gameObject.CompareTag("Enemy"))
                currentLife--;
        }

        #region Methods

        public void PlayerRespawn(Transform spawnPoint)
        {
            if (Input.GetButtonDown(m_ControlConfig.respawnInput.ToString()) && canRespawn)
            {
                transform.position = spawnPoint.position;
                isAlive = true;
                currentLife = m_CharStats.life;
                canRespawn = false;
            }
        }

        public IEnumerator Death()
        {
            startDeathCR = false;
            // Stop the player and set active to false            
            rb.velocity = new Vector2(0, 0);           

            // Disable object after
            yield return new WaitForSeconds(respawnTime);

            // Set the player on the center of the screen (this fix the CameraView when a Player die)
           // transform.position = new Vector2(0, 0);

            // Destroy the weapon
            if (playerRightArm.transform.GetChild(0).childCount > 0)
            {
                GameObject first = playerRightArm.transform.GetChild(0).transform.GetChild(0).gameObject;
                Destroy(first.gameObject);
            }

            rb.isKinematic = false;
            canRespawn = true;
        }

        public void armRotation(HORIZONTAL h, VERTICAL v, GameObject arm)
        {
            Vector3 position = new Vector3(Input.GetAxis(h.ToString()), Input.GetAxis(v.ToString()), 0);           

            float angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;

            if (angle == 0 && facingRight)
                angle = 180;

            // Get the arm component on weapon
            Transform armObject;
            if (arm.transform.GetChild(0).GetChild(0).GetChild(2) != null)
            {
                armObject = arm.transform.GetChild(0).GetChild(0).GetChild(2);
                armObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
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
            playerAnim.SetFloat("Arm", angle);
            if (angle == 0 && facingRight)
                angle = 180;
            playerRightArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public void SwapArm(bool direction)
        {
            if(playerRightArm.transform.GetChild(0).childCount == 1 && direction == !facingRight)
            {
                playerRightArm.transform.GetChild(0).GetChild(0).transform.SetParent(playerLeftArm.transform.GetChild(0).transform);
                playerLeftArm.transform.GetChild(0).GetChild(0).transform.position = playerLeftArm.transform.GetChild(0).transform.position;
            }
            else if(playerLeftArm.transform.GetChild(0).childCount == 1 && direction == facingRight)
            {
                playerLeftArm.transform.GetChild(0).GetChild(0).transform.SetParent(playerRightArm.transform.GetChild(0).transform);
                playerRightArm.transform.GetChild(0).GetChild(0).transform.position = playerRightArm.transform.GetChild(0).transform.position;
            }
        }

        public void WeaponControl(GameObject arm)
        {
            // Set arm layer active
            playerAnim.SetLayerWeight(1, 1);
            // Enable The rotation of joystick
            if (m_ControlConfig.moveArmWithRightStick)
            {
                JoyRotation(m_ControlConfig.RightHorizontal, m_ControlConfig.RightVertical);
            }
            else if (!m_ControlConfig.moveArmWithRightStick)
            {
                JoyRotation(m_ControlConfig.LeftHorizontal, m_ControlConfig.LeftVertical);
            }

            // Shoot condition
            if (currentWeapon.autoFire == false)
            {
                if (Input.GetButtonDown(m_ControlConfig.shootInput.ToString()) && currentWeapon.isGrabbed)
                {
                    currentWeapon.Shoot(arm.transform.GetChild(1).gameObject);

                    CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
                    //shake.ShakeCamera(1.2f, .2f);
                }
            }
            if (currentWeapon.autoFire)
            {
                if (Input.GetButton(m_ControlConfig.shootInput.ToString()) && currentWeapon.isGrabbed)
                {
                    currentWeapon.Shoot(arm.transform.GetChild(1).gameObject);

                    CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
                    shake.ShakeCamera(1.2f, .2f);
                }
            }

            // Flip the weapon when equipped
            if (facingRight && currentWeapon.isGrabbed)
                currentWeapon.transform.localEulerAngles = new Vector3(180, 0, 0);
            else if (facingRight == false && currentWeapon.isGrabbed)
                currentWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);

            // Rotation of Muzz effect
            if (m_ControlConfig.moveArmWithRightStick)
            {
                armRotation(m_ControlConfig.RightHorizontal, m_ControlConfig.RightVertical, arm);
            }
            else if (!m_ControlConfig.moveArmWithRightStick)
            {
                armRotation(m_ControlConfig.LeftHorizontal, m_ControlConfig.LeftVertical, arm);
            }
            return;
        }

        #endregion

        private void Update()
        {
            if(startDeathCR)
            {
                StartCoroutine(Death());
            }
        }
    }
}
