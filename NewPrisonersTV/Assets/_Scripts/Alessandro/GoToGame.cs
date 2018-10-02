using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{
    public int playerNumber;
    public GAMEMODE gameMode;
    
    public void GoTo()
    {
        GMController.instance.SetGameMode(gameMode);
        GMController.instance.SetPlayersRequired(playerNumber);

        SceneManager.LoadScene("Level Test AE");
    }

    public void Options()
    {
        // add options functions
    }
}
