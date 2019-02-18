using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChooseModality : MonoBehaviour
{

    public GAMEMODE gameMode;
    public AssignPlayerController controllerAssignment;
    public GameObject modalityPanel;
    [HideInInspector] public int playerNumber;
    
    public void GoTo()
    {
        GMController.instance.CurrentMode = gameMode;
        GMController.instance.PlayersRequired = playerNumber;

        if (playerNumber > 1)
        {
            controllerAssignment.gameObject.SetActive(true);
            controllerAssignment.ActivateButtons(playerNumber);
            modalityPanel.SetActive(false);
        }
        else
        {           
            GMController.instance.PlayersInputConfig[0] = new ConfigInUse(GMController.instance.SelectedInputConfig[0]);
            if (GMController.instance.NumbOfJoysticks == 0)
                GMController.instance.PlayersInputConfig[0].ControllerIndex = 0;              
            else
                GMController.instance.PlayersInputConfig[0].ControllerIndex = GMController.instance.ActualControllersOrder[0];
              
            GMController.instance.PlayersInputConfig[0].ControllerNumber = 0;

            GMController.instance.NextLevel();
        }
    }

   

    public void Options()
    {
        // add options functions
    }
}
