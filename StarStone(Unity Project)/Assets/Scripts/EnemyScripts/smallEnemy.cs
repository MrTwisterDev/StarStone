using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class smallEnemy : enemyBase
{







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
                else if (hasMelee && currentTimer <= 0 && getNearestPlayer() <= minimumProjectileRadius)
                {
                    Debug.Log(gameObject.name + " Attacks dealing: " + meleeDamage + " Damage!!");
                    meleePlayer();
                }

                break;
        }
    }

}
