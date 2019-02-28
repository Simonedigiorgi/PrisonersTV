using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class InterfaceButtonDescription
{
    public Text buttonFunctionality;
    public Image buttonIcon;
}

public class AssignPlayerController : MonoBehaviour
{
    public GameObject[] playerButtons;
    public Text[] playerControllerText; 
    public GameObject playerModePanel;
    public GameObject modalityPanel;
    public ChooseModality[] modality;
    public Text config;
    public InterfaceButtonDescription[] configButtons;

    [HideInInspector] public int playerNumber;

    private int keyboardUser = 0;
    private int assignmentTurn = 0; // is equal to the index of the player that has to choose
    private bool isWaiting = false;
    private int configIndex = 0;

    private void Update()
    { 
        if (isWaiting)
            StartCoroutine(WaitForControllers());

        // if the cancel button or the "escape" key is pressed reset variables and return to main menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(GMController.instance.InputModule.cancelButton))
        {
            // disable panel
            playerModePanel.SetActive(true);

            ResetAssignment();

            GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerModePanel.transform.GetChild(0).gameObject, new BaseEventData(GMController.instance.CurrentEventSystem));
            gameObject.SetActive(false); 
        }
        // change button configuration
        if (Input.GetButtonDown(GMController.instance.InputModule.alternativeButton1))
        {
            if (configIndex > 0)           
                configIndex--;                            
            else            
                configIndex = GMController.instance.allJConfigs.Length - 1;

            ChangeButtonIcons();
        }
        else if (Input.GetButtonDown(GMController.instance.InputModule.alternativeButton2))
        {
            if (configIndex < GMController.instance.allJConfigs.Length-1)
                configIndex++;
            else
                configIndex = 0;

            ChangeButtonIcons();
        }
    }
    public void ResetAssignment()
    {
        // reset variables
        keyboardUser = 0;
        assignmentTurn = 0;
        isWaiting = false;

        GMController.instance.KeyboardInUse = false;
        GMController.instance.LastControllerAssigned = -1;

        for (int i = 0; i < playerNumber; i++)
        {
            playerButtons[i].SetActive(false);
            playerControllerText[i].gameObject.SetActive(false);
        }

        GMController.instance.controllerCheckNeeded = true;
    }
    public void ChangeButtonIcons()
    {
        config.text = GMController.instance.allJConfigs[configIndex].name;
        for (int i = 0; i < GMController.instance.allJConfigs[configIndex].buttonDescription.Length; i++)
        {
            if (configButtons[i].buttonFunctionality.text != GMController.instance.allJConfigs[configIndex].buttonDescription[i].buttonFunctionality)
                configButtons[i].buttonFunctionality.text = GMController.instance.allJConfigs[configIndex].buttonDescription[i].buttonFunctionality;

            configButtons[i].buttonIcon.sprite = GMController.instance.allJConfigs[configIndex].buttonDescription[i].buttonIcon;
        }
    }

    IEnumerator WaitForControllers()
    {
        isWaiting = false;

        for (int y = 0; y < playerButtons.Length; y++)
        {
            if (playerButtons[y].activeSelf)
            {
                playerButtons[y].GetComponent<Button>().interactable = false;
            }
        }
        Debug.Log("INSERT CONTROLLER");

        yield return new WaitUntil(() => GMController.instance.NumbOfJoysticks >= assignmentTurn); 

        int first = -1;
        for (int y = 0; y < playerButtons.Length; y++)
        {
            if (playerButtons[y].activeSelf)
            {
                playerButtons[y].GetComponent<Button>().interactable = true;
                if (first == -1)
                    first = y;                
            }
        }
        GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerButtons[first], new BaseEventData(GMController.instance.CurrentEventSystem));
        GMController.instance.ChangeInputModule(GMController.instance.JConfigInMenu, GMController.instance.ActualControllersOrder[GMController.instance.LastControllerAssigned + 1] + 1); 
        Debug.Log("CONTROLLER INSERTED");
    }

    public void ActivateButtons(int playerNumber)
    {
        ChangeButtonIcons();  
        for (int i = 0; i < playerNumber; i++)
        {
            playerButtons[i].SetActive(true);
            playerButtons[i].GetComponent<Button>().interactable = true;
            playerControllerText[i].gameObject.SetActive(true);
            playerControllerText[i].text = "Controller " + (i+1) + " - ";
        }
        GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerButtons[0], new BaseEventData(GMController.instance.CurrentEventSystem));
    }
   
    public void AssignConfig() 
    {     
        for (int i = 0; i < GMController.instance.PlayersRequired; i++)
        {
            // the selected button index is equal to the player index in game
            if (GMController.instance.CurrentEventSystem.currentSelectedGameObject == playerButtons[i]) 
            {
                // if the input controls comes from keyboard
                if (GMController.instance.CheckInputControls(GMController.instance.keyboardMenu, GMController.instance.KeyboardConfig.ControllerIndex)) 
                {
                    GMController.instance.SelectedInputConfig[GMController.instance.PlayersRequired - 1] = GMController.instance.allJConfigs[configIndex];

                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.KeyboardConfig.PlayerInputConfig);
                    GMController.instance.PlayersInputConfig[i].ControllerIndex = GMController.instance.KeyboardConfig.ControllerIndex;
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = GMController.instance.KeyboardConfig.ControllerNumber;
                    GMController.instance.PlayersInputConfig[i].LastUsed = TYPEOFINPUT.KM;
                    keyboardUser = 1;
                    GMController.instance.KeyboardInUse = true;
                    playerControllerText[GMController.instance.PlayersRequired - 1].text = "Controller " + (GMController.instance.PlayersRequired) + " - P" + (i+1);
                }
                else   
                {
                    GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser] = GMController.instance.allJConfigs[configIndex];

                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser]);//NEED CHECK
                    GMController.instance.PlayersInputConfig[i].ControllerIndex = GMController.instance.ActualControllersOrder[assignmentTurn - keyboardUser];
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = assignmentTurn-keyboardUser;
                    GMController.instance.PlayersInputConfig[i].LastUsed = TYPEOFINPUT.J;
                    //Debug.Log(GMController.instance.PlayersInputConfig[i].ControllerNumber.ToString() + "  " + GMController.instance.PlayersInputConfig[i].ControllerIndex.ToString() + " " + GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser].ToString());
                    GMController.instance.LastControllerAssigned = assignmentTurn - keyboardUser;
                    playerControllerText[assignmentTurn - keyboardUser].text = "Controller " + (assignmentTurn - keyboardUser + 1) + " - P" + (i+1);  

                }

                assignmentTurn++;
                playerButtons[i].SetActive(false);
                for (int y = 0; y < playerButtons.Length; y++)  
                {
                    if (playerButtons[y].activeSelf)
                    {
                        GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerButtons[y], new BaseEventData(GMController.instance.CurrentEventSystem));
                        break;
                    }
                } 
              
                break; 
            }
        }
        // give menu control to the next player
        if (assignmentTurn < GMController.instance.PlayersRequired)
        {
            configIndex = 0;
            // if there are enough controller for the next player NEED CHECK  
            if (GMController.instance.NumbOfJoysticks > GMController.instance.LastControllerAssigned + 1)
            {
                GMController.instance.ChangeInputModule(GMController.instance.JConfigInMenu, GMController.instance.ActualControllersOrder[GMController.instance.LastControllerAssigned + 1] + 1);
            }
            // if there are not enough controllers and the keyboard was NOT assigned
            else if (GMController.instance.NumbOfJoysticks < GMController.instance.PlayersRequired && !GMController.instance.KeyboardInUse)
            {
                GMController.instance.ChangeInputModule(GMController.instance.keyboardMenu, GMController.instance.KeyboardConfig.ControllerIndex);
            }
            // if there are not enough controllers and the keyboard was assigned 
            else
            {
                isWaiting = true;
            }
        }
        // if all the players are done then go to the game modality panel
        else if (assignmentTurn == GMController.instance.PlayersRequired)
        {
            modalityPanel.SetActive(true);
            GMController.instance.CurrentEventSystem.SetSelectedGameObject(modality[0].gameObject, new BaseEventData(GMController.instance.CurrentEventSystem));
            for (int i = 0; i < modality.Length; i++)
            {
                modality[i].playerNumber = playerNumber;
            }

            GMController.instance.controllerCheckNeeded = true;
            gameObject.SetActive(false); 
        }
       
       
    }
}
