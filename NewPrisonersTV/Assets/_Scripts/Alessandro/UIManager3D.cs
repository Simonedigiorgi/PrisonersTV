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

    [Tooltip("The horizontal distance of the UI hammo text to the player")] public float hammoHorizontalOffset;
    [Tooltip("The vertical distance of the UI hammo text to the player")] public float hammoVerticalOffset;


    void Start ()
    {
        mainCamera = Camera.main;

        if (GMController.instance.GetGameMode() != GAMEMODE.Menu)
        {
            actualWeapon = new Weapon3D[GMController.instance.GetPlayerNum()];
            playerHand = new GameObject[GMController.instance.GetPlayerNum()];
            hammo = new Text[GMController.instance.GetPlayerNum()];
            score = new Text[GMController.instance.GetPlayerNum()];
            playerContinue = new Text[GMController.instance.GetPlayerNum()];
            lifeBar = new SpriteRenderer[GMController.instance.GetPlayerNum()];
            //Get the components for all the players
            for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
            {
                playerHand[i] = GMController.instance.playerInfo[i].playerController.playerRightArm.transform.GetChild(0).gameObject;
                lifeBar[i] = GMController.instance.playerInfo[i].player.transform.GetChild(3).GetComponent<SpriteRenderer>();
                playerContinue[i] = playerUI[i].transform.GetChild(0).GetComponent<Text>();
                hammo[i] = playerUI[i].transform.GetChild(1).GetComponent<Text>();
                score[i] = playerUI[i].transform.GetChild(2).GetComponent<Text>();
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GMController.instance.GetGameMode() != GAMEMODE.Menu)
        {
            for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
            {
                #region ContinueText
                //Enabled and disabled Continue text
                if (!GMController.instance.playerInfo[i].playerController.isAlive)
                {
                    playerContinue[i].gameObject.SetActive(true);
                }
                else
                {
                    playerContinue[i].gameObject.SetActive(false);
                }
                #endregion

                #region Bullets

                //Enabled and disabled Hammo text and assign hammo value at the text
                if (playerHand[i].transform.childCount <= 0)
                {
                    hammo[i].text = 0.ToString();
                    hammo[i].gameObject.SetActive(false);
                }
                else if(playerHand[i].transform.childCount > 0)
                {
                    hammo[i].gameObject.SetActive(true);              
                    hammo[i].text = GMController.instance.playerInfo[i].playerController.currentWeapon.bullets.ToString();
                }
                //set hammo text position
                hammo[i].transform.position = mainCamera.WorldToScreenPoint(GMController.instance.playerInfo[i].player.transform.position);
                if (GMController.instance.playerInfo[i].playerController.facingRight)
                    hammo[i].transform.position += new Vector3(hammoHorizontalOffset, hammoVerticalOffset, 0);
                else
                    hammo[i].transform.position += new Vector3(-hammoHorizontalOffset, hammoVerticalOffset, 0);

                #endregion

                #region Life

                //Rescale and Recolor life bar
                if (GMController.instance.playerInfo[i].playerController.currentLife == 3)
                {
                    lifeBar[i].transform.localScale = new Vector3(15, 2.5f, 0);
                    lifeBar[i].color = Color.green;
                }
                else if (GMController.instance.playerInfo[i].playerController.currentLife == 2)
                {
                    lifeBar[i].transform.localScale = new Vector3(10, 2.5f, 0);
                    lifeBar[i].color = Color.yellow;
                }
                else if (GMController.instance.playerInfo[i].playerController.currentLife == 1)
                {
                    lifeBar[i].transform.localScale = new Vector3(5, 2.5f, 0);
                    lifeBar[i].color = Color.red;
                }
                else if (GMController.instance.playerInfo[i].playerController.currentLife <= 0)
                {
                    lifeBar[i].transform.localScale = Vector3.zero;
                }

                #endregion

                #region Score

                score[i].text = "P"+ (i+1) + " Score: " + GMController.instance.playerInfo[i].score.ToString();
      
                #endregion
            }
        }
  
    }
}
