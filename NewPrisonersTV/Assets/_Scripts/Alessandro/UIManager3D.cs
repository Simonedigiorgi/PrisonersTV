using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;
using UnityEngine.EventSystems;

public class UIManager3D : MonoBehaviour
{
    public Transform tensionBarUI;
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
    Text tensionMultiUI;
    RectTransform tensionLevelUI;
    float tensionLevelLenght;
    SpriteRenderer[] lifeBar;                                                                       //the lifeBar on player
    StandaloneInputModule inputModule;
    Camera mainCamera;
  
    void Start ()
    {
        mainCamera = Camera.main;

        if (GMController.instance.GetGameMode() != GAMEMODE.Menu)
        {
            tensionBarUI.gameObject.SetActive(true);
            //actualWeapon = new Weapon3D[GMController.instance.GetPlayerNum()];
            playerHand = new GameObject[GMController.instance.GetPlayerNum()];
            hammo = new Text[GMController.instance.GetPlayerNum()];
            score = new Text[GMController.instance.GetPlayerNum()];
            playerContinue = new Text[GMController.instance.GetPlayerNum()];
            lifeBar = new SpriteRenderer[GMController.instance.GetPlayerNum()];
            inputModule = eventSystem.GetComponent<StandaloneInputModule>();
            // set the tension bar
            tensionMultiUI = tensionBarUI.GetChild(1).GetChild(0).GetComponent<Text>();
            tensionLevelUI = tensionBarUI.GetChild(0).GetComponent<RectTransform>();
            tensionLevelLenght = tensionLevelUI.rect.width;
            tensionLevelUI.sizeDelta = new Vector2(0, tensionLevelUI.rect.height);
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
                    if (GMController.instance.playerInfo[i].playerController.currentWeapon != null)
                        hammo[i].text = GMController.instance.playerInfo[i].playerController.currentWeapon.bullets.ToString();
                    else
                        hammo[i].text = "∞"; 
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
        // percentage of current life 
        float percentageOfLife = GMController.instance.playerInfo[player].playerController.currentLife * 100 / GMController.instance.playerInfo[player].playerController.m_CharStats.life;
        //Recolor life bar
        if (percentageOfLife > 75)
        {
            lifeBar[player].color = Color.green;
        }
        else if (percentageOfLife > 50 && percentageOfLife < 75)
        {
            lifeBar[player].color = Color.yellow;
        }
        else if (percentageOfLife < 50 && percentageOfLife > 25 )
        {
            lifeBar[player].color = Color.magenta;
        }
        else if (percentageOfLife < 25)
        {
            lifeBar[player].color = Color.red;
        }
        // change the lifebar proportions to match the life %
        lifeBar[player].transform.localScale = new Vector3((percentageOfLife / 100 * 15), 2.5f, 0);
    }
    public void UpdateTensionBar()
    {
        float percentageOfTension = GMController.instance.currentTensionLevel * 100 / GMController.instance.currentTensionMax;
        if (tensionLevelUI.rect.width <= tensionLevelLenght) 
            tensionLevelUI.sizeDelta = new Vector2((percentageOfTension / 100 * tensionLevelLenght), tensionLevelUI.rect.height);
        if(tensionLevelUI.rect.width > tensionLevelLenght)
            tensionLevelUI.sizeDelta = new Vector2(tensionLevelLenght, tensionLevelUI.rect.height);
    }
    public void UpdateTensionMulti()
    {
        tensionMultiUI.text = GMController.instance.currentTensionMulti +"x";
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
