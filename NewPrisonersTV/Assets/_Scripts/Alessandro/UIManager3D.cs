using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Character;
using UnityEngine.EventSystems;

public class TensionBarElement
{
    public Transform transform;
    public Image image;
    public bool[] isActive; //records if is active for each multiplier level of the bar 

    public TensionBarElement(GameObject element)
    {
        transform = element.transform;
        image = element.GetComponent<Image>();
        isActive = new bool[GMController.instance.tensionStats.maxBarMulti];
    }
}
public class UIManager3D : MonoBehaviour
{
    public Transform tensionBarUI;         // reference to all the tension bar UI component
    public Transform tensionThresholdsUI; // reference to threshold UI element
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
    TensionBarElement[] ThresholdUIList;
    SpriteRenderer[] lifeBar;                                                                       //the lifeBar on player
    
    Camera mainCamera;
  
    void Start ()
    {
        mainCamera = Camera.main;

        if (GMController.instance.CurrentMode != GAMEMODE.Menu)
        {
            tensionBarUI.gameObject.SetActive(true);
            ThresholdUIList = new TensionBarElement[GMController.instance.tensionStats.barDivision];
            //actualWeapon = new Weapon3D[GMController.instance.GetPlayerNum()];
            playerHand = new GameObject[GMController.instance.PlayersRequired]; 
            hammo = new Text[GMController.instance.PlayersRequired];
            score = new Text[GMController.instance.PlayersRequired];
            playerContinue = new Text[GMController.instance.PlayersRequired];
            lifeBar = new SpriteRenderer[GMController.instance.PlayersRequired];          
            // set the tension bar
            tensionMultiUI = tensionBarUI.GetChild(1).GetChild(0).GetComponent<Text>();
            tensionLevelUI = tensionBarUI.GetChild(0).GetComponent<RectTransform>();
            tensionLevelLenght = tensionLevelUI.rect.width;  // record the initial bar lenght
            tensionLevelUI.sizeDelta = new Vector2(0, tensionLevelUI.rect.height);// resize the bar to look empty
            SetBarThresholds(); // create and set to position the thresholds UI
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
        if (GMController.instance.CurrentMode != GAMEMODE.Menu)
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
            if (GMController.instance.CurrentMode == GAMEMODE.Story)
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

    public void ChangeThresholdUIColor(int index)
    {
        // when the threshold is reached for the first time sets the color to red and set it active
        ThresholdUIList[index].image.color = Color.red;
        ThresholdUIList[index].isActive[GMController.instance.currentTensionMulti - 1] = true;
    }
    public void ChangeAllThresholUIColor()
    {
        // for all the UI elements when going back to a previous multiplier level sets the color to red
        for (int i = 0; i < ThresholdUIList.Length; i++)
        {
            ThresholdUIList[i].image.color = Color.red;
        }
    }
    public void ResetThresholdUIColor()
    {
        // for all the UI elements if they are not active in the current bar multiplier then sets the color to white
        for (int i = 0; i < ThresholdUIList.Length; i++)
        {
            if (!ThresholdUIList[i].isActive[GMController.instance.currentTensionMulti - 1])
            {
                ThresholdUIList[i].image.color = Color.white;
            }
        }      
    }

    private void SetBarThresholds()
    {
        // create and set the threshold UI components in the right position
        float tensionThreshold = tensionLevelLenght / GMController.instance.tensionStats.barDivision;
        for (int i = 0; i < GMController.instance.tensionStats.barDivision; i++)
        {
            ThresholdUIList[i] = new TensionBarElement(Instantiate(tensionThresholdsUI.gameObject, tensionBarUI));
            float currentThresholdPosition = tensionThreshold * (i+1); 
            ThresholdUIList[i].transform.localPosition = new Vector2(tensionBarUI.GetChild(0).transform.localPosition.x + currentThresholdPosition, 0);
            ThresholdUIList[i].transform.gameObject.SetActive(true);  
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
       if (GMController.instance.LevelCount+1 < GMController.instance.TotalScenes)
       {
            GMController.instance.RewardOrder();
            rewardPanel.SetActive(true);
            resultsPanel.SetActive(false);
            GMController.instance.bonusWeapon.RewardPool();
       }
       else
            GMController.instance.readyForNextLevel = true;

        yield return null; 
    }
    public void RewardSelection() // move to gm
    {     
        for (int i = 0; i < GMController.instance.bonusWeapon.rewardButtons.Length; i++)
        {
            if (GMController.instance.bonusWeapon.rewardButtons[i].RewardButton != null && GMController.instance.bonusWeapon.rewardButtons[i].RewardButton == GMController.instance.eventSystem.currentSelectedGameObject)
            {
                GMController.instance.AddWeaponReward(GMController.instance.lastPlayerThatChooseReward, GMController.instance.bonusWeapon.bonusPool[GMController.instance.bonusWeapon.rewardButtons[i].PoolIndex]);
                GMController.instance.bonusWeapon.rewardButtons[i].ButtonT.parent = null;
                Destroy(GMController.instance.bonusWeapon.rewardButtons[i].RewardButton);
                GMController.instance.eventSystem.SetSelectedGameObject(GMController.instance.bonusWeapon.panel.GetChild(0).gameObject, new BaseEventData(GMController.instance.eventSystem));

                break;
            }
        }    
    }
   
}
