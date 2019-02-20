using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssignPlayerController : MonoBehaviour
{
    public GameObject[] playerButtons;
    private int keyboardUser = 0;
    private int assignmentTurn = 0; // is equal to the index of the player that has to choose
    private bool isWaiting = false;

    private void Update()
    {
        if (isWaiting)
            StartCoroutine(WaitForControllers());
    }

    IEnumerator WaitForControllers()
    {
        isWaiting = false;
        GMController.instance.CurrentEventSystem.currentInputModule.DeactivateModule();
        Debug.Log("INSERT CONTROLLER");

        yield return new WaitUntil(() => GMController.instance.NumbOfJoysticks >= assignmentTurn);

        GMController.instance.CurrentEventSystem.currentInputModule.ActivateModule(); 
        for (int y = 0; y < playerButtons.Length; y++)
        {
            if (playerButtons[y].activeSelf)
            {
                GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerButtons[y], new BaseEventData(GMController.instance.CurrentEventSystem));
                break;
            }
        }
        GMController.instance.ChangeInputModule(GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser], GMController.instance.ActualControllersOrder[GMController.instance.lastControllerAssigned + 1] + 1);
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

                if (GMController.instance.CheckInputControls(GMController.instance.KeyboardConfig.PlayerInputConfig, GMController.instance.KeyboardConfig.ControllerIndex)) 
                {
                    GMController.instance.PlayersInputConfig[i] = new ConfigInUse(GMController.instance.KeyboardConfig.PlayerInputConfig);
                    GMController.instance.PlayersInputConfig[i].ControllerIndex = GMController.instance.KeyboardConfig.ControllerIndex;
                    GMController.instance.PlayersInputConfig[i].ControllerNumber = GMController.instance.KeyboardConfig.ControllerNumber;
                    keyboardUser = 1;
                }
                else 
                {   // search for a configuration matching the input module config and assign it to player
                    bool found = false;
                    for (int y = 0; y < GMController.instance.SelectedInputConfig.Length; y++)
                    {
                        for (int j = 0; j < GMController.instance.NumbOfJoysticks; j++)
                        {
                           
                            if (GMController.instance.CheckInputControls(GMController.instance.SelectedInputConfig[y], GMController.instance.ActualControllersOrder[j]+1))
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
                                     GMController.instance.PlayersInputConfig[i].ControllerIndex = GMController.instance.ActualControllersOrder[j];
                                     GMController.instance.PlayersInputConfig[i].ControllerNumber = j; 
                                     Debug.Log(GMController.instance.PlayersInputConfig[i].ControllerNumber.ToString() +"  " + GMController.instance.PlayersInputConfig[i].ControllerIndex.ToString() + " " + GMController.instance.SelectedInputConfig[y].ToString());  
                                     GMController.instance.lastControllerAssigned = j;
                                     found = true;
                                     break;
                                 }
                            }                          
                        }
                        if (found)
                            break;
                    }                  
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
            if (GMController.instance.NumbOfJoysticks > GMController.instance.lastControllerAssigned + 1 && GMController.instance.ActualControllersOrder[GMController.instance.lastControllerAssigned] < GMController.instance.NumbOfJoysticks - 1)
            {
                GMController.instance.ChangeInputModule(GMController.instance.SelectedInputConfig[assignmentTurn - keyboardUser], GMController.instance.ActualControllersOrder[GMController.instance.lastControllerAssigned + 1] + 1);
            }
            else if (GMController.instance.NumbOfJoysticks < GMController.instance.PlayersRequired && !GMController.instance.KeyboardInUse)
            {
                GMController.instance.ChangeInputModule(GMController.instance.KeyboardConfig.PlayerInputConfig, GMController.instance.KeyboardConfig.ControllerIndex);
                GMController.instance.KeyboardInUse = true;
            }
            else // if there are not enough controllers and the keyboard was assigned
            {
                isWaiting = true;
            }
        }
        // if all the players are done then starts the game
        else if (assignmentTurn == GMController.instance.PlayersRequired)
        {
            GMController.instance.NextLevel(); 
        }
       
       
    }
}
