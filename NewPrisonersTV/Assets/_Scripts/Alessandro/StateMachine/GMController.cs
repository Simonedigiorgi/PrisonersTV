using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StateMachine;
using Character;
using AI;
using Sirenix.OdinInspector;

public class GMController : MonoBehaviour
{
    public float startGameTimer;
    public float deathTimer = 0f;
    public float slowdownTimerMultiplier; 

    public GameObject[] playerPrefab;
    public Transform[] playerSpawnPoint;

    // Needed for Singleton pattern 
    [HideInInspector] public static GMController instance = null;
    // Variables used in order to trigger transitions when the game is not active
    [HideInInspector] public bool isGameActive = false;
    [HideInInspector] public bool canStartGameCD = false;                    // start game countdown coroutine
    [HideInInspector] public bool gameStart = false;                         // start players and everithing else in the level

    [HideInInspector] public PlayerInfo[] playerInfo;                        // info on players in the  current scene
    [HideInInspector] public Camera m_MainCamera;

    // Needed for game mode setup
    [HideInInspector] public bool playerSetupDone = false;
    [HideInInspector] public float currentGameTime;
    [HideInInspector] public int maxEnemy;
    [HideInInspector] public UIManager3D UI;
    [HideInInspector] public BonusWeapon bonusWeapon;

    [BoxGroup("Story Settings")] public float gameTimer;
    [BoxGroup("Story Settings")] public float keySpawnTime;
    [BoxGroup("Story Settings")] public GameObject key;
    [BoxGroup("Story Settings")] public Transform keySpawn;

    [BoxGroup("Max levels per difficulty")] public int maxEasyScenes;
    [BoxGroup("Max levels per difficulty")] public int maxMediumScenes;
    [BoxGroup("Max levels per difficulty")] public int maxHardScenes;
    
    [BoxGroup("List of all levels")] public List<string> easyScenes;
    [BoxGroup("List of all levels")] public List<string> mediumScenes;
    [BoxGroup("List of all levels")] public List<string> hardScenes;

    [BoxGroup("Enemy Settings")] public int maxBats;
    [BoxGroup("Enemy Settings")] public int maxNinja;

    [HideInInspector] public EnemySpawn[] enemySpawns;
    [HideInInspector] public List<_EnemyController> allEnemies;

    private _EnemyController[] startingEnemies;

    private int currentEnemyCount;
    private int currentBats;
    private int currentNinja;

    private static int playerRequired;                                              // number of players for the current game mode
    private static GAMEMODE currentMode = GAMEMODE.Menu;                            // current game mode, is Menu by default
    private static int levelCount = 0;

    // copy of the lists of scenes used for the pool
    private static List<string> currentEasyScenes;
    private static List<string> currentMediumScenes;
    private static List<string> currentHardScenes;

    private int totalScenes;                                                         // sum of all scenes needed for the game mode

    private static int[] playersTotalScore;                                          // record the sum of all level scores for each player in the current game
    private static Weapon3D[] weaponRewardFromLastLevel;                             // record the rewards choosen in the last level

    [HideInInspector] public bool canSpawnKey = true;                                // true if the key is not in game
    private bool keyInGame = false;                                                  // true when the key can be spawned

    [HideInInspector] public bool gameEnded = false;                                 // true if the player reach and interact with the exit door. it disables pause
    [HideInInspector] public bool canResultCR = true;                                // if false starts the result coroutine
    [HideInInspector] public bool canChooseReward = false;
    [HideInInspector] public bool readyForNextLevel = false;                         // if true allow to skip to the next level
    [HideInInspector] public int[] DecrescentScoreOrder;
    [HideInInspector] public int lastPlayerThatChooseReward;                         // records the last player number that choose a weapon reward
    [HideInInspector] public int rewardIndex = 0;                                    // used for choosing rewards

    void Awake() 
    {
        //Singleton
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);

