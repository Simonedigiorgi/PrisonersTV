﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Character
{
    public class _CharacterController : MonoBehaviour
    {

        [BoxGroup("Animator")] public Animator playerAnim;                                          // Get the Player Animators
        [BoxGroup("Components")] public Weapon3D baseWeapon;
        [BoxGroup("Components")] public GameObject playerRightArm;                                  // Player's arm
        [BoxGroup("Components")] public GameObject playerLeftArm;                                   // Player's arm
        [BoxGroup("Components")] public Transform TargetForEnemies;                                 // target for enemy sight and attacks
        [BoxGroup("Components")] public GameObject groundCheck;                                     // Player ground collider
        [BoxGroup("Components")] public Collider2D playerCollider;
        [BoxGroup("Components")] public CharacterStats m_CharStats;
        [BoxGroup("Components")] public CharacterControlMapping inputMapping;
        [BoxGroup("Rules")] public float respawnTime;
        //---------------------------------------------------------------------------------------
        [HideInInspector] public Rigidbody2D rb;                                                    // Rigidbody component
        [HideInInspector] public Weapon3D currentWeapon;
        [HideInInspector] public Transform playerT;

        [HideInInspector] public bool facingRight;                                                  // Player flip facing
        [HideInInspector] public bool isInDash;                                                     // Check if the player is in dash
        [HideInInspector] public bool isGrounded;                                                   // Is the Player on ground?   
        [HideInInspector] public bool isAlive;                                                      // Is player still Alive?
        [HideInInspector] public bool canRespawn;                                                   // Indicates if the players can respawn
        [HideInInspector] public bool startDeathCR;                                                 // If true can start the death coroutine

        [HideInInspector] public int currentLife;                                                   // player's life points at current moment
        [HideInInspector] public int playerNumber;                                                  // player identification number and index in the playerInfo list
        [HideInInspector] public int extraJumps;                                                    // How many double jumps can he make

        [HideInInspector] public bool canHeal = true;                                               // If true the player can interact with the healing station
        [HideInInspector] public bool hasKey = false;                                               // true if the player is holding the key
        [HideInInspector] public float animSpeed;

        [HideInInspector] public bool canExit = false;                                              // if true ends the level
        [HideInInspector] public bool canGetReward = false;                                         // true if the player already choose the reward

        [HideInInspector] public float moveInput;                                                   // records the movement magnitude;
        [HideInInspector] public float currentDashTimer;
        [HideInInspector] public float tensionUpTimer;                                              // when it reaches 0 will add movement points to tension
        [HideInInspector] public float tensionDownTimer;                                            // when it reaches 0 will sub movement points to tension

        // aim variables
        Vector3 aimPosition;
        float aimAngle;         // record the angle of the input
        float aimAngleABS;      // record the absolute value of the angle
        float sign;             // record the sign of the angle
        float rotations;        // record the angle of rotations

        //---------------------------------------------------------------------------------------
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerT = transform;
            currentLife = m_CharStats.life;
            isAlive = true;
            animSpeed = playerAnim.speed;
            tensionUpTimer = GMController.instance.tensionStats.movementTimer;
            tensionDownTimer = GMController.instance.tensionStats.standStillTimer;
            currentDashTimer = m_CharStats.dashTimer;
        }

        private void Update()
        {
            if (GMController.instance.gameStart)
            {
                if (startDeathCR)
                {
                    StartCoroutine(Death());
                }
            }
        }
        //---------------------------------------------------------------------------------------
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isAlive)
            {          
                if (collision.CompareTag("EnergyDispenser") && canHeal && Input.GetButtonDown(inputMapping.interactInput))
                {
                    collision.GetComponent<HealStation>().UseStation(GetComponent<_CharacterController>());
                }
                if (collision.CompareTag("Weapon") && Input.GetButtonDown(inputMapping.interactInput))
                {
                    Weapon3D weapon = collision.GetComponent<Weapon3D>();
                    if (!weapon.isGrabbed)
                    {
                        weapon.hand = playerRightArm.transform.GetChild(0).gameObject;

                        weapon.weaponOwnership = playerNumber;
                        weapon.GrabAndDestroy(this);
                        GMController.instance.TensionThresholdCheck(GMController.instance.tensionStats.actionsPoints); // add tension points for action
                    }
                }
                if (collision.CompareTag("Key") && Input.GetButtonDown(inputMapping.interactInput))
                {
                    Weapon3D key = collision.GetComponent<Weapon3D>();
                    if (!key.isGrabbed)
                    {
                        key.hand = playerRightArm.transform.GetChild(0).gameObject;

                        key.weaponOwnership = playerNumber;
                        key.GrabAndDestroy(this);
                        hasKey = true;
                    }
                }
                if (collision.CompareTag("Exit") && hasKey)
                {
                    canExit = true;
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Exit"))
            {
                canExit = false;
            }
        }
        //---------------------------------------------------------------------------------------
        #region METHODS      

        public void PlayerRespawn(Transform spawnPoint)
        {
            if (Input.GetButtonDown(inputMapping.respawnInput) && canRespawn)
            {
                EnableBaseWeapon();  
                playerT.position = spawnPoint.position;
                isAlive = true;
                currentLife = m_CharStats.life;
                GMController.instance.UI.UpdateLifeUI(playerNumber); // update life on UI
                GMController.instance.UI.SetContinueText(playerNumber); // set continue text if needed
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

            DestroyCurrentWeapon();
            GMController.instance.LowerTensionCheck(GMController.instance.tensionStats.playerDeathPoints);// sub tension
            //reset score
            GMController.instance.playerInfo[playerNumber].Score = 0; // reset score
            GMController.instance.UI.UpdateScoreUI(playerNumber); // update score on UI
            rb.isKinematic = false;
            canRespawn = true;
        }
        public void DestroyCurrentWeapon()
        {
            // Destroy the weapon
            if (playerRightArm.transform.GetChild(0).childCount > 0)
            {
                GameObject first = playerRightArm.transform.GetChild(0).transform.GetChild(0).gameObject;
                if (first.CompareTag("Key"))
                {
                    hasKey = false;
                    GMController.instance.canSpawnKey = true;
                    GMController.instance.SlowdownSpawns();
                }

                if (first == baseWeapon.gameObject)  
                {
                    //hide the base weapon instead of destroying it
                    baseWeapon.transform.parent = null;
                    baseWeapon.mesh.enabled = false;
                }
                else
                {
                    Destroy(first); 
                    currentWeapon = null; 
                }
            }
        }
        public void EnableBaseWeapon()
        {
            if(currentWeapon!= null)
                Destroy(currentWeapon.gameObject);
            baseWeapon.transform.parent = playerRightArm.transform.GetChild(0).transform;
            baseWeapon.transform.position = baseWeapon.transform.parent.position;
            baseWeapon.mesh.enabled = true;
        }
        public void SetupBaseWeapon()
        {
            baseWeapon.weaponOwnership = playerNumber;// give the right ownership to the weapon
            baseWeapon.bullet.membership = playerNumber;// give the right ownership to the bullet
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
        public void JoyRotation(string h, string v)
        {
            aimPosition = new Vector3(Input.GetAxis(h), Input.GetAxis(v), 0);

            aimAngle = Mathf.Atan2(aimPosition.y, aimPosition.x) * Mathf.Rad2Deg;

            playerAnim.SetFloat("Arm", aimAngle);

            if (aimAngle == 0 && facingRight)
                aimAngle = 180;

            playerRightArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle));
        }
        // 8-direction aim with controller
        public void JoyRotationEightDir(string h, string v)
        {
            aimPosition = new Vector3(Input.GetAxisRaw(h), Input.GetAxisRaw(v), 0);

            aimAngle = Mathf.Atan2(aimPosition.y, aimPosition.x) * Mathf.Rad2Deg;

            rotations = 360 / 8;

            if (aimAngle > 0) sign = 1; else sign = -1;
            aimAngleABS = Mathf.Abs(aimAngle);

            for (int i = 0; i <= 4; i++) 
            {
                if (aimAngleABS >= (rotations * i) && aimAngleABS <= ((rotations * i) + (rotations / 2)))  
                {
                    aimAngle = (rotations * i) * sign;
                    break;
                }
                else if(aimAngleABS < (rotations * (i+1)))
                { 
                    aimAngle = (rotations * (i+1))* sign; 
                    break;
                }
            }
            // avoid strange animation blend
            if (aimAngle == -180)
                aimAngle = 180;
            // blend animation
            playerAnim.SetFloat("Arm", aimAngle);

            // makes the arm face the players forward
            if (aimAngle == 0 && facingRight)
                aimAngle = 180;

            playerRightArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle));
        }
        // free aim with mouse
        public void MouseRotation(string h, string v)
        {
            aimPosition = Input.mousePosition - GMController.instance.m_MainCamera.WorldToScreenPoint(playerT.position);

            aimAngle = Mathf.Atan2(aimPosition.y, aimPosition.x) * Mathf.Rad2Deg;

            playerAnim.SetFloat("Arm", aimAngle);
            if (aimAngle == 0 && facingRight)
                aimAngle = 180;

            playerRightArm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle));
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
        public void CallShoot(GameObject arm, Weapon3D weapon)
        {
            // decide if it has to shoot from base weapon or not
            weapon.Shoot(arm.transform.GetChild(1).gameObject);

            //CameraShake shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
            //shake.ShakeCamera(1.2f, .2f);
        }
        public void WeaponControl(GameObject arm, bool hasWeapon)
        {
            // Set arm layer active
            playerAnim.SetLayerWeight(1, 1);

            if (GMController.instance.PlayersInputConfig[playerNumber].LastUsed == TYPEOFINPUT.J)
            {
                // Enable The rotation of joystick
                if (inputMapping.moveArmWithRightStick)
                {
                    JoyRotationEightDir(inputMapping.RightHorizontal, inputMapping.RightVertical);
                }
                else if (!inputMapping.moveArmWithRightStick)
                {
                    JoyRotationEightDir(inputMapping.LeftHorizontal, inputMapping.LeftVertical);
                }
            }
            else
            {
                // Enable The rotation of joystick
                if (inputMapping.moveArmWithRightStick)
                {
                    MouseRotation(inputMapping.RightHorizontal, inputMapping.RightVertical);
                }
                else if (!inputMapping.moveArmWithRightStick)
                {
                    JoyRotationEightDir(inputMapping.LeftHorizontal, inputMapping.LeftVertical);
                }               
            }

            if (hasWeapon)
            {
                // Shoot condition equipped weapon
                if (!currentWeapon.autoFire && Input.GetButtonDown(inputMapping.shootInput))
                {
                    CallShoot(arm, currentWeapon);
                }
                else if (currentWeapon.autoFire && Input.GetButton(inputMapping.shootInput))
                {
                    CallShoot(arm, currentWeapon);
                }

                // Flip the weapon when equipped
                if (facingRight && currentWeapon.isGrabbed)
                    currentWeapon.transform.localEulerAngles = new Vector3(180, 0, 0);
                else if (facingRight == false && currentWeapon.isGrabbed)
                    currentWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                // Shoot condition base weapon
                if (baseWeapon.autoFire && Input.GetButton(inputMapping.shootInput))
                {
                    CallShoot(arm, baseWeapon);
                }
                else if (!baseWeapon.autoFire && Input.GetButtonDown(inputMapping.shootInput))
                {
                    CallShoot(arm, baseWeapon);
                }

                // Flip the weapon when equipped
                if (facingRight)
                    baseWeapon.transform.localEulerAngles = new Vector3(180, 0, 0);
                else if (facingRight == false)
                    baseWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            return; 
        }

        #endregion
        //---------------------------------------------------------------------------------------
    }
}
