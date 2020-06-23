using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject levelOneEnemy;
    public GameObject levelTwoEnemy;
    public GameObject levelThreeEnemy;

    public List <GameObject> activeSmallEnemies;
    public int maxSmallEnemies;
    public List<GameObject> activeMediumEnemies;
    public int maxMediumEnemies;
    public List<GameObject> activeLargeEnemies;
    public int maxLargeEnemies;
    private bool canSpawnEnemy;
    private float spawnCooldownTime;
    public Transform[] enemySpawnPoints;

    private bool timerActive;
    public float timerValue;

    public PlayerController playerController;
    public UIController uIController;

    public enum gameDifficulty
    {
        easyDifficulty,
        normalDifficulty,
        hardDifficulty

    };

    public gameDifficulty currentGameDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        timerValue = 10f;
        spawnCooldownTime = 1.75f;
        canSpawnEnemy = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameTimers();
        EnemySpawning();
        CheckEnemyStatus();
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
                    timerValue = 180f;
                    maxSmallEnemies = 5;
                    maxMediumEnemies = 3;
                    maxLargeEnemies = 2;
                    break;
                case gameDifficulty.normalDifficulty:
                    playerController.healthRegenCutoff = 60f;
                    timerValue = 120f;
                    maxSmallEnemies = 8;
                    maxMediumEnemies = 5;
                    maxLargeEnemies = 3;
                    break;
                case gameDifficulty.hardDifficulty:
                    playerController.healthRegenCutoff = 50f;
                    timerValue = 90f;
                    maxSmallEnemies = 10;
                    maxMediumEnemies = 7;
                    maxLargeEnemies = 5;
                    break;
            }
        }
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
