using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mediumEnemy : enemyBase
{
    

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case enemyStates.idleState:

                break;
            case enemyStates.interuptState:

                break;
            case enemyStates.hostileState:
                enemyAgent.destination = nearestPlayer.transform.position;


                currentTimer -= Time.deltaTime;
                if(currentTimer <= 0 && getNearestPlayer() <= minimumProjectileRadius)
                {
                    meleePlayer();
                }
                else
                {
               //     resetTimer(true);
                }
                break;
        }
    }
}
