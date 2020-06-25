using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    private bool isInGame;

    #region
    [Header("Enemy GameObject Prefabs")]
    [Tooltip("The prefab GameObject for the small enemy.")]
    public GameObject levelOneEnemy;
    [Tooltip("The prefab GameObject for the medium enemy.")]
    public GameObject levelTwoEnemy;
    [Tooltip("The prefab GameObject for the large enemy.")]
    public GameObject levelThreeEnemy;
    #endregion

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
    #endregion

    #region
    [Header("Enemy Difficulty Settings")]
    [Tooltip("The base maximum number of small enemies that will spawn at a time in the first wave on the easy difficulty.")]
    public int easyBaseMaxSmallEnemies;
    [Tooltip("The base maximum number of medium enemies that will spawn at a time in the first wave on the easy difficulty.")]
    public int easyBaseMaxMediumEnemies;
    [Tooltip("The base maximum number of large enemies that will spawn at a time in the first wave on the easy difficulty.")]
    public int easyBaseMaxLargeEnemies;
    [Space]
    [Tooltip("The base maximum number of small enemies that will spawn at a time in the first wave on the normal difficulty.")]
    public int normalBaseMaxSmallEnemies;
    [Tooltip("The base maximum number of medium enemies that will spawn at a time in the first wave on the normal difficulty.")]
    public int normalBaseMaxMediumEnemies;
    [Tooltip("The base maximum number of large enemies that will spawn at a time in the first wave on the normal difficulty.")]
    public int normalBaseMaxLargeEnemies;
    [Space]
    [Tooltip("The base maximum number of small enemies that will spawn at a time in the first wave on the hard difficulty.")]
    public int hardBaseMaxSmallEnemies;
    [Tooltip("The base maximum number of medium enemies that will spawn at a time in the first wave on the hard difficulty.")]
    public int hardBaseMaxMediumEnemies;
    [Tooltip("The base maximum number of large enemies that will spawn at a time in the first wave on the hard difficulty.")]
    public int hardBaseMaxLargeEnemies;
    [Space]
    [Tooltip("The value by which the maxiumum number of small enemies spawning in a wave increases each wave on the easy difficulty.")]
    public int easyMaxSmallEnemyIncrease;                                                  
    [Tooltip("The value by which the maxiumum number of medium enemies spawning in a wave increases each wave on the easy difficulty.")]
    public int easyMaxMediumEnemyIncrease;                                                 
    [Tooltip("The value by which the maxiumum number of large enemies spawning in a wave increases each wave on the easy difficulty.")]
    public int easyMaxLargeEnemyIncrease;                                                  
    [Space]                                                                                
    [Tooltip("The value by which the maxiumum number of small enemies spawning in a wave increases each wave on the normal difficulty.")]
    public int normalMaxSmallEnemyIncrease;                                                
    [Tooltip("The value by which the maxiumum number of medium enemies spawning in a wave increases each wave on the normal difficulty.")]
    public int normalMaxMediumEnemyIncrease;                                               
    [Tooltip("The value by which the maxiumum number of large enemies spawning in a wave increases each wave on the normal difficulty.")]
    public int normalMaxLargeEnemyIncrease;
    [Space]
    [Tooltip("The value by which the maximum number of small enemies spawning in a wave increases each wave on the hard difficulty.")]
    public int hardMaxSmallEnemyIncrease;
    [Tooltip("The value by which the maximum number of medium enemies spawning in a wave increases each wave on the hard difficulty.")]
    public int hardMaxMediumEnemyIncrease;
    [Tooltip("The value by which the maximum number of large enemies spawning in a wave increases each wave on the hard difficulty.")]
    public int hardMaxLargeEnemyIncrease;
    [Space]
    public int smallEnemiesInEasyWave;
    public int mediumEnemiesInEasyWave;
    public int largeEnemiesInEasyWave;
    [Space]
    public int smallEnemiesInNormalWave;
    public int mediumEnemiesInNormalWave;
    public int largeEnemiesInNormalWave;
    [Space]
    public int smallEnemiesInHardWave;
    public int mediumEnemiesInHardWave;
    public int largeEnemiesInHardWave;

    private int smallEnemiesSpawned;
    private int mediumEnemiesSpawned;
    private int largeEnemiesSpawned;

    private int enemiesKilled;
    #endregion

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
    private bool timerActive;
    public float intermissionLength;
    private float intermissionTimerValue;
    #endregion

    public GameObject[] starstoneArray;

    private PlayerController playerController;
    private UIController uIController;

    public enum gameDifficulty
    {
        easyDifficulty,
        normalDifficulty,
        hardDifficulty

    };

    [Tooltip("The current difficulty setting in the game.")]
    public gameDifficulty currentGameDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        currentGameDifficulty = gameDifficulty.normalDifficulty;
        if(enemySpawnDelay == 0) { enemySpawnDelay = 1f; }
        if(easyWaveTime == 0) { easyWaveTime = 180f; }
        if(normalWaveTime == 0) { normalWaveTime = 120f; }
        if(hardWaveTime == 0) { hardWaveTime = 90f; }
        spawnCooldownTime = enemySpawnDelay;
        currentWave = 1;
        canSpawnEnemy = true;
        intermissionTimerValue = intermissionLength;

        //Thomas' Work//
        activeSmallEnemies = new List<GameObject>();
        activeMediumEnemies = new List<GameObject>();
        activeLargeEnemies = new List<GameObject>();
        //End of work//
    }

    // Update is called once per frame
    void Update()
    {
        if (isInGame)
        {
            GameTimers();
            EnemySpawning();
            CheckEnemyStatus();
            if (enemiesKilled >= smallEnemiesInWave + mediumEnemiesInWave + largeEnemiesInWave)
            {
                intermissionTimerValue -= Time.deltaTime;
                uIController.UpdateIntermissionTimer(intermissionTimerValue);
                if (intermissionTimerValue <= 0) 
                {
                    NextWave();
                }
            }
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        Debug.Log(level);
        if(level == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            enemySpawnPoints = new Transform[4];
            currentWave = 1;
            for(int i = 0; i < enemySpawnPoints.Length; i++)
            {
                enemySpawnPoints[i] = GameObject.Find("EnemySpawner" + i).GetComponent<Transform>();
            }
            switch (currentGameDifficulty)
            {
                case gameDifficulty.easyDifficulty:
                    playerController.healthRegenCutoff = 70f;
                    gameWaveTime = easyWaveTime;
                    waveTimerValue = gameWaveTime;
                    maxSmallEnemies = easyBaseMaxSmallEnemies;
                    maxMediumEnemies = easyBaseMaxMediumEnemies;
                    maxLargeEnemies = easyBaseMaxLargeEnemies;
                    smallEnemiesInWave = smallEnemiesInEasyWave;
                    mediumEnemiesInWave = mediumEnemiesInEasyWave;
                    largeEnemiesInWave = largeEnemiesInEasyWave;
                    break;
                case gameDifficulty.normalDifficulty:
                    playerController.healthRegenCutoff = 60f;
                    gameWaveTime = normalWaveTime;
                    waveTimerValue = gameWaveTime;
                    maxSmallEnemies = normalBaseMaxSmallEnemies;
                    maxMediumEnemies = normalBaseMaxMediumEnemies;
                    maxLargeEnemies = normalBaseMaxLargeEnemies;
                    smallEnemiesInWave = smallEnemiesInNormalWave;
                    mediumEnemiesInWave = mediumEnemiesInNormalWave;
                    largeEnemiesInWave = largeEnemiesInNormalWave;
                    break;
                case gameDifficulty.hardDifficulty:
                    playerController.healthRegenCutoff = 50f;
                    gameWaveTime = hardWaveTime;
                    waveTimerValue = gameWaveTime;
                    maxSmallEnemies = hardBaseMaxSmallEnemies;
                    maxMediumEnemies = hardBaseMaxMediumEnemies;
                    maxLargeEnemies = hardBaseMaxLargeEnemies;
                    smallEnemiesInWave = smallEnemiesInHardWave;
                    mediumEnemiesInWave = mediumEnemiesInHardWave;
                    largeEnemiesInWave = largeEnemiesInHardWave;
                    break;
            }
            isInGame = true;
            timerActive = true;
            uIController.SetBaseTimerValue(waveTimerValue);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            isInGame = false;
            timerActive = false;
        }
    }

    public void NextWave()
    {
        Debug.Log("New wave starting!");
        enemiesKilled = 0;
        smallEnemiesSpawned = 0;
        mediumEnemiesSpawned = 0;
        largeEnemiesSpawned = 0;
        waveTimerValue = gameWaveTime;
        currentWave++;
        uIController.UpdateWaveNumber(currentWave);
    }

    public void GameTimers()
    {
        if (timerActive == true)
        {
            waveTimerValue -= Time.deltaTime;
            if (waveTimerValue <= 0)
            {
                Debug.Log("Game Over!");
            }
            uIController.UpdateWaveTimer(waveTimerValue);
        }

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
        for(int i = 0; i <= activeSmallEnemies.Count - 1; i++)
        {
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
        Debug.Log(enemiesKilled);
        Debug.Log(smallEnemiesInWave + mediumEnemiesInWave + largeEnemiesInWave);
    }

    public void EnemySpawning()
    {
        int arrayIndex = UnityEngine.Random.Range(0, 4);
        Transform pointToSpawn = enemySpawnPoints[arrayIndex];
        if (activeSmallEnemies.Count < maxSmallEnemies && canSpawnEnemy && smallEnemiesSpawned + 1 <= smallEnemiesInWave)
        {
            activeSmallEnemies.Add(Instantiate(levelOneEnemy, pointToSpawn.position, Quaternion.identity));
            smallEnemiesSpawned++;
            canSpawnEnemy = false;
        }
        if(activeMediumEnemies.Count < maxMediumEnemies && canSpawnEnemy && mediumEnemiesSpawned + 1 <= mediumEnemiesInWave)
        {
            activeMediumEnemies.Add(Instantiate(levelTwoEnemy, pointToSpawn.position, Quaternion.identity));
            mediumEnemiesSpawned++;
            canSpawnEnemy = false;
        }
        if(activeLargeEnemies.Count < maxLargeEnemies && canSpawnEnemy && largeEnemiesSpawned + 1 <= largeEnemiesInWave)
        {
            activeLargeEnemies.Add(Instantiate(levelThreeEnemy, pointToSpawn.position, Quaternion.identity));
            largeEnemiesSpawned++;
            canSpawnEnemy = false;
        }
    }

    public void BuffEnemies(int activeBuff)
    {
        switch (activeBuff)
        {
            case 0:
                for(int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    NavMeshAgent enemyAgent = activeSmallEnemies[i].GetComponent<NavMeshAgent>();
                    enemyAgent.speed = 6;
                }
                for(int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    NavMeshAgent enemyAgent = activeMediumEnemies[j].GetComponent<NavMeshAgent>();
                    enemyAgent.speed = 5;
                }
                for(int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    NavMeshAgent enemyAgent = activeLargeEnemies[k].GetComponent<NavMeshAgent>();
                    enemyAgent.speed = 5;
                }
                break;
            case 1:
                for(int i = 0; i <= activeSmallEnemies.Count - 1; i++)
                {
                    smallEnemy enemyController = activeSmallEnemies[i].GetComponent<smallEnemy>();
                    enemyController.maxEnemyHP += 10;
                    if(enemyController.enemyHP >= enemyController.maxEnemyHP - 10) 
                    {
                        enemyController.enemyHP = enemyController.maxEnemyHP;
                    }
                }
                for(int j = 0; j <= activeMediumEnemies.Count - 1; j++)
                {
                    mediumEnemy enemyController = activeMediumEnemies[j].GetComponent<mediumEnemy>();
                    enemyController.maxEnemyHP += 10;
                    if(enemyController.enemyHP >= enemyController.maxEnemyHP - 10)
                    {
                        enemyController.enemyHP = enemyController.maxEnemyHP;
                    }
                }
                for(int k = 0; k <= activeLargeEnemies.Count - 1; k++)
                {
                    mediumEnemy enemyController = activeLargeEnemies[k].GetComponent<mediumEnemy>();
                    enemyController.maxEnemyHP += 10;
                    if(enemyController.enemyHP >= enemyController.maxEnemyHP)
                    {
                        enemyController.enemyHP = enemyController.maxEnemyHP;
                    }
                }
                break;
            case 2:
                //do another thing
                break;
            case 3:
                //do a final thing
                break;
        }
    }

    public void ActivateNewStarstone()
    {
        //Activate the next highest charged starstone
    }

    public void ChangeDifficulty(int difficulty)
    {
        currentGameDifficulty = (gameDifficulty)difficulty;
        Debug.Log(currentGameDifficulty);
    }
}