        totalScenes = maxEasyScenes + maxMediumScenes + maxHardScenes;
        maxEnemy = maxBats + maxNinja;

        //Get all the players required for the current game mode
        if (currentMode != GAMEMODE.Menu)
        {
            if(playersTotalScore == null)
               playersTotalScore = new int[playerRequired];
            Debug.Log(playersTotalScore.Length);

            // GETS THE REQUIRED COMPONENTS FOR THIS MODE
            UI = FindObjectOfType<UIManager3D>();
            bonusWeapon = UI.rewardPanel.GetComponent<BonusWeapon>();
            DecrescentScoreOrder = new int[playerRequired];
            playerInfo = new PlayerInfo[playerRequired];

            //spawn players and add them to the current playerInfo list
            PlayerSetup();
            StartEnemyCount();
            CollectEnemySpawns();

            if (levelCount > 1) // if the current level is not the first of the current game
            {
                // add weapon of choice
                for (int i = 0; i < playerInfo.Length; i++)
                {
                    Weapon3D rewardWeapon = Instantiate(weaponRewardFromLastLevel[i]);
                    rewardWeapon.isReward = true;
                    rewardWeapon.hand = playerInfo[i].playerController.playerRightArm.transform.GetChild(0).gameObject;
                    rewardWeapon.weaponMembership = i;
                    rewardWeapon.GrabAndDestroy(playerInfo[i].playerController);
                }
            }
            else
            {
                weaponRewardFromLastLevel = new Weapon3D[playerRequired];
            }
        }

