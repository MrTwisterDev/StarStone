using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mediumEnemy : enemyBase
{





    void Start()
    {
        enemyState = enemyStates.hostileState;
        players = GameObject.FindGameObjectsWithTag("Player"); //Array used for multiple player handling (While multiple players aren't originally planned they may be added)
        enemyAgent = GetComponent<NavMeshAgent>();

        resetTimer();
        getNearestPlayer(); //Get the closest player to the enemy


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
                    resetTimer();
                }
                else if(hasMelee)
                {
                    meleePlayer();
                    resetTimer();
                }

                break;
        }
    }



    private void resetTimer()
    {
        currentTimer = Random.Range(projectileMinTimer, projectileMaxTimer);
    }

    private void OnDrawGizmos()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, minimumDetectionRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gameObject.transform.position, minimumProjectileRadius);
            Gizmos.DrawWireSphere(gameObject.transform.position, maximumProjectileRadius);
        }
    }
}
