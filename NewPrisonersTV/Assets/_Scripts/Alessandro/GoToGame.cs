using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoToGame : MonoBehaviour
{
    public int playerNumber;
    public EventSystem eventSystem;
    public GameObject buttons;
    public GameObject modalityPanel;
    public ChooseModality[] modality;

    public void SetNumber()
    {
        modalityPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(modality[0].gameObject, new BaseEventData(eventSystem));
        for (int i = 0; i < modality.Length; i++)
        {
            modality[i].playerNumber = playerNumber;
        }
        buttons.SetActive(false);
    }

    public void BackToMenu()
    {
        modalityPanel.SetActive(false);
        buttons.SetActive(true);
        eventSystem.SetSelectedGameObject(buttons.transform.GetChild(0).gameObject, new BaseEventData(eventSystem));
    }
}