        // refill the scene pool and reset total scores, weaponRewards
        if(currentMode == GAMEMODE.Menu)
        {
            currentEasyScenes = new List<string>(); 
            for (int i = 0; i < easyScenes.Count; i++)
            {
                currentEasyScenes.Add(easyScenes[i]);
            }
            currentMediumScenes = new List<string>();
            for (int i = 0; i < mediumScenes.Count; i++)
            {
                currentMediumScenes.Add(mediumScenes[i]);
            }
            currentHardScenes = new List<string>();
            for (int i = 0; i < hardScenes.Count; i++)
            {
                currentHardScenes.Add(hardScenes[i]);
            }

            weaponRewardFromLastLevel = null;
            playersTotalScore = null;
            levelCount = 0;
            Debug.Log(playersTotalScore);
        }     

    }

    private void Start()
    { 
        m_MainCamera = Camera.main;        
    }

    private void Update()
    {
        if(canStartGameCD)
        {
            StartCoroutine(StartGameCD());
        }
    }

    public void AddWeaponReward(int i, Weapon3D weapon)
    {
        weaponRewardFromLastLevel[i] = weapon;
    }

    public int GetPlayerTotalScore(int i)
    {
        return playersTotalScore[i];
    }
    public void AddPlayersTotalScore(int i, int score)
    {
        playersTotalScore[i] += score;
    }

    public GAMEMODE GetGameMode()
    {
        return currentMode;
    }   
    public void SetGameMode(GAMEMODE mode)
    {
        currentMode = mode;
    }

    public int GetPlayerNum()
    {
        return playerRequired;
    }
    public void SetPlayersRequired(int num)
    {
        playerRequired = num;
    }

    public int GetLevelCount()
    {
        return levelCount;
    }
    public void AddLevelCount()
    {
        levelCount++;
    }

    public int GetEnemyCount()
    {
        return currentEnemyCount;
    }
    public void AddEnemyCount()
    {
        currentEnemyCount++;
    }
    public void SubEnemyCount()
    {
        currentEnemyCount--;
    }

    public int GetBatsCount()
    {
        return currentBats;
    }
    public void AddBatsCount()
    {
        currentBats++;
        AddEnemyCount();
    }
    public void SubBatsCount()
    {
        currentBats--;
        SubEnemyCount();
    }

    public int GetNinjaCount()
    {
        return currentNinja;
    }
    public void AddNinjaCount()
    {
        currentNinja++;
        AddEnemyCount();
    }
    public void SubNinjaCount()
    {
        currentNinja++;
        SubEnemyCount();
    }

    public void SetActive(bool state)
    {
        isGameActive = state;
    }
    public bool GetGameStatus()
    {
        return isGameActive;
    }

    public bool GetKeyInGame()
    {
        return keyInGame;
    }
    public void SetKeyInGame(bool condition)
    {
        keyInGame = condition;
    }

    private void StartEnemyCount()
    {
        startingEnemies = FindObjectsOfType<_EnemyController>();
        for (int i = 0; i < startingEnemies.Length; i++)
        {
            allEnemies.Add(startingEnemies[i]);
            if(startingEnemies[i].enemyType == ENEMYTYPE.Bat)
            {
                AddBatsCount();
            }
            else if(startingEnemies[i].enemyType == ENEMYTYPE.Ninja)
            {
                AddNinjaCount();
            }
        }
    }
    public void CollectEnemySpawns()
    {
        enemySpawns = FindObjectsOfType<EnemySpawn>();
    }
    public void SlowdownSpawns()
    {
        if (!canSpawnKey)
        {
            for (int i = 0; i < enemySpawns.Length; i++)
            {
                enemySpawns[i].spawnTimer *= slowdownTimerMultiplier;
                enemySpawns[i].ResetTimer();
            }
        }
        else
        {
            for (int i = 0; i < enemySpawns.Length; i++)
            {
                enemySpawns[i].spawnTimer /= slowdownTimerMultiplier;
                enemySpawns[i].ResetTimer();
            }
        }
    }

    private void PlayerSetup()
    {
        for (int i = 0; i < playerRequired; i++)
        {
            GameObject player = Instantiate(playerPrefab[i], playerSpawnPoint[i].position, playerSpawnPoint[i].rotation);
            player.SetActive(true);
            playerInfo[i] = new PlayerInfo(player, player.GetComponent<_CharacterController>(), playerSpawnPoint[i], 0);

            playerInfo[i].playerController.playerNumber = i;
        }
        playerSetupDone = true;
    }
    public void RewardOrder()
    {
        int[] tempOrder = new int[playerRequired];
        for (int i = 0; i < tempOrder.Length; i++)
        {
            tempOrder[i] = playerInfo[i].score;
        }
        System.Array.Reverse(tempOrder);

        for (int i = 0; i < tempOrder.Length; i++)
        {
            if (playerInfo[i].score == tempOrder[i])
                DecrescentScoreOrder[i] = playerInfo[i].playerController.playerNumber;
        }
    }

    public IEnumerator StartGameCD()
    {
        // game start countdown
        canStartGameCD = false; 
        yield return new WaitForSeconds(startGameTimer);
        gameStart = true;
        yield return null;
    }

    public void NextLevel()
    {
        AddLevelCount();
        // easy difficulty
        if (GetLevelCount() <= maxEasyScenes)
        {
            int i = Random.Range(0, currentEasyScenes.Count);
            string level = currentEasyScenes[i];
            currentEasyScenes.RemoveAt(i);
            SceneManager.LoadScene(level);
        }
        // medium difficulty
        else if (GetLevelCount() > maxEasyScenes && GetLevelCount() <= (totalScenes - maxHardScenes))
        {
            int i = Random.Range(0, currentMediumScenes.Count);
            string level = currentMediumScenes[i];
            currentMediumScenes.RemoveAt(i);
            SceneManager.LoadScene(level);
        }
        // hard difficulty
        else if (GetLevelCount() > (totalScenes - maxHardScenes) && GetLevelCount() <= totalScenes)
        {
            int i = Random.Range(0, currentHardScenes.Count);
            string level = currentHardScenes[i];
            currentHardScenes.RemoveAt(i);
            SceneManager.LoadScene(level);
        }

    }
}


