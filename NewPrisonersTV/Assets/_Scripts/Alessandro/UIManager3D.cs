using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;
using UnityEngine.EventSystems;

public class UIManager3D : MonoBehaviour
{
    public Transform objective;
    public Text objectiveText;
    [Tooltip("The horizontal distance of the UI hammo text to the player")] public float hammoHorizontalOffset;
    [Tooltip("The vertical distance of the UI hammo text to the player")] public float hammoVerticalOffset; 
    public GameObject[] playerUI;
    public GameObject resultsPanel;
    [Range(1,10)]
    public float resultSpeedMulti;
    public Text[] resultsUI;
    public GameObject rewardPanel;
    public EventSystem eventSystem;

    //Weapon3D[] actualWeapon;                                                                          //the weapon grabbed
    GameObject[] playerHand;                                                                              //Hand player
    Text[] hammo;                                                                                   //UI player hammo text
    Text[] score;
    Text[] playerContinue;
    SpriteRenderer[] lifeBar;                                                                       //the lifeBar on player
    StandaloneInputModule inputModule;
    Camera mainCamera;
     
    void Start ()
    {
        mainCamera = Camera.main;

        if (GMController.instance.GetGameMode() != GAMEMODE.Menu)
        {
            //actualWeapon = new Weapon3D[GMController.instance.GetPlayerNum()];
            playerHand = new GameObject[GMController.instance.GetPlayerNum()];
            hammo = new Text[GMController.instance.GetPlayerNum()];
            score = new Text[GMController.instance.GetPlayerNum()];
            playerContinue = new Text[GMController.instance.GetPlayerNum()];
            lifeBar = new SpriteRenderer[GMController.instance.GetPlayerNum()];
            inputModule = eventSystem.GetComponent<StandaloneInputModule>();

            //Get the components and sets/enables UI for all the players
            for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
            {
                playerHand[i] = GMController.instance.playerInfo[i].playerController.playerRightArm.transform.GetChild(0).gameObject;
                lifeBar[i] = GMController.instance.playerInfo[i].player.transform.GetChild(3).GetComponent<SpriteRenderer>();
                playerContinue[i] = playerUI[i].transform.GetChild(0).GetComponent<Text>();
                hammo[i] = playerUI[i].transform.GetChild(1).GetComponent<Text>();
                score[i] = playerUI[i].transform.GetChild(2).GetComponent<Text>();
                resultsUI[i].gameObject.SetActive(true);
                UpdateScoreUI(i);
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
            }
            if (GMController.instance.GetGameMode() == GAMEMODE.Story)
            {
                if (GMController.instance.gameStart)
                    objectiveText.text = "Remaining Time: " + (int)GMController.instance.currentGameTime;
                if (!GMController.instance.canResultCR)                
                    StartCoroutine(ShowResults()); 
            }
        }
  
    }

    public void UpdateScoreUI(int player)
    {
        score[player].text = "P" + (player + 1) + " Score: " + GMController.instance.playerInfo[player].score.ToString();
    }
    public void UpdateLifeUI(int player)
    {
        //Rescale and Recolor life bar
        if (GMController.instance.playerInfo[player].playerController.currentLife == 3)
        {
            lifeBar[player].transform.localScale = new Vector3(15, 2.5f, 0);
            lifeBar[player].color = Color.green;
        }
        else if (GMController.instance.playerInfo[player].playerController.currentLife == 2)
        {
            lifeBar[player].transform.localScale = new Vector3(10, 2.5f, 0);
            lifeBar[player].color = Color.yellow;
        }
        else if (GMController.instance.playerInfo[player].playerController.currentLife == 1)
        {
            lifeBar[player].transform.localScale = new Vector3(5, 2.5f, 0);
            lifeBar[player].color = Color.red;
        }
        else if (GMController.instance.playerInfo[player].playerController.currentLife <= 0)
        {
            lifeBar[player].transform.localScale = Vector3.zero;
        }
    }
    public void SetContinueText(int player)
    {
        if (!GMController.instance.playerInfo[player].playerController.isAlive)
        {
            playerContinue[player].gameObject.SetActive(true);
        }
        else
        {
            playerContinue[player].gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowResults()
    {
       GMController.instance.canResultCR = true;    
       resultsPanel.SetActive(true);
       for (int i = 0; i < GMController.instance.playerInfo.Length; i++)
       {
          float scoreTemp = 0;
          while (scoreTemp < GMController.instance.playerInfo[i].score)
          {
                yield return null;
                scoreTemp += Time.unscaledDeltaTime * resultSpeedMulti;         
                resultsUI[i].text = "P" + (i + 1) + " Score: " + (int)scoreTemp; 
                if(Input.anyKeyDown)
                {
                    break;
                }
          }

          GMController.instance.AddPlayersTotalScore(i, GMController.instance.playerInfo[i].score);         
          resultsUI[i].text = "P" + (i + 1) + " Score: " + GMController.instance.playerInfo[i].score + "\n" 
                            + "Total Score: " + GMController.instance.GetPlayerTotalScore(i);           
       }
       while(!Input.anyKeyDown)
       {
            //wait for input
            yield return null;
       }
       // preparation for the rewards, set the player order, activates the reward panel and generate the reward pool
        GMController.instance.RewardOrder(); 
        rewardPanel.SetActive(true);
        resultsPanel.SetActive(false);
        GMController.instance.bonusWeapon.RewardPool();
       
        yield return null; 
    }

    public void ChangeInputModule(CharacterControlConfig player)
    {      
        inputModule.horizontalAxis = player.LeftHorizontal.ToString();
        inputModule.verticalAxis = player.LeftVertical.ToString();
        inputModule.submitButton = player.interactInput.ToString();
        inputModule.cancelButton = player.shootInput.ToString();
    }
}
