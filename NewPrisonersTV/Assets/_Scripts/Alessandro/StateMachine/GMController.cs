using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StateMachine;
using Character;

public class GMController : MonoBehaviour
{
    // Needed for Singleton pattern 
    [HideInInspector] public static GMController instance = null;

    // Variables used in order to trigger transitions when the game is not active
    public bool isGameActive = false;
    public float deathTimer = 0f;

    [HideInInspector] public PlayerInfo[] playerInfo;
    [HideInInspector] public Camera m_MainCamera;

    // Needed for game mode setup
    [HideInInspector] public bool playerSetupDone = false;
    public int playerRequired;
    public GAMEMODE currentMode;

    public GameObject[] playerPrefab;
    public Transform[] playerSpawnPoint;

    void Awake() 
    {
        //Singleton
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);

        //Get all the players required for the current game mode
        if (currentMode != GAMEMODE.None)
        {
            playerInfo = new PlayerInfo[playerRequired];
            //spawn players and add them to the current playerInfo list
            PlayerSetup();     
        }

    }

    private void Start()
    { 
        m_MainCamera = Camera.main;        
    }

    public void SetActive(bool state)
    {
        isGameActive = state;
    }

    public bool GetGameStatus()
    {
        return isGameActive;
    }

    public void PlayerSetup()
    {
        for (int i = 0; i < playerRequired; i++)
        {
            GameObject player = Instantiate(playerPrefab[i], playerSpawnPoint[i].position, playerSpawnPoint[i].rotation);

            playerInfo[i] = new PlayerInfo(player, player.GetComponent<_CharacterController>(), playerSpawnPoint[i], 0);

            playerInfo[i].playerController.playerNumber = i;
        }
        playerSetupDone = true;
    }

    //public IEnumerator WaitFadeOut()
    //{
    //    yield return new WaitForSecondsRealtime(fadeOutTime);
    //}

    public void MoveToNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void MoveToScene(int nextSceneIndex)
    {
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}


