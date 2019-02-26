﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssignPlayerController : MonoBehaviour
{
    public GameObject[] playerButtons;
    public GameObject playerModePanel;
    public GameObject modalityPanel;
    public ChooseModality[] modality;
    [HideInInspector] public int playerNumber;

    private int keyboardUser = 0;
    private int assignmentTurn = 0; // is equal to the index of the player that has to choose
    private bool isWaiting = false;

    private void Update()
    {
        Debug.Log(assignmentTurn);  
        if (isWaiting)
            StartCoroutine(WaitForControllers());
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(GMController.instance.InputModule.cancelButton))
        {
            // disable panel
            playerModePanel.SetActive(true);

            keyboardUser = 0;
            assignmentTurn = 0;
            isWaiting = false; 

            GMController.instance.controllerCheckNeeded = true;
            GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerModePanel.transform.GetChild(0).gameObject, new BaseEventData(GMController.instance.CurrentEventSystem));
            gameObject.SetActive(false);
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

        yield return new WaitUntil(() => GMController.instance.NumbOfJoysticks >= assignmentTurn || assignmentTurn == 0);

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
        for (int i = 0; i < playerNumber; i++)
        {
            playerButtons[i].SetActive(true);
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
                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.KeyboardConfig.PlayerInputConfig);
                    GMController.instance.PlayersInputConfig[i].ControllerIndex = GMController.instance.KeyboardConfig.ControllerIndex;
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = GMController.instance.KeyboardConfig.ControllerNumber;
                    GMController.instance.PlayersInputConfig[i].LastUsed = TYPEOFINPUT.KM;
                    keyboardUser = 1;
                }
                else   
                {
                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser]);//NEED CHECK
                    GMController.instance.PlayersInputConfig[i].ControllerIndex = GMController.instance.ActualControllersOrder[assignmentTurn - keyboardUser];
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = assignmentTurn-keyboardUser;
                    GMController.instance.PlayersInputConfig[i].LastUsed = TYPEOFINPUT.J;
                    //Debug.Log(GMController.instance.PlayersInputConfig[i].ControllerNumber.ToString() + "  " + GMController.instance.PlayersInputConfig[i].ControllerIndex.ToString() + " " + GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser].ToString());
                    GMController.instance.LastControllerAssigned = assignmentTurn - keyboardUser;                   
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
            // if there are enough controller for the next player NEED CHECK 
            if (GMController.instance.NumbOfJoysticks > GMController.instance.LastControllerAssigned + 1 && GMController.instance.ActualControllersOrder[GMController.instance.LastControllerAssigned] < GMController.instance.NumbOfJoysticks - 1)
            {
                GMController.instance.ChangeInputModule(GMController.instance.JConfigInMenu, GMController.instance.ActualControllersOrder[GMController.instance.LastControllerAssigned + 1] + 1);
            }
            else if (GMController.instance.NumbOfJoysticks < GMController.instance.PlayersRequired && !GMController.instance.KeyboardInUse)
            {
                GMController.instance.ChangeInputModule(GMController.instance.keyboardMenu, GMController.instance.KeyboardConfig.ControllerIndex);
                GMController.instance.KeyboardInUse = true;
            }
            else // if there are not enough controllers and the keyboard was assigned
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
