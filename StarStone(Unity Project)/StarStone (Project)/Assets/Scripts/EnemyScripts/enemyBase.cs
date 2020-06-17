using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBase : MonoBehaviour
{

    //~James' Work~\\
    public GameObject soulParticles;
    //~~~~~~~~~~~~~~\\

    public float enemyHP;
    public enum enemyStates
    {
        idleState, //Enemy is Idle and not attacking the player
        interuptState, //The enemy has been interupted from either state by being shot (By a weapon with stun) or another circumstance
        hostileState //The enemy is agressive and actively seeking the player to kill
    }

    public enum stoneBuffs
    {
        speedBuff,
        healthBuff,
        fireBuff,
        noBuff
    }

    public enemyStates enemyState;

     void Start()
     {
        
     }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damageAmount)
    {
        enemyHP -= damageAmount;
        if(enemyHP <= 0)
        {
            destroyEnemy();
        }
    }

    public virtual void destroyEnemy() //Method virtual so enemies can have unique death animations, however if not overriden destroy the enemy
    {
        //James' work\\
        Instantiate(soulParticles, transform.position, soulParticles.transform.rotation);
        //~~~~~~~~~~~~\\
        Destroy(gameObject);
    }
}
