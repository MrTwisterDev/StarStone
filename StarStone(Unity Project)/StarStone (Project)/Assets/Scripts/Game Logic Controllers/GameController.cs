using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

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
    [Header("Wave Variables")]
    [Tooltip("The duration of each wave on the Easy difficulty (in seconds).")]
    public float easyWaveTime;
    [Tooltip("The duration of each wave on the Normal difficulty (in seconds).")]
    public float normalWaveTime;
    [Tooltip("The duration of each wave on the Hard difficulty (in seconds).")]
    public float hardWaveTime;
    private float timerValue;
    private int currentWave;
    [Tooltip("The number of small enemies in the current wave.")]
    public int smallEnemiesInWave;
    [Tooltip("The number of medium enemies in the current wave.")]
    public int mediumEnemiesInWave;
    [Tooltip("The number of large enemies in the current wave.")]
    public int largeEnemiesInWave;
    private bool timerActive;
    #endregion

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
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        Debug.Log(level);
        if(level == 1)
        {
            playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            enemySpawnPoints = new Transform[4];
            for(int i = 0; i < enemySpawnPoints.Length; i++)
            {
                enemySpawnPoints[i] = GameObject.Find("EnemySpawner" + i).GetComponent<Transform>();
            }
            switch (currentGameDifficulty)
            {
                case gameDifficulty.easyDifficulty:
                    playerController.healthRegenCutoff = 70f;
                    timerValue = easyWaveTime;
                    maxSmallEnemies = 5;
                    maxMediumEnemies = 3;
                    maxLargeEnemies = 2;
                    break;
                case gameDifficulty.normalDifficulty:
                    playerController.healthRegenCutoff = 60f;
                    timerValue = normalWaveTime;
                    maxSmallEnemies = 8;
                    maxMediumEnemies = 5;
                    maxLargeEnemies = 3;
                    break;
                case gameDifficulty.hardDifficulty:
                    playerController.healthRegenCutoff = 50f;
                    timerValue = hardWaveTime;
                    maxSmallEnemies = 10;
                    maxMediumEnemies = 7;
                    maxLargeEnemies = 5;
                    break;
            }
        }
        isInGame = true;
        timerActive = true;
    }

    public void GameTimers()
    {
        if (timerActive == true)
        {
            timerValue -= Time.deltaTime;
            if (timerValue <= 0)
            {
                Debug.Log("Game Over!");
            }
            uIController.UpdateWaveTimer(timerValue);
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
            }
        }
        for (int j = 0; j <= activeMediumEnemies.Count - 1; j++)
        {
            if (activeMediumEnemies[j] == null)
            {
                GameObject deadEnemy = activeMediumEnemies[j];
                activeMediumEnemies.Remove(deadEnemy);   
            }
        }
        for (int k = 0; k <= activeLargeEnemies.Count - 1; k++)
        {
            if (activeLargeEnemies[k] == null)
            {
                GameObject deadEnemy = activeLargeEnemies[k];
                activeLargeEnemies.Remove(deadEnemy);   
            }
        }
    }

    public void EnemySpawning()
    {
        int arrayIndex = UnityEngine.Random.Range(0, 4);
        Transform pointToSpawn = enemySpawnPoints[arrayIndex];
        if (activeSmallEnemies.Count < maxSmallEnemies && canSpawnEnemy)
        {
            activeSmallEnemies.Add(Instantiate(levelOneEnemy, pointToSpawn.position, Quaternion.identity));
            canSpawnEnemy = false;
        }
        if(activeMediumEnemies.Count < maxMediumEnemies && canSpawnEnemy)
        {
            activeMediumEnemies.Add(Instantiate(levelTwoEnemy, pointToSpawn.position, Quaternion.identity));
            canSpawnEnemy = false;
        }
        if(activeLargeEnemies.Count < maxLargeEnemies && canSpawnEnemy)
        {
            activeLargeEnemies.Add(Instantiate(levelThreeEnemy, pointToSpawn.position, Quaternion.identity));
            canSpawnEnemy = false;
        }
    }

    public void ChangeDifficulty(int difficulty)
    {
        currentGameDifficulty = (gameDifficulty)difficulty;
        Debug.Log(currentGameDifficulty);
    }
}
