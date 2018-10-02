using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour, IPointerClickHandler
{
    public int playerNumber;
    public GAMEMODE gameMode;

    public void OnPointerClick(PointerEventData pointer)
    {
        GMController.instance.SetGameMode(gameMode);
        GMController.instance.SetPlayersRequired(playerNumber);

        SceneManager.LoadScene("Level Test AE");
        Debug.Log("click");
    }
}
