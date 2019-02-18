using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoToGame : MonoBehaviour
{
    public int playerNumber;
    public GameObject buttons;
    public GameObject modalityPanel;
    public ChooseModality[] modality;

    public void SetNumber()
    {
        modalityPanel.SetActive(true);
        GMController.instance.CurrentEventSystem.SetSelectedGameObject(modality[0].gameObject, new BaseEventData(GMController.instance.CurrentEventSystem));
        for (int i = 0; i < modality.Length; i++)
        {
            modality[i].playerNumber = playerNumber;
        }
        GMController.instance.PlayersInputConfig = new ConfigInUse[playerNumber];
        //Debug.Log(GMController.instance.PlayersInputConfig.Length);
        buttons.SetActive(false);
    }

    public void BackToMenu()
    {
        modalityPanel.SetActive(false);
        buttons.SetActive(true); 
        GMController.instance.CurrentEventSystem.SetSelectedGameObject(buttons.transform.GetChild(0).gameObject, new BaseEventData(GMController.instance.CurrentEventSystem));
    }
}
