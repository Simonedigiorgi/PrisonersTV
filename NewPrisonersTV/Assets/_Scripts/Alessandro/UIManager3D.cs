using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;

public class UIManager3D : MonoBehaviour {

    public GameObject[] playerUI;

    Weapon3D[] actualWeapon;                                                                          //the weapon grabbed
    GameObject[] playerHand;                                                                              //Hand player
    Text[] hammo;                                                                                   //UI player hammo text
    Text[] score;
    Text[] playerContinue;
    SpriteRenderer[] lifeBar;                                                                       //the lifeBar on player

    Camera mainCamera;

    GMController gameManager;

    [Tooltip("The horizontal distance of the UI hammo text to the player")] public float hammoHorizontalOffset;
    [Tooltip("The vertical distance of the UI hammo text to the player")] public float hammoVerticalOffset;


    void Start ()
    {
        //Find game manager
        if (GameObject.Find("GameManager") != null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GMController>();
        }
        else
        {
            Debug.Log("ADD GAME MANAGER TO SCENE!!! (named 'GameManager')");
        }

        mainCamera = Camera.main;

        if (gameManager.currentMode != GAMEMODE.None)
        {
            actualWeapon = new Weapon3D[gameManager.playerRequired];
            playerHand = new GameObject[gameManager.playerRequired];
            hammo = new Text[gameManager.playerRequired];
            score = new Text[gameManager.playerRequired];
            lifeBar = new SpriteRenderer[gameManager.playerRequired];

            //Get the components for all the players
            for (int i = 0; i < gameManager.playerInfo.Length; i++)
            {
                playerHand[i] = gameManager.playerInfo[i].playerController.playerArm.transform.GetChild(0).gameObject;
                lifeBar[i] = gameManager.playerInfo[i].player.transform.GetChild(4).GetComponent<SpriteRenderer>();
                hammo[i] = playerUI[i].transform.GetChild(1).GetComponent<Text>();
                score[i] = playerUI[i].transform.GetChild(2).GetComponent<Text>();
                playerContinue[i] = playerUI[i].transform.GetChild(0).GetComponent<Text>();
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManager.currentMode != GAMEMODE.None)
        {
            for (int i = 0; i < gameManager.playerInfo.Length; i++)
            {
                #region ContinueText
                //Enabled and disabled Continue text
                if (!gameManager.playerInfo[i].playerController.isAlive)
                {
                    playerContinue[i].enabled = true;

                }
                else
                {
                    playerContinue[i].enabled = false;
                }
                #endregion

                #region Bullets
                //Switch weapon
                //if (gameManager.playerInfo[i].playerController.currentWeapon == null && playerHand[i].transform.childCount > 0)
                //{
                //    actualWeapon[i] = playerHand[i].transform.GetChild(0).GetComponent<Weapon3D>();
                //}


                //Enabled and disabled Hammo text and assign hammo value at the text
                if (playerHand[i].transform.childCount <= 0)
                {
                    hammo[i].text = 0.ToString();
                    hammo[i].gameObject.SetActive(false);
                }
                else
                {
                    hammo[i].gameObject.SetActive(true);
                    hammo[i].text = gameManager.playerInfo[i].playerController.currentWeapon.bullets.ToString();
                }
                //set hammo text position
                hammo[i].transform.position = mainCamera.WorldToScreenPoint(gameManager.playerInfo[i].player.transform.position);
                if (gameManager.playerInfo[i].playerController.facingRight)
                    hammo[i].transform.position += new Vector3(hammoHorizontalOffset, hammoVerticalOffset, 0);
                else
                    hammo[i].transform.position += new Vector3(-hammoHorizontalOffset, hammoVerticalOffset, 0);

                #endregion

                #region Life

                //Rescale and Recolor life bar
                if (gameManager.playerInfo[i].playerController.currentLife == 3)
                {
                    lifeBar[i].transform.localScale = new Vector3(15, 2.5f, 0);
                    lifeBar[i].color = Color.green;
                }
                else if (gameManager.playerInfo[i].playerController.currentLife == 2)
                {
                    lifeBar[i].transform.localScale = new Vector3(10, 2.5f, 0);
                    lifeBar[i].color = Color.yellow;
                }
                else if (gameManager.playerInfo[i].playerController.currentLife == 1)
                {
                    lifeBar[i].transform.localScale = new Vector3(5, 2.5f, 0);
                    lifeBar[i].color = Color.red;
                }
                else if (gameManager.playerInfo[i].playerController.currentLife <= 0)
                {
                    lifeBar[i].transform.localScale = Vector3.zero;
                }

                #endregion

                #region Score

                score[i].text = "P"+i+ " Score: " + gameManager.playerInfo[i].score.ToString();
      
                #endregion
            }
        }
  
    }
}
