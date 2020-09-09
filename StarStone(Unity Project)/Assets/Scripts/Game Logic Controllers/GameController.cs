using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class GameController : MonoBehaviour
{
    //**********************************************************************|
    // Project Name: Temple Imperium                                        |
    // Script Name: Game Controller                                         |
    // Script Author: James Smale + Thomas Lang (Win state functionality)   |
    // Purpose: Handles all aspects of game logic and flow, including       |
    //          difficulty levels, wave timers and enemy spawning           |
    //**********************************************************************|

    //Prefabs
    #region
    [Header("GameObject Prefabs")]
    [Tooltip("The prefab GameObject for the small enemy.")]
    public GameObject levelOneEnemy;
    [Tooltip("The prefab GameObject for the medium enemy.")]
    public GameObject levelTwoEnemy;
    [Tooltip("The prefab GameObject for the large enemy.")]
    public GameObject levelThreeEnemy;
    [Tooltip("The prefab for character variant one.")]
    public GameObject characterVariantOne;
    [Tooltip("The prefab for character variant two.")]
    public GameObject characterVariantTwo;
    #endregion
    //Enemy Spawning
    #region
    [Header("Enemy Spawning")]
    [Tooltip("The maximum number of active small enemies at any given time.")]
    public int maxSmallEnemies;
    private List <GameObject> activeSmallEnemies;

    [Tooltip("The maximum number of active medium enemies at any given time.")]
    public int maxMediumEnemies;
    private List<GameObject> activeMediumEnemies;

    [Tooltip("The maximum number of active large enemies at any given time.")]
    public int maxLargeEnemies;
    private List<GameObject> activeLargeEnemies;

    private bool canSpawnEnemy;
    [Tooltip("The time delay between enemies spawning.")]
    public float enemySpawnDelay;
    private float spawnCooldownTime;
    private Transform[] enemySpawnPoints;
    private Transform spawnerParent;
    private Transform pointToSpawnOn;
    #endregion
    //Enemy Difficulty Settings
    #region
    [Header("Enemy Difficulty Settings")]
    [Space]
    //Easy Settings
    [Header("Easy Settings")]
    [Tooltip("The base maximum number of small enemies that will spawn at a time in the first wave on the easy difficulty.")]
    public int easyBaseMaxSmallEnemies;
    [Tooltip("The base maximum number of medium enemies that will spawn at a time in the first wave on the easy difficulty.")]
    public int easyBaseMaxMediumEnemies;
    [Tooltip("The base maximum number of large enemies that will spawn at a time in the first wave on the easy difficulty.")]
    public int easyBaseMaxLargeEnemies;
    [Space]
    [Tooltip("The value by which the maxiumum number of small enemies spawning in a wave increases each wave on the easy difficulty.")]
    public int easyMaxSmallEnemyIncrease;
    [Tooltip("The value by which the maxiumum number of medium enemies spawning in a wave increases each wave on the easy difficulty.")]
    public int easyMaxMediumEnemyIncrease;
    [Tooltip("The value by which the maxiumum number of large enemies spawning in a wave increases each wave on the easy difficulty.")]
    public int easyMaxLargeEnemyIncrease;
    [Space]
    [Tooltip("The total number of small enemies in an easy wave.")]
    public int smallEnemiesInEasyWave;
    [Tooltip("The total number of medium enemies in an easy wave.")]
    public int mediumEnemiesInEasyWave;
    [Tooltip("The total number of large enemies in an easy wave.")]
    public int largeEnemiesInEasyWave;
    [Space]
    //Normal Settings
    [Header("Normal Settings")]
    [Tooltip("The base maximum number of small enemies that will spawn at a time in the first wave on the normal difficulty.")]
    public int normalBaseMaxSmallEnemies;
    [Tooltip("The base maximum number of medium enemies that will spawn at a time in the first wave on the normal difficulty.")]
    public int normalBaseMaxMediumEnemies;
    [Tooltip("The base maximum number of large enemies that will spawn at a time in the first wave on the normal difficulty.")]
    public int normalBaseMaxLargeEnemies;
    [Space]
    [Tooltip("The value by which the maxiumum number of small enemies spawning in a wave increases each wave on the normal difficulty.")]
    public int normalMaxSmallEnemyIncrease;
    [Tooltip("The value by which the maxiumum number of medium enemies spawning in a wave increases each wave on the normal difficulty.")]
    public int normalMaxMediumEnemyIncrease;
    [Tooltip("The value by which the maxiumum number of large enemies spawning in a wave increases each wave on the normal difficulty.")]
    public int normalMaxLargeEnemyIncrease;
    [Space]
    [Tooltip("The total number of small enemies in a normal wave.")]
    public int smallEnemiesInNormalWave;
    [Tooltip("The total number of medium enemies in a normal wave.")]
    public int mediumEnemiesInNormalWave;
    [Tooltip("The total number of large enemies in a normal wave.")]
    public int largeEnemiesInNormalWave;
    [Space]
    //Hard Settings
    [Header("Hard Settings")]
    [Tooltip("The base maximum number of small enemies that will spawn at a time in the first wave on the hard difficulty.")]
    public int hardBaseMaxSmallEnemies;
    [Tooltip("The base maximum number of medium enemies that will spawn at a time in the first wave on the hard difficulty.")]
    public int hardBaseMaxMediumEnemies;
    [Tooltip("The base maximum number of large enemies that will spawn at a time in the first wave on the hard difficulty.")]
    public int hardBaseMaxLargeEnemies;
    [Space]                                                                               
    [Tooltip("The value by which the maximum number of small enemies spawning in a wave increases each wave on the hard difficulty.")]
    public int hardMaxSmallEnemyIncrease;
    [Tooltip("The value by which the maximum number of medium enemies spawning in a wave increases each wave on the hard difficulty.")]
    public int hardMaxMediumEnemyIncrease;
    [Tooltip("The value by which the maximum number of large enemies spawning in a wave increases each wave on the hard difficulty.")]
    public int hardMaxLargeEnemyIncrease;
    [Space]
    [Tooltip("The total number of small enemies in a hard wave.")]
    public int smallEnemiesInHardWave;
    [Tooltip("The total number of medium enemies in a hard wave.")]
    public int mediumEnemiesInHardWave;
    [Tooltip("The total number of large enemies in a hard wave.")]
    public int largeEnemiesInHardWave;
    //Current Wave Information
    private int smallEnemiesSpawned;
    private int mediumEnemiesSpawned;
    private int largeEnemiesSpawned;
    private int enemiesKilled;
    #endregion
    //Wave Management
    #region
    [Header("Wave Variables")]
    [Tooltip("The duration of each wave on the Easy difficulty (in seconds).")]
    public float easyWaveTime;
    [Tooltip("The duration of each wave on the Normal difficulty (in seconds).")]
    public float normalWaveTime;
    [Tooltip("The duration of each wave on the Hard difficulty (in seconds).")]
    public float hardWaveTime;
    private float gameWaveTime;
    private float waveTimerValue;
    private int currentWave;
    [Tooltip("The number of small enemies in the current wave.")]
    public int smallEnemiesInWave;
    [Tooltip("The number of medium enemies in the current wave.")]
    public int mediumEnemiesInWave;
    [Tooltip("The number of large enemies in the current wave.")]
    public int largeEnemiesInWave;
    public int smallEnemyIncrease;
    public int mediumEnemyIncrease;
    public int largeEnemyIncrease;
    private bool timerActive;
    [Tooltip("The duration of the intermission between waves.")]
    public float intermissionLength;
    private float intermissionTimerValue;
    #endregion
    //Starstone Management
    #region
    [Header("Starstones")]
    [Tooltip("An array of the four Starstones in the scene.")]
    public GameObject[] starstoneArray;
    [Tooltip("The currently active Starstone powerup effect.")]
    public starstoneEffects currentStarstone;
    [Space]
    #endregion
    //Player Management
    #region
    [Header("Player Management")]
    [Tooltip("A boolean that determines whether the game's multiplayer co-op mode is being used.")]
    public bool isCoOp;
    private Transform playerOneSpawnPoint;
    private Transform playerTwoSpawnPoint;
    public GameObject playerOneCharacter;
    public GameObject playerTwoCharacter;
    private PlayerBase playerOneController;
    private PlayerBase playerTwoController;
    private Camera playerOneCamera;
    private Camera playerTwoCamera;
    public PlayerBase[] playerControllers;
    [Tooltip("The GameObject used to handle character selection")]
    public characterSelection coopcharacterSelector;
    public characterSelection soloCharacterSelector;
    [Tooltip("The number of players that are currently dead.")]
    public int deadPlayers;
    [Space]
    #endregion
    //Game Management
    #region
    [Header("Game Management")]
    [Tooltip("A boolean determining whether or not a player has found a generator.")]
    public bool hasFoundGenerator;
    [Tooltip("The number of souls currently contained within the generator.")]
    public int soulsInGenerator;
    [Tooltip("The number of souls required inside the generator in order to win the game.")]
    public int requiredSoulsInGenerator;
    private GameObject victoryCanvas;
    public UIController[] uIController;
    private splitScreen splitSceenController;
    private bool isInGame;
    [Space]
    [Tooltip("The current difficulty setting in the game.")]
    public gameDifficulty currentGameDifficulty;
    [Space]
    #endregion
    //Game Sounds
    #region
    public AudioClip waveStart;
    #endregion

    [HideInInspector]
    public List<GameObject> enemiesList = new List<GameObject>(); //Tom's work
    [HideInInspector]
    public bool enemiesSpawned = false; //Tom's work
    private bool finalWaveDone = false; //Tom's work

    public enum gameDifficulty
    {
        easyDifficulty,
        normalDifficulty,
        hardDifficulty

    };

    public enum starstoneEffects
    {
        speedEffect,
        healthEffect,
        fireEffect,
        buffEffect,
        noEffect
    }



    // Start is called before the first frame update
    void Start()
    {
        //Sets the default difficulty to Normal to ensure the game can't be started without a difficulty selected
        currentGameDifficulty = gameDifficulty.normalDifficulty;
        //If any of the below variables are left unassigned, they are given a default value on start
        if(enemySpawnDelay == 0) { enemySpawnDelay = 1f; }
        if(easyWaveTime == 0) { easyWaveTime = 180f; }
        if(normalWaveTime == 0) { normalWaveTime = 120f; }
        if(hardWaveTime == 0) { hardWaveTime = 90f; }
        if(requiredSoulsInGenerator == 0) { requiredSoulsInGenerator = 50; }
        //Sets the value of the enemy spawning cooldown timer to the default value input in the inspector
        spawnCooldownTime = enemySpawnDelay;
        //Starts the current wave at 1
        currentWave = 1;
        //Allows the game to spawn enemies on startup
        canSpawnEnemy = true;
        //Sets the value of the intermission timer to the value input in the inspector
        intermissionTimerValue = intermissionLength;
        //Co-op mode is turned off by default
        isCoOp = false;
        starstoneArray = new GameObject[4];

        //Thomas' Work//
        activeSmallEnemies = new List<GameObject>();
        activeMediumEnemies = new List<GameObject>();
        activeLargeEnemies = new List<GameObject>();

        currentStarstone = starstoneEffects.noEffect;
        //End of work//
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("Timer active: " + timerActive + "Is in game: " + isInGame);
        //If the game is not paused or in a menu, the game timers and enemy spawning will run
        if (isInGame)
        {
            GameTimers();
            CheckDeadPlayers();
            if (timerActive)
            {
                EnemySpawning();
                CheckEnemyStatus();
            }
            if(hasFoundGenerator && !timerActive)
            {
                timerActive = true;
                for (int i = 0; i < playerControllers.Length; i++)
                {
                    uIController[i].UpdateWaveNumber(currentWave);
                }
            }
            //If the player has killed all enemies in a wave, the wave ends and an intermission starts
            if (enemiesKilled >= smallEnemiesInWave + mediumEnemiesInWave + largeEnemiesInWave)
            {
                if (timerActive) { timerActive = false; }
                intermissionTimerValue -= Time.deltaTime;
                for (int i = 0; i < playerControllers.Length; i++)
                {
                uIController[i].UpdateIntermissionTimer((int)intermissionTimerValue);
                }
                //When the intermission timer runs out, the next wave begins
                if (intermissionTimerValue <= 0) 
                {
                    NextWave();
                    intermissionTimerValue = intermissionLength;
                }
            }
            //Thomas
            int connectedGenerators = 0;
            foreach (var generator in starstoneArray)
            {
                if (generator.GetComponent<StarstoneController>().genEnabled)
                {
                    connectedGenerators++;
                }
            }
            if(connectedGenerators == starstoneArray.Length && finalWaveDone != true)
            {
                //FINAL WAVE
                smallEnemiesInWave *= 2;
                mediumEnemiesInWave *= 2;
                largeEnemiesInWave *= 2;
                finalWaveDone = true;
            }
            //End of Thomas
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        //If the level loaded is the playable level, all of the necessary variables are assigned depending on the currently selected difficulty
        if(level == 1)
        {
            splitSceenController = FindObjectOfType<splitScreen>();
            if (isCoOp)
            {
                playerControllers = new PlayerBase[2];
                uIController = new UIController[2];
            }
            else
            {
                playerControllers = new PlayerBase[1];
                uIController = new UIController[1];
                splitSceenController.gameObject.SetActive(false);
            }
            InstantiatePlayers();
            spawnerParent = GameObject.Find("EnemySpawners").GetComponent<Transform>();
            playerOneCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            //Locks the cursor to the center of the screen to prevent it from moving outside of the playable area, ensuring the player cannot accidentall leave the game window
            Cursor.lockState = CursorLockMode.Locked;
            //Find the UI Controller object in the scene and assigns its script to the uIController variable
            for (int i = 0; i < playerControllers.Length; i++)
            {
                uIController[i] = playerControllers[i].gameObject.GetComponent<UIController>();
            }
            //Sets the length of the spawn point array
            enemySpawnPoints = new Transform[spawnerParent.childCount];
            FindStarstones();

            //Finds all of the enemy spawners in the scene and adds them to the array so they can be accessed randomly in the enemy spawning method
            for (int i = 0; i < enemySpawnPoints.Length; i++)
            {
                enemySpawnPoints[i] = spawnerParent.Find("EnemySpawner" + i).GetComponent<Transform>();
            }
            //Assigns the variables used for enemy spawning and health regeneration to the values assigned in the inspector depending on the current game difficulty
            switch (currentGameDifficulty)
            {
                case gameDifficulty.easyDifficulty:
                    playerOneController.healthRegenCutoff = 70f;
                    gameWaveTime = easyWaveTime;
                    waveTimerValue = gameWaveTime;
                    maxSmallEnemies = easyBaseMaxSmallEnemies;
                    smallEnemyIncrease = easyMaxSmallEnemyIncrease;
                    maxMediumEnemies = easyBaseMaxMediumEnemies;
                    mediumEnemyIncrease = easyMaxMediumEnemyIncrease;
                    maxLargeEnemies = easyBaseMaxLargeEnemies;
                    largeEnemyIncrease = easyMaxLargeEnemyIncrease;
                    smallEnemiesInWave = smallEnemiesInEasyWave;
                    mediumEnemiesInWave = mediumEnemiesInEasyWave;
                    largeEnemiesInWave = largeEnemiesInEasyWave;
                    break;
                case gameDifficulty.normalDifficulty:
                    playerOneController.healthRegenCutoff = 60f;
                    gameWaveTime = normalWaveTime;
                    waveTimerValue = gameWaveTime;
                    maxSmallEnemies = normalBaseMaxSmallEnemies;
                    smallEnemyIncrease = normalMaxSmallEnemyIncrease;
                    maxMediumEnemies = normalBaseMaxMediumEnemies;
                    mediumEnemyIncrease = normalMaxMediumEnemyIncrease;
                    maxLargeEnemies = normalBaseMaxLargeEnemies;
                    largeEnemyIncrease = normalMaxLargeEnemyIncrease;
                    smallEnemiesInWave = smallEnemiesInNormalWave;
                    mediumEnemiesInWave = mediumEnemiesInNormalWave;
                    largeEnemiesInWave = largeEnemiesInNormalWave;
                    break;
                case gameDifficulty.hardDifficulty:
                    playerOneController.healthRegenCutoff = 50f;
                    gameWaveTime = hardWaveTime;
                    waveTimerValue = gameWaveTime;
                    maxSmallEnemies = hardBaseMaxSmallEnemies;
                    smallEnemyIncrease = hardMaxSmallEnemyIncrease;
                    maxMediumEnemies = hardBaseMaxMediumEnemies;
                    mediumEnemyIncrease = hardMaxMediumEnemyIncrease;
                    maxLargeEnemies = hardBaseMaxLargeEnemies;
                    largeEnemyIncrease = hardMaxLargeEnemyIncrease;
                    smallEnemiesInWave = smallEnemiesInHardWave;
                    mediumEnemiesInWave = mediumEnemiesInHardWave;
                    largeEnemiesInWave = largeEnemiesInHardWave;
                    break;
            }
            isInGame = true;
            //Initiates the wave timer
            if (hasFoundGenerator)
            {
                timerActive = true;
            }
            //Updates the wave timer UI element
            for (int i = 0; i < playerControllers.Length; i++)
            {
                uIController[i].SetBaseTimerValue(waveTimerValue);
            }

        }
        else if(level == 0)
        {
            soloCharacterSelector = GameObject.Find("CharacterSelector").GetComponent<characterSelection>();
            coopcharacterSelector = GameObject.Find("Co-OpScreen").GetComponent<characterSelection>();
            coopcharacterSelector.gameObject.SetActive(false);
            ResetWaveData();
            //Resets the number of dead players to keep the value accurate
            deadPlayers = 0;
            isCoOp = false;
            Cursor.lockState = CursorLockMode.None;     //Unlocks the cursor so the player can select menu options
            isInGame = false;                           //Prevents game timers and enemy spawning methods from being executed
            timerActive = false;
            Time.timeScale = 1;
            waveTimerValue = 10f;
        }
        else if(level == 2)
        {
            playerControllers = new PlayerBase[1];
            uIController = new UIController[1];
            playerControllers[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
            uIController[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<UIController>();
        }
    }

    public void ToggleCoop()
    {
        isCoOp = !isCoOp;
    }

    public void PauseAllPlayers()
    {
        //Sets all players to be paused
        for(int i = 0; i < playerControllers.Length; i++)
        {
            Time.timeScale = 0;
            playerControllers[i].playerState = PlayerBase.PlayerStates.pausedState;
            playerControllers[i].pauseMenu.SetActive(true);
        }
    }

    public void UnpauseAllPlayers()
    {
        //Sets all players to their standard state
        for(int i = 0; i < playerControllers.Length; i++)
        {
            Time.timeScale = 1;
            playerControllers[i].pauseMenu.SetActive(false);
            playerControllers[i].playerState = PlayerBase.PlayerStates.standardState;

        }
    }

    public void UpdateChosenCharacters()
    {
        if (isCoOp)
        {
            //If the selected character variant in Henri, then the character player one will spawn as is set to character variant one
            if (coopcharacterSelector.shownPlayers[0].gameObject.tag == "Henri")
            {
                playerOneCharacter = characterVariantOne;
            }
            //otherwise, it is set to character variant two as there are only two characters to choose from
            else
            {
                playerOneCharacter = characterVariantTwo;
            }
            //If the game is being played in co-op mode, the same process is carried out to determine player two's chosen character
            if (coopcharacterSelector.shownPlayers[1].gameObject.tag == "Henri")
            {
                playerTwoCharacter = characterVariantOne;
            }
            else
            {
                playerTwoCharacter = characterVariantTwo;
            }
        }
        else
        {
            if(soloCharacterSelector.shownPlayers[0].gameObject.tag == "Henri")
            {
                playerOneCharacter = characterVariantOne;
            }
            else
            {
                playerOneCharacter = characterVariantTwo;
            }
        }
    }

    public void InstantiatePlayers()
    {
        //Finds the transforms at which the player characters should spawn
        playerOneSpawnPoint = GameObject.Find("PlayerOneSpawnPoint").GetComponent<Transform>();
        playerTwoSpawnPoint = GameObject.Find("PlayerTwoSpawnPoint").GetComponent<Transform>();
        //Instantiates player one at the correct position, saving its PlayerBase to the playerOneController variable
        playerOneController = Instantiate(playerOneCharacter, playerOneSpawnPoint.position, Quaternion.identity).GetComponent<PlayerBase>();
        playerControllers[0] = playerOneController;
        playerOneCamera = playerOneController.gameObject.GetComponentInChildren<Camera>();
        playerOneController.gameObject.name = "PlayerOne";
        //Assigns the playerNumber string as PlayerOne to ensure that inputs are read correctly
        playerOneController.playerNumber = "PlayerOne";
        //If the game is being played in co-op mode, a second player is spawned at the correct position
        if(isCoOp)
        {
            //Instantiates player two and assigns the necessary variables
            playerTwoController = Instantiate(playerTwoCharacter, playerTwoSpawnPoint.position, Quaternion.identity).GetComponent<PlayerBase>();
            playerControllers[1] = playerTwoController;
            playerTwoController.gameObject.name = "PlayerTwo";
            playerTwoController.playerNumber = "PlayerTwo";
            playerTwoCamera = playerTwoController.gameObject.GetComponentInChildren<Camera>();
            //Disables the audio listener on player two's camera so that it doesn't cause issues within Unity
            playerTwoCamera.GetComponent<AudioListener>().enabled = false;
            //Finds the split screen controller and adds the player cameras to its array before updating the screen resolution
            splitSceenController.playerCharacters[0] = playerOneCamera;
            splitSceenController.playerCharacters[1] = playerTwoCamera;
            splitSceenController.updateScreenResolution();
        }
    }

    public void FindStarstones()
    {
        starstoneArray[0] = GameObject.Find("HealthStarstone");
        starstoneArray[1] = GameObject.Find("BuffStarstone");
        starstoneArray[2] = GameObject.Find("SpeedStarstone");
        starstoneArray[3] = GameObject.Find("FireStarstone");
    }

    public void NextWave()
    {
        //Resets variables linked to enemies in order to prevent new waves from starting immediately after
        enemiesKilled = 0;
        smallEnemiesSpawned = 0;
        mediumEnemiesSpawned = 0;
        largeEnemiesSpawned = 0;
        enemiesList.Clear();
        //Resets the game's wave timer to its initial value
        waveTimerValue = gameWaveTime;
        //Increments the wave number by 1
        currentWave++;
        //Increases the number of enemies of each type that will spawn in the next round by the value dictated by the difficulty
        smallEnemiesInWave += smallEnemyIncrease;
        mediumEnemiesInWave += mediumEnemyIncrease;
        largeEnemiesInWave += largeEnemyIncrease;
        //Updates the wave information UI based on the new wave data
        for (int i = 0; i < playerControllers.Length; i++)
        {
            uIController[i].UpdateWaveNumber(currentWave);
        }

        foreach (var generator in starstoneArray)
        {
            if (generator.GetComponent<StarstoneController>().selectedGenerator == StarstoneController.selectionType.disconnecting)
            {
                generator.GetComponent<StarstoneController>().genEnabled = true;
                generator.GetComponent<StarstoneController>().selectedGenerator = StarstoneController.selectionType.notSelected;
            }
        }

        foreach (var generator in starstoneArray)
        {
            if(generator.GetComponent<StarstoneController>().selectedGenerator == StarstoneController.selectionType.isSelected)
            {
                generator.GetComponent<StarstoneController>().ActivateEffect();
                BuffEnemies();
                generator.GetComponent<StarstoneController>().selectedGenerator = StarstoneController.selectionType.disconnecting;
            }
        }

        if (finalWaveDone)
        {
            Time.timeScale = 0;
            victoryCanvas.SetActive(true);
        }
        //Plays a sound to signify the start of a new wave
        AudioSource.PlayClipAtPoint(waveStart, playerOneController.gameObject.transform.position);
        //Starts the wave timer running
        timerActive = true;
    }

    public void ResetWaveData()
    {
        //Resets all variables necessary for game flow that are not reset when the game level is loaded
        isInGame = false;
        hasFoundGenerator = false;
        timerActive = false;
        currentWave = 1;
        intermissionTimerValue = intermissionLength;
        //Loops through the lists of enemies and clears them until the number of enemies killed no longer increases
        //This is to ensure all null objects are removed from the lists of enemies before the game loads, as loading without this resulted in
        //false positive killcounts
        while (enemiesKilled > 0)
        {
            CheckEnemyStatus();
            enemiesKilled = 0;
            CheckEnemyStatus();
        }
        soulsInGenerator = 0;
        smallEnemiesSpawned = 0;
        mediumEnemiesSpawned = 0;
        largeEnemiesSpawned = 0;
    }

    public void GameTimers()
    {
        //Wave timer
        if (timerActive == true)
        {
            waveTimerValue -= Time.deltaTime;
            if (waveTimerValue <= 0)
            {
                playerOneController.playerState = PlayerBase.PlayerStates.deadState;
                if(isCoOp)
                {
                    playerTwoController.playerState = PlayerBase.PlayerStates.deadState;
                }
            }
            for (int i = 0; i < playerControllers.Length; i++)
            {
                uIController[i].UpdateWaveTimer(waveTimerValue);
            }
        }
        //Enemy spawning cooldown timer
        if (!canSpawnEnemy)
        {
            spawnCooldownTime -= Time.deltaTime;
            if(spawnCooldownTime <= 0)
            {
                spawnCooldownTime = 0.25f;
                canSpawnEnemy = true;
            }
        }
    }

    public void CheckEnemyStatus()
    {
        //Repetedly runs through the lists of each type of enemy
        for(int i = 0; i <= activeSmallEnemies.Count - 1; i++)
        {
            //If the enemy at the current list address has been killed, it is removed from the list so another can be spawned
            if(activeSmallEnemies[i] == null)
            {
                GameObject deadEnemy = activeSmallEnemies[i];
                activeSmallEnemies.Remove(deadEnemy);                
                enemiesKilled++;
            }
        }
        for (int j = 0; j <= activeMediumEnemies.Count - 1; j++)
        {
            if (activeMediumEnemies[j] == null)
            {
                GameObject deadEnemy = activeMediumEnemies[j];
                activeMediumEnemies.Remove(deadEnemy);
                enemiesKilled++;
            }
        }
        for (int k = 0; k <= activeLargeEnemies.Count - 1; k++)
        {
            if (activeLargeEnemies[k] == null)
            {
                GameObject deadEnemy = activeLargeEnemies[k];
                activeLargeEnemies.Remove(deadEnemy);
                enemiesKilled++;
            }
        }
    }

    public bool PlayerCanSeeSpawner()
    {
        RaycastHit rayhit;
        //Calculates the direction in which to fire a raycast from the spawn point to the player's camera
        Vector3 direction = (pointToSpawnOn.position - playerOneCamera.gameObject.transform.position).normalized;
        Physics.Raycast(pointToSpawnOn.position, direction, out rayhit, 1000f);
        //If the spawner hits a player, it will not be able to spawn an enemy
        if (rayhit.collider.gameObject.GetComponent<PlayerBase>())
        {
            return true;
        }
        //If the game is in Co-Op mode, it also checks if the spawner can be seen by player two
        else if (isCoOp)
        {
            direction = (pointToSpawnOn.position - playerTwoCamera.gameObject.transform.position).normalized;
            Physics.Raycast(pointToSpawnOn.position, direction, out rayhit, 1000f);
            if (rayhit.collider.gameObject.GetComponent<PlayerBase>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void CheckDeadPlayers()
    {
        //If all of the players in the game are dead, then the death canvas for each of them is activated and time is stopped
        if(deadPlayers == playerControllers.Length)
        {
            foreach(PlayerBase player in playerControllers)
            {
                player.uIController.ToggleDeathCanvas(true);
            }
            Time.timeScale = 0;
        }
    }

    public void EnemySpawning()
    {
        //Generates a random number between 0 and the number of spawner objects there are which is used to pick a random spawn point from the array
        int arrayIndex = UnityEngine.Random.Range(0, spawnerParent.childCount);
        pointToSpawnOn = enemySpawnPoints[arrayIndex];
        if (!PlayerCanSeeSpawner())
        {
            //Checks if the number of each type of enemy alive in the scene is less than the maximum number, as well as if the
            //spawning cooldown has ended and if spawning a new enemy of that type will exceed the number allowed in that wave to determine whether or not to spawn a new enemy
            if (activeSmallEnemies.Count < maxSmallEnemies && canSpawnEnemy && smallEnemiesSpawned + 1 <= smallEnemiesInWave)
            {
                activeSmallEnemies.Add(Instantiate(levelOneEnemy, pointToSpawnOn.position, Quaternion.identity));
                enemyBase newEnemy = activeSmallEnemies[activeSmallEnemies.Count - 1].GetComponent<enemyBase>();
                ApplyNewEnemyBuff(newEnemy);
                smallEnemiesSpawned++;
                canSpawnEnemy = false;
            }
            if (activeMediumEnemies.Count < maxMediumEnemies && canSpawnEnemy && mediumEnemiesSpawned + 1 <= mediumEnemiesInWave)
            {
                activeMediumEnemies.Add(Instantiate(levelTwoEnemy, pointToSpawnOn.position, Quaternion.identity));
                enemyBase newEnemy = activeMediumEnemies[activeMediumEnemies.Count - 1].GetComponent<enemyBase>();
                ApplyNewEnemyBuff(newEnemy);
                mediumEnemiesSpawned++;
                canSpawnEnemy = false;
            }
            if (activeLargeEnemies.Count < maxLargeEnemies && canSpawnEnemy && largeEnemiesSpawned + 1 <= largeEnemiesInWave)
            {
                activeLargeEnemies.Add(Instantiate(levelThreeEnemy, pointToSpawnOn.position, Quaternion.identity));
                enemyBase newEnemy = activeLargeEnemies[activeLargeEnemies.Count - 1].GetComponent<enemyBase>();
                ApplyNewEnemyBuff(newEnemy);
                largeEnemiesSpawned++;
                canSpawnEnemy = false;
            }
            enemiesSpawned = true;
        }
    }

    public void connectNewGenerator(GameObject starstoneGenerator)
    {
        bool disconnectPhase = false;
        foreach(var generator in starstoneArray)
        {
            if(generator.GetComponent<StarstoneController>().selectedGenerator == StarstoneController.selectionType.disconnecting)
            {
                disconnectPhase = true;
            }
        }
        if (disconnectPhase)
        {
            Debug.Log("Cannot connect a new generator, finish the one disconnecting first!");
        }
        else
        {
            foreach (var generator in starstoneArray)
            {

                generator.GetComponent<StarstoneController>().selectedGenerator = StarstoneController.selectionType.notSelected;
            }
            starstoneGenerator.GetComponent<StarstoneController>().selectedGenerator = StarstoneController.selectionType.isSelected;
        }
    }

    public void ApplyNewEnemyBuff(enemyBase newEnemy)
    {
        //Applies the currently activate Starstone powerup to the newly spawned enemy
        switch (currentStarstone)
        {
            case starstoneEffects.speedEffect:
                newEnemy.changePowerup(enemyBase.stoneBuffs.speedBuff);
                break;
            case starstoneEffects.healthEffect:
                newEnemy.changePowerup(enemyBase.stoneBuffs.healthBuff);
                break;
            case starstoneEffects.fireEffect:
                newEnemy.changePowerup(enemyBase.stoneBuffs.fireBuff);
                break;
            case starstoneEffects.buffEffect:
                newEnemy.changePowerup(enemyBase.stoneBuffs.noBuff);
                break;
            case starstoneEffects.noEffect:
                newEnemy.changePowerup(enemyBase.stoneBuffs.noBuff);
                break;
        }
    }

    public void BuffEnemies()
    {
        switch(currentStarstone)
        {
            //Loops through the lists of the different levels of enemy active in the scene and buffs them, depending on the current active Starstone effect
            case starstoneEffects.speedEffect:
                for(int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    activeSmallEnemies[i].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.speedBuff);
                }
                for(int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    activeMediumEnemies[j].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.speedBuff);
                }
                for(int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    activeLargeEnemies[k].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.speedBuff);
                }
                break;
            case starstoneEffects.healthEffect:
                for (int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    activeSmallEnemies[i].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.healthBuff);
                }
                for (int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    activeMediumEnemies[j].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.healthBuff);
                }
                for (int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    activeLargeEnemies[k].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.healthBuff);
                }
                break;
            case starstoneEffects.fireEffect:
                for (int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    activeSmallEnemies[i].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.fireBuff);
                }
                for (int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    activeMediumEnemies[j].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.fireBuff);
                }
                for (int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    activeLargeEnemies[k].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.fireBuff);
                }
                break;
            case starstoneEffects.buffEffect:
                for (int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    activeSmallEnemies[i].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.noBuff);
                }
                for (int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    activeMediumEnemies[j].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.noBuff);
                }
                for (int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    activeLargeEnemies[k].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.noBuff);
                }
                break;
            case starstoneEffects.noEffect:
                for (int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    activeSmallEnemies[i].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.noBuff);
                }
                for (int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    activeMediumEnemies[j].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.noBuff);
                }
                for (int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    activeLargeEnemies[k].GetComponent<enemyBase>().changePowerup(enemyBase.stoneBuffs.noBuff);
                }
                break;
        }
    }

    public void ActivateNewStarstone()
    {
        float highestCharge = 0;
        int indexToActivate = 0;
        //Iterates through the starstones in the scene to determine which to activate
        for(int i = 0; i < starstoneArray.Length - 1; i++)
        {
            StarstoneController currentStarstone = starstoneArray[i].GetComponent<StarstoneController>();
            //If the charge of the current starstone is greater than the previously recorded highest charge, the new charge is assigned to highestCharge
            //and the array index of the starstone is saved to be activated later
            if(currentStarstone.starstoneCharge > highestCharge)
            {
                highestCharge = currentStarstone.starstoneCharge;
                indexToActivate = i;
            }
        }
        //Once all starstones have been checked, the one with the highest charge is activated
        starstoneArray[indexToActivate].GetComponent<StarstoneController>().ActivateEffect();
    }

    public void ChangeDifficulty(int difficulty)
    {
        //Updates the game difficulty to whatever is selected by the user, casting an int to an enum
        currentGameDifficulty = (gameDifficulty)difficulty;
        Debug.Log(currentGameDifficulty);
    }
}
