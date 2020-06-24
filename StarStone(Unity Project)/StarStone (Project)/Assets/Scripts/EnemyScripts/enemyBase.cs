using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBase : MonoBehaviour
{

    //~James' Work~\\
    public GameObject soulParticles;
    //~~~~~~~~~~~~~~\\

    public GameObject projectileToFire;
    public GameObject nearestPlayer;
    protected NavMeshAgent enemyAgent;
    protected GameObject[] players;

    public float minimumDetectionRadius;
    public float minimumProjectileRadius;//If 0 then the player can be anywhere within the maximum radius to throw projectiles
    public float maximumProjectileRadius;//The distance of which the enemy will try and "shoot" projectiles at the enemy

    public float projectileSpeed;//The speed a projectile will travel
    public float projectileMaxTimer;//The maximum time it takes for a new projectile to fire
    public float projectileMinTimer;//The minimum time it takes for a AI to fire a projectile
    [SerializeField] protected float currentTimer;

    public float enemyHP;
    [HideInInspector]public float maxEnemyHP;
    [SerializeField] protected bool showDebugGizmos = false;

    public bool hasMelee;
    public int meleeDamage;


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
        maxEnemyHP = enemyHP;
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

    public void fireProjectile()
    {
        GameObject instancedProjectile = Instantiate(projectileToFire, transform.position + new Vector3(0, 0.5f), transform.rotation);
        instancedProjectile.GetComponent<Rigidbody>().AddForce((nearestPlayer.transform.position - transform.position).normalized * (projectileSpeed * 100));

    }

    public bool detectPlayer()
    {
        if (minimumDetectionRadius >= getNearestPlayer())
        {
            Vector3 _Direction = nearestPlayer.transform.position - transform.position;
            RaycastHit hitObject;
            Physics.Raycast(transform.position, _Direction, out hitObject);
            if (hitObject.collider.gameObject.tag == "Player")
            {
                Debug.Log(gameObject.name + "Found Player: " + hitObject.collider.gameObject.name);
                return true;
            }
            else
            {
                Debug.Log(gameObject.name + "Cannot see player!" + "Enemy hunger for food grr");
                return false;
            }
        }
        return false;
    }

    public float getNearestPlayer() //Gets the closest player, and returns the distance the nearest player is
    {
        float _farthestDistance = Mathf.Infinity;
        foreach (GameObject player in players) //
        {
            if (_farthestDistance >= Vector3.Distance(transform.position, player.transform.position))
            {
                _farthestDistance = Vector3.Distance(transform.position, player.transform.position);

                nearestPlayer = player;

            }
        }
        return _farthestDistance;
    }

    protected void meleePlayer()
    {
        //Play Melee Animation

        getNearestPlayer();

        //Currently means that a player cannot avoid a melee attack once it has started. Can be improved
        nearestPlayer.GetComponent<PlayerController>().currentHealth -= meleeDamage;
    }

}
