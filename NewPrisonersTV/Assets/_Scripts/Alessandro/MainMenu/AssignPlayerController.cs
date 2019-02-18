using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssignPlayerController : MonoBehaviour
{
    public GameObject[] playerButtons;
    public int keyboardUser = 0;
    private int assignmentTurn = 0; // is equal to the index of the player that has to choose

    public void ActivateButtons(int playerNumber)
    {
        for (int i = 0; i < playerNumber; i++)
        {
            playerButtons[i].SetActive(true);
        }
        GMController.instance.eventSystem.SetSelectedGameObject(playerButtons[0], new BaseEventData(GMController.instance.eventSystem));        
    }
   
    private void AssignControllerNumber(int i)
    {
        Debug.Log(Input.GetJoystickNames().Length);
        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
        {
            if (!string.IsNullOrEmpty(Input.GetJoystickNames()[j]))
            {
                Debug.Log(Input.GetJoystickNames()); 
                bool assigned = false;
                for (int x = 0; x < GMController.instance.PlayersInputConfig.Length; x++)
                {
                    if (GMController.instance.PlayersInputConfig[x] != null &&
                        GMController.instance.PlayersInputConfig[x].ControllerNumber == j)
                        assigned = true;
                }
                if (!assigned)
                {
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = j;
                    break;
                }
            }
        }
    }

    public void AssignConfig() 
    {     
        for (int i = 0; i < GMController.instance.PlayersRequired; i++)
        {
            // the selected button index is equal to the player index in game
            if (GMController.instance.eventSystem.currentSelectedGameObject == playerButtons[i])
            {

                if (GMController.instance.CheckInputControls(GMController.instance.keyboardConfig.PlayerInputConfig,-1))
                {
                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.keyboardConfig.PlayerInputConfig);
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = -1;
                    keyboardUser = 1;
                }
                else 
                {   // search for a configuration matching the input module config and assign it to player
                    for (int y = 0; y < GMController.instance.SelectedInputConfig.Length; y++)
                    {
                        for (int j = 0; j < Input.GetJoystickNames().Length; j++)
                        {
                            if (!string.IsNullOrEmpty(Input.GetJoystickNames()[j]))
                            {
                                if (GMController.instance.CheckInputControls(GMController.instance.SelectedInputConfig[y],j+1))
                                {
                                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.SelectedInputConfig[y]);                 
                                    bool assigned = false;
                                    for (int x = 0; x < GMController.instance.PlayersInputConfig.Length; x++)
                                    {
                                        if (GMController.instance.PlayersInputConfig[x] != null &&
                                            GMController.instance.PlayersInputConfig[x].ControllerNumber == j)
                                            assigned = true;
                                    }
                                    if (!assigned)
                                    {
                                        GMController.instance.PlayersInputConfig[i].ControllerNumber = j;
                                        GMController.instance.lastControllerAssigned = j;
                                        break;
                                    }
                                }
                               
                            }
                        }
                       
                    }                  
                }
                                            
                assignmentTurn++;
                playerButtons[i].SetActive(false);
                for (int y = 0; y < playerButtons.Length; y++)  
                {
                    if (playerButtons[y].activeSelf)
                    {
                        GMController.instance.eventSystem.SetSelectedGameObject(playerButtons[y], new BaseEventData(GMController.instance.eventSystem));
                        break;
                    }
                }
                // give menu control to the next player
                if (assignmentTurn < GMController.instance.PlayersRequired)
                {
                    for (int y = 0; y < GMController.instance.actualControllersOrder.Length; y++)
                    {
                        if(GMController.instance.actualControllersOrder[y] == GMController.instance.lastControllerAssigned && GMController.instance.actualControllersOrder[y]< GMController.instance.actualControllersOrder.Length-1)
                        {
                            GMController.instance.ChangeInputModule(GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser], GMController.instance.actualControllersOrder[y+1]+1); 
                            break;
                        }
                    }
                    
                }
                break;
            }
        }
        // if all the players are done then starts the game
        if(assignmentTurn == GMController.instance.PlayersRequired)
        {
            GMController.instance.NextLevel(); 
        }

        if (GMController.instance.numbOfJoysticks < GMController.instance.PlayersRequired && !GMController.instance.KeyboardInUse)
        {           
            GMController.instance.ChangeInputModule(GMController.instance.keyboardConfig.PlayerInputConfig,-1);
            GMController.instance.KeyboardInUse = true;                     
        }
    }
}
