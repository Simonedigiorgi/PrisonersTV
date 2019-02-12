using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssignPlayerController : MonoBehaviour
{
    public GameObject[] playerButtons;
    public CharacterControlConfig[] playersConfig;

    private int assignmentTurn = 0;

    public void ActivateButtons(int playerNumber)
    {
        for (int i = 0; i < playerNumber; i++)
        {
            playerButtons[i].SetActive(true);
        }
        GMController.instance.eventSystem.SetSelectedGameObject(playerButtons[0], new BaseEventData(GMController.instance.eventSystem));        
    }
   
    public void AssignConfig() 
    {     
        for (int i = 0; i < GMController.instance.PlayersRequired; i++)
        {
            if (GMController.instance.eventSystem.currentSelectedGameObject == playerButtons[i])
            {
                GMController.instance.PlayersInputConfig[i] = playersConfig[assignmentTurn];
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
                if(assignmentTurn < GMController.instance.PlayersRequired)
                    GMController.instance.ChangeInputModule(playersConfig[assignmentTurn]);
                break;
            }
        }
        if(assignmentTurn == GMController.instance.PlayersRequired)
        {
            GMController.instance.NextLevel(); 
        }

        GMController.instance.numbOfJoysticks = 0;
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            if (!string.IsNullOrEmpty(Input.GetJoystickNames()[i]))
                GMController.instance.numbOfJoysticks++;
        }
        if (GMController.instance.numbOfJoysticks <= 1)
        {
            GMController.instance.ChangeInputModule(GMController.instance.keyboardConfig);
            if (assignmentTurn < GMController.instance.PlayersRequired)
                playersConfig[assignmentTurn] = GMController.instance.keyboardConfig;
        }
    }
}
