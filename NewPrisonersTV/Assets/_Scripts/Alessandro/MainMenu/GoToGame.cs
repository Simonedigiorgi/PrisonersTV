using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoToGame : MonoBehaviour
{
    public int playerNumber;
    public GameObject playerModePanel;
    public GameObject modalityPanel;
    public GameObject assignmentPanel;
    public ChooseModality[] modality;

    AssignPlayerController assignment;

    public void SetNumber()
    {
        GMController.instance.PlayersRequired = playerNumber;
        GMController.instance.PlayersInputConfig = new ConfigInUse[playerNumber];

        assignment = assignmentPanel.GetComponent<AssignPlayerController>();
        GMController.instance.controllerCheckNeeded = false;
        assignmentPanel.SetActive(true);
        assignment.ActivateButtons(playerNumber);

        GMController.instance.CurrentEventSystem.SetSelectedGameObject(assignment.playerButtons[0], new BaseEventData(GMController.instance.CurrentEventSystem));
        playerModePanel.SetActive(false);
    }
    public void BackToMenu()
    {
        // needed to reset variables from AssignPlayerController
        assignment = assignmentPanel.GetComponent<AssignPlayerController>();
        assignment.ResetAssignment();

        modalityPanel.SetActive(false);
        playerModePanel.SetActive(true); 
        GMController.instance.CurrentEventSystem.SetSelectedGameObject(playerModePanel.transform.GetChild(0).gameObject, new BaseEventData(GMController.instance.CurrentEventSystem));
    }
}
