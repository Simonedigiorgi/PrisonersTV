using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChooseModality : MonoBehaviour
{
    public GAMEMODE gameMode;
   [HideInInspector] public int playerNumber;
    
    public void GoTo()
    {
        GMController.instance.SetGameMode(gameMode);
        GMController.instance.SetPlayersRequired(playerNumber);
        GMController.instance.NextLevel();
    }

   

    public void Options()
    {
        // add options functions
    }
}
