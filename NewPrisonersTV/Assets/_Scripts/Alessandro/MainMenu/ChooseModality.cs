using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChooseModality : MonoBehaviour
{

    public GAMEMODE gameMode;
    public AssignPlayerController controllerAssignment;
    public GameObject modalityPanel;
    
    public void GoTo()
    {
        GMController.instance.CurrentMode = gameMode;
        GMController.instance.NextLevel();        
    }

   

    public void Options()
    {
        // add options functions
    }
}
