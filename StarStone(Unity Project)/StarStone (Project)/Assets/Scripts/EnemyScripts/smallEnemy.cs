using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class smallEnemy : enemyBase
{





    void Start()
    {
        maxEnemyHP = enemyHP;
        enemyState = enemyStates.hostileState;
        players = GameObject.FindGameObjectsWithTag("Player"); //Array used for multiple player handling (While multiple players aren't originally planned they may be added)
        enemyAgent = GetComponent<NavMeshAgent>();
        getNearestPlayer();
        resetTimer(false);
    }

    // Update is called once per frame
    void Update()
    {

        switch (enemyState)
        {
            case enemyStates.idleState:
                if (detectPlayer())
                {
                    enemyState = enemyStates.hostileState;
                }
                break;
            case enemyStates.interuptState:

                break;
            case enemyStates.hostileState:
                enemyAgent.destination = nearestPlayer.transform.position;

                currentTimer -= Time.deltaTime;
                if (currentTimer <= 0 && getNearestPlayer() > minimumProjectileRadius)
                {
                    fireProjectile();
                    resetTimer(false);
                }
                else if(hasMelee && currentTimer <= 0 && getNearestPlayer() <= minimumProjectileRadius)
                {
                    Debug.Log(gameObject.name + " Attacks dealing: " + meleeDamage + " Damage!!");
                    meleePlayer();
                }

                break;
        }
    }


}
