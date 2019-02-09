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
    #region EXPOSED VARIABLES
    public int startGameTimer;
    public float deathTimer = 0f;
    public float slowdownTimerMultiplier;                                   // used to slow down the enemy spawn rate when the key is in game
    //------------------------------------------------------------------
    public GameObject[] playerPrefab;
    public Transform[] playerSpawnPoint;
    public Transform decalPool;                                             // reference to the decal pool parent obj
    public DecalDepot decalDepot;                                           // reference to the decal prefab list
    public Transform bulletPool;                                            // reference to the bullet pool parent obj
    public BulletDepot bulletDepot;                                         // reference to the bullet prefab list
    public TensionBarStats tensionStats;
    //------------------------------------------------------------------
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
    [BoxGroup("Enemy Settings")] public int maxKamikaze; 
    [BoxGroup("Enemy Settings")] public int maxSpiders;
    [BoxGroup("Enemy Settings")] public int maxDogs;
    [BoxGroup("Enemy Settings")] public int maxSentinel;
    #endregion
    //------------------------------------------------------------------
    // Needed for Singleton pattern 
    [HideInInspector] public static GMController instance = null;
    // Variables used in order to trigger transitions when the game is not active
    [HideInInspector] public bool isGameActive = false;                      // state machine stuff
    [HideInInspector] public bool canStartGameCD = false;                    // start game countdown coroutine
    [HideInInspector] public bool gameStart = false;                         // start players and everithing else in the level, if false pause the game, skips updates and sets the players to inactive
    [HideInInspector] public bool inGame = false;                            // true when in a level scene, enables the state change to the currentState

    [HideInInspector] public PlayerInfo[] playerInfo;                        // info on players in the  current scene  
    [HideInInspector] public Camera m_MainCamera;

    // Needed for game mode setup ---------------------------------------
    [HideInInspector] public bool playerSetupDone = false; 
    [HideInInspector] public float currentGameTime;
    [HideInInspector] public UIManager3D UI;
    [HideInInspector] public BonusWeapon bonusWeapon;
    [HideInInspector] public WeaponSpawn[] weaponSpawns;                      // list of weapon spawns
    [HideInInspector] public int currentTensionLevel = 0;                     // tension bar fill level
    [HideInInspector] public int currentTensionMulti = 1;                     // tension bar multiplier
    [HideInInspector] public int currentTensionMax;                           // max tension bar capacity
    [HideInInspector] public int tensionThreshold;                            // number of segments in the bar
    [HideInInspector] public TensionBonus[] tensionBonus;                     // list of bonus to apply at each threshold
    //------------------------------------------------------------------
    #region ENEMIES/SPAWNS INFO
    [HideInInspector] public int maxEnemy; 
   
    [HideInInspector] public EnemySpawn[] enemySpawns;
    [HideInInspector] public List<_EnemyController> allEnemies;

    private _EnemyController[] startingEnemies;

    private int currentEnemyCount;
    private int currentBats;
    private int currentNinja;
    private int currentKamikaze;
    private int currentSpiders;
    private int currentDogs;
    private int currentSentinel;
    #endregion
    //------------------------------------------------------------------
    #region STATIC VARIABLES
    private static int playerRequired;                                              // number of players for the current game mode
    private static GAMEMODE currentMode = GAMEMODE.Menu;                            // current game mode, is Menu by default
    private static int levelCount = 0;
    public int LevelCount { get { return levelCount; } }

    // copy of the lists of scenes used for the pool
    private static List<string> currentEasyScenes;
    private static List<string> currentMediumScenes;
    private static List<string> currentHardScenes;

    private int totalScenes;                                                         // sum of all scenes needed for the game mode
    public int TotalScenes { get { return totalScenes; } }

    private static int[] playersTotalScore;                                          // record the sum of all level scores for each player in the current game
    private static Weapon3D[] weaponRewardFromLastLevel;                             // record the rewards choosen in the last level
    #endregion
    //------------------------------------------------------------------
    private bool keyInGame = false;                                                  // true when the key can be spawned
    [HideInInspector] public bool canSpawnKey = true;                                // true if the key is not in game
    //------------------------------------------------------------------
    #region END GAME/REWARDS VARIABLES
    [HideInInspector] public bool gameEnded = false;                                 // true if the player reach and interact with the exit door. it disables pause 
    [HideInInspector] public bool canResultCR = true;                                // if false starts the result coroutine
    [HideInInspector] public bool canChooseReward = false;                           // if true enables the reward script
    [HideInInspector] public bool readyForNextLevel = false;                         // if true allow to skip to the next level
     public int[] CrescentScoreOrder;                                // list of players from the one with less score to the one with top score 
    [HideInInspector] public int lastPlayerThatChooseReward;                         // records the last player number that choose a weapon reward
    [HideInInspector] public int rewardIndex = 0;                                    // used for choosing rewards
    #endregion
    //------------------------------------------------------------------

    void Awake() 
    {
        //Singleton
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);
        // Sum up scenes and enemies
        totalScenes = maxEasyScenes + maxMediumScenes + maxHardScenes;
        maxEnemy = maxBats + maxNinja + maxKamikaze + maxSpiders + maxDogs + maxSentinel; //sum if all enemy types

        //Get all the players required for the current game mode
        if (currentMode != GAMEMODE.Menu)
        {
            inGame = true;
            // if the total score array doesn't exist the initialize one
            if (playersTotalScore == null)
                playersTotalScore = new int[playerRequired];

            // GETS THE REQUIRED COMPONENTS FOR THIS MODE
            UI = FindObjectOfType<UIManager3D>();
            bonusWeapon = UI.rewardPanel.GetComponent<BonusWeapon>(); 
            CrescentScoreOrder = new int[playerRequired]; 
            playerInfo = new PlayerInfo[playerRequired];

            //spawn players and add them to the current playerInfo list, collect spawn and enemy info
            PlayerSetup();
            StartEnemyCount();
            CollectEnemySpawns();
            CollectWeaponSpawns();
            // set tension threshold and max bar dimension
            currentTensionMax = (int)(tensionStats.standardBarCapacity * (1 + (tensionStats.multiXPlayer * (playerInfo.Length-1))));
            tensionThreshold = currentTensionMax / tensionStats.barDivision;
            TensionBonusSetup();

            // if the current level is not the first of the current game
            if (levelCount > 1)
            {
                // add weapon of choice
                for (int i = 0; i < playerInfo.Length; i++)
                {
                    SetStartingWeapon(i);
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
            inGame = false;
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

        if (gameStart)
        {   // update spawn timer for enemies
            if (enemySpawns != null)
            {
                for (int i = 0; i < enemySpawns.Length; i++)
                {
                    if (enemySpawns[i].spawnType != ENEMYTYPE.None)
                    {
                        if (enemySpawns[i].GetTimer() > 0)
                        {
                            enemySpawns[i].TimerCountDown();
                        }
                        else
                        {
                            enemySpawns[i].SpawnEnemyCheck();
                        }
                    }
                }
            }
        }
    }
    //------------------------------------------------------------------
    private void SetStartingWeapon(int playerNumber)
    {
        Weapon3D rewardWeapon = Instantiate(weaponRewardFromLastLevel[playerNumber]);
        rewardWeapon.isReward = true;
        rewardWeapon.bullets = rewardWeapon.bulletsIfReward;
        rewardWeapon.hand = playerInfo[playerNumber].playerController.playerRightArm.transform.GetChild(0).gameObject;
        rewardWeapon.weaponMembership = playerNumber;
        rewardWeapon.GrabAndDestroy(playerInfo[playerNumber].playerController);
    }
    public void AddWeaponReward(int playerNumber, Weapon3D weapon)
    {
        weaponRewardFromLastLevel[playerNumber] = weapon;
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

    public void AddLevelCount()
    {
        levelCount++;
    }
      
    public void LowerTensionCheck(int newTension)
    {
        int tensionDifference = 0;
        //detract points from tension
        if ((currentTensionLevel - newTension) >= 0)
            currentTensionLevel -= newTension;
        else if (instance.currentTensionMulti > 1)
        {
            currentTensionMulti--;
            tensionDifference = currentTensionLevel - newTension;
            currentTensionLevel = currentTensionMax + tensionDifference;
           // Debug.Log("tension diff  " + tensionDifference + " tenLevel  "+ currentTensionLevel ); 
            UI.UpdateTensionMulti();// update UI
            UI.ChangeAllThresholUIColor(); // update UI color
        }
        else if (currentTensionLevel < 0)
            currentTensionLevel = 0; 
        UI.UpdateTensionBar(); // update UI
    }
    public void TensionThresholdCheck(int newTension)
    {   
        // used to temporary store the extra points when the bar is full
        int tensionDifference = 0;
        // add tension points to current level
        if((currentTensionLevel + newTension) > currentTensionMax)
        {
            tensionDifference = (currentTensionLevel + newTension) - currentTensionMax;
            currentTensionLevel = currentTensionMax; 
        }
        else
            currentTensionLevel += newTension;

        for (int i = 1; i <= tensionStats.barDivision; i++)
        {   //calculate various threshold steps
            int currentThreshold = tensionThreshold * i;
            //set the max threshold to the max bar capacity
            if (currentThreshold > currentTensionMax)            
                currentThreshold = currentTensionMax;           
            //if the tension level is equal/superior the currentThreshold then activate bonus
            if (currentTensionLevel >= currentThreshold)
            {
                //activate bonus
                for (int y = 0; y < tensionBonus.Length; y++)
                {   // if the bonus is not already active and the tension and the threshold are right
                    if (!tensionBonus[y].isActive && currentTensionMulti == tensionBonus[y].barMulti && tensionBonus[y].barThreshold == i)
                    {
                        TensionThresholdBonuses(y);
                        UI.ChangeThresholdUIColor(i-1);   // change the color of the UI element
                    }                  
                }
            }
        }
        currentTensionLevel += tensionDifference;
        // if the bar is full then increase the multiplier and add the difference to the currentTensionLevel
        if (currentTensionLevel >= currentTensionMax)
        {
            if (currentTensionMulti < tensionStats.maxBarMulti)
            {
                currentTensionMulti++;
                tensionDifference = currentTensionLevel - currentTensionMax;
                currentTensionLevel = tensionDifference;
                UI.UpdateTensionMulti();// update UI
                UI.ResetThresholdUIColor(); // update UI color
            }
            else 
                currentTensionLevel = currentTensionMax;   
        }
        UI.UpdateTensionBar();   // update UI
    }
    private void TensionThresholdBonuses(int index)
    {   // give the bonus based on the type
        if (tensionBonus[index].type == BONUSTYPE.EnemyLevel)
        {
            for (int i = 0; i < enemySpawns.Length; i++)
            {
                if (enemySpawns[i].spawnLevel < 3)
                    enemySpawns[i].spawnLevel++;
            }
            tensionBonus[index].isActive = true;
        }
        else if(tensionBonus[index].type == BONUSTYPE.NewWeapons)
        {
            for (int i = 0; i < weaponSpawns.Length; i++)
            {
                weaponSpawns[i].weaponList = tensionBonus[index].newList;
                weaponSpawns[i].NewRates(tensionBonus[index].lowGradeRate, tensionBonus[index].midGradeRate, tensionBonus[index].specialGradeRate);
                weaponSpawns[i].Sliderproportion();
                weaponSpawns[i].ReorderRates();
            }
            tensionBonus[index].isActive = true;
        }
    }
    private void TensionBonusSetup()
    {   
        tensionBonus = new TensionBonus[tensionStats.bonus.Length];        
        for (int i = 0; i < tensionBonus.Length; i++)
        {
            tensionBonus[i] = new TensionBonus(tensionStats.bonus[i]);   
        }
    }
    //------------------------------------------------------------------
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
            else if (startingEnemies[i].enemyType == ENEMYTYPE.Kamikaze)
            {
                AddKamikazeCount();
            }
            else if (startingEnemies[i].enemyType == ENEMYTYPE.Spider)
            {
                AddSpidersCount();
            }

        }
    }
    private void CollectEnemySpawns()
    {
        enemySpawns = FindObjectsOfType<EnemySpawn>();
    }
    private void CollectWeaponSpawns()
    {
        weaponSpawns = FindObjectsOfType<WeaponSpawn>();
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
            playerInfo[i].playerController.SetupBaseWeapon();
            bulletPool.transform.GetChild(i).gameObject.SetActive(true);// activate the player bullet pool
            decalPool.transform.GetChild(i).gameObject.SetActive(true);// activate the player decal pool
        }
        playerSetupDone = true;
    }
    public void RewardOrder()
    {  // create a temporary array to store scores
        int[] tempOrder = new int[playerRequired];
        for (int i = 0; i < tempOrder.Length; i++)
        {
            tempOrder[i] = playerInfo[i].score;
        }
        // sort them in crescent order
        System.Array.Sort(tempOrder); 
        // compare the temp array scores with the players and assign reward in the right order
        for (int i = 0; i < tempOrder.Length; i++)
        {
            for (int y = 0; y < playerInfo.Length; y++)
            {
                if (playerInfo[y].score == tempOrder[i])
                {
                    CrescentScoreOrder[i] = playerInfo[y].playerController.playerNumber;
                }
            }
        }
    }

    public IEnumerator StartGameCD()
    {
        // game start countdown
        canStartGameCD = false;
        for (int i = 0; i < startGameTimer; i++)
        {
            UI.objectiveText.text = "Game Starts in " + (startGameTimer-i);
            yield return new WaitForSeconds(1);
        } 

        gameStart = true;
        yield return null; 
    }
    public void NextLevel()
    {
        AddLevelCount();
        // easy difficulty
        if (levelCount <= maxEasyScenes)
        {
            int i = Random.Range(0, currentEasyScenes.Count);
            string level = currentEasyScenes[i];
            currentEasyScenes.RemoveAt(i);
            SceneManager.LoadScene(level);
        }
        // medium difficulty
        else if (levelCount > maxEasyScenes && levelCount <= (totalScenes - maxHardScenes))
        {
            int i = Random.Range(0, currentMediumScenes.Count);
            string level = currentMediumScenes[i];
            currentMediumScenes.RemoveAt(i);
            SceneManager.LoadScene(level);
        }
        // hard difficulty
        else if (levelCount > (totalScenes - maxHardScenes) && levelCount <= totalScenes)
        {
            int i = Random.Range(0, currentHardScenes.Count);
            string level = currentHardScenes[i];
            currentHardScenes.RemoveAt(i);
            SceneManager.LoadScene(level);
        }
        else
        {
            currentMode = GAMEMODE.Menu;
            SceneManager.LoadScene(0);
        }

    }
    //------------------------------------------------------------------
    #region EnemyCount Get/Set

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
        currentNinja--;
        SubEnemyCount();
    }

    public int GetKamikazeCount()
    {
        return currentKamikaze;
    }
    public void AddKamikazeCount()
    {
        currentKamikaze++;
        AddEnemyCount();
    }
    public void SubKamikazeCount()
    {
        currentKamikaze--;
        SubEnemyCount();
    }

    public int GetSpidersCount()
    {
        return currentSpiders;
    }
    public void AddSpidersCount()
    {
        currentSpiders++;
        AddEnemyCount();
    }
    public void SubSpidersCount()
    {
        currentSpiders--;
        SubEnemyCount();
    }

    public int GetDogsCount()
    {
        return currentDogs;
    }
    public void AddDogsCount()
    {
        currentDogs++;
        AddEnemyCount();
    }
    public void SubDogsCount()
    {
        currentDogs--;
        SubEnemyCount();
    }

    public int GetSentinelCount()
    {
        return currentSentinel;
    }
    public void AddSentinelCount()
    {
        currentSentinel++;
        AddEnemyCount();
    }
    public void SubSentinelCount()
    {
        currentSentinel--;
        SubEnemyCount();
    }

    #endregion
    //------------------------------------------------------------------
}


