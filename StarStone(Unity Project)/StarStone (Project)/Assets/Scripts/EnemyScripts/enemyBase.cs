using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBase : MonoBehaviour
{

    [Header("Instansiated Objects")]

    //~James' Work~\\
    public GameObject soulParticles;
    public GameObject healthPack;
    public GameObject ammoBox;
    public GameObject invulnerabilityDrop;
    public GameObject speedBoostDrop;
    [Tooltip("The chance of the enemy dropping anything at all when they die.")]
    public int chanceToDrop;
    [Tooltip("The % chance that an enemy will drop a health pack or ammo box when they die.")]
    public int chanceToDropPickup;
    [Tooltip("The chance that the enemy will drop a powerup on death.")]
    public int chanceToDropPowerUp;
    //~~~~~~~~~~~~~~\\
    public GameObject projectileToFire;
    public GameObject nearestPlayer;

   

    protected NavMeshAgent enemyAgent;
    protected GameObject[] players;

    [Header ("AI Values")]
    [Tooltip("The radius of which the enemy will spot the player")]
    public float detectionRadius;
    [Tooltip("The minimum distance of which enemies will use projectiles, if a player is closer than this the enemy will use their melee attack")]
    public float minimumProjectileRadius;//If 0 then the player can be anywhere within the maximum radius to throw projectiles
    [Tooltip("The maximum distance the enemy will use a projectile")]
    public float maximumProjectileRadius;//The distance of which the enemy will try and "shoot" projectiles at the enemy

    [Tooltip("The speed of which a projectile travels in air")]
    public float projectileSpeed;//The speed a projectile will travel
    [Tooltip("The maximum amount of time until an enemy will fire a projectile")]
    public float attackMaxTimer;//The maximum time it takes for a new projectile to fire/use melee attack
    [Tooltip("The minimum amount of time until an enemy will fire a projectile")]
    public float attackMinTimer;//The minimum time it takes for a AI to fire a projectile/use melee attack
    [SerializeField] protected float currentTimer;

    [Header("Enemy Stats")]
    [Tooltip("The amount of Health this enemy has")]
    public float enemyHP;
    [Tooltip("The amount of damage this enemies melee attack deals")]
    public int meleeDamage;

    public bool hasMelee;

    //James' Work\\
    [Header("Fire Damage")]
    [Tooltip("The amount of damage the enemy takes from burning.")]
    public float burnDamage;
    [Tooltip("The amount of time for which an enemy will burn.")]
    public float burnTime;
    [HideInInspector]
    public float burnTimer;
    [Tooltip("Determines whether or not an enemy should take burn damage.")]
    public bool isBurning;
    public ParticleSystem burnParticles;
    //~~~~~~~~~~~\\

    [Header("Debug/Test Options")]
    [SerializeField] protected bool showDebugGizmos = false;
    [HideInInspector]public float maxEnemyHP;



    public enum enemyStates
    {
        idleState, //Enemy is Idle and not attacking the player
        interuptState, //The enemy has been interupted from either state by being shot (By a weapon with stun) or another circumstance
        hostileState //The enemy is agressive and actively seeking the player to kill
    }

    [Tooltip("The amount to buff enemies health when they are health buffed")]
    [SerializeField]protected float healthBuffAmount; //Amount to buff an enemies health
    [Tooltip("The amount to intcrease an enemies speed when they are buffed with speed")]
    [SerializeField]protected float speedBuffAmount; //Amount to buff an enemies speed
    public enum stoneBuffs
    {
        speedBuff,
        healthBuff,
        fireBuff,
        blackHole,
        noBuff
    }

    public enemyStates enemyState;
    public stoneBuffs enemyPowerup;

     void Start()
     {

        maxEnemyHP = enemyHP;
        enemyState = enemyStates.hostileState;
        players = GameObject.FindGameObjectsWithTag("Player"); //Array used for multiple player handling (While multiple players aren't originally planned they may be added)
        enemyAgent = GetComponent<NavMeshAgent>();
        getNearestPlayer();
        resetTimer(false);
        //James' Work\\
        if(chanceToDropPickup == 0){ chanceToDropPickup = 25; }
        if(burnTime == 0) { burnTime = 5f; }
        if(burnDamage == 0) { burnDamage = 0.5f; }
        isBurning = false;
        //~~~~~~~~~~~\\
    }

    // Update is called once per frame
    void Update()
    {
        //James' work\\
        if (isBurning)
        {
            TakeFireDamage();
        }
        //~~~~~~~~~~~\\
    }

    public void takeDamage(float damageAmount)
    {
        //James' work - checks the enemy has more than 0 health before they take damage, otherwise with spreadshot weapons they can be "killed" multiple times
        //and spawn multiple souls
        if (enemyHP > 0)
        {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
            enemyHP -= damageAmount;
            if (enemyHP <= 0)
            {
                destroyEnemy();
            }
        }
    }

    //James' Work\\
    public void TakeFireDamage()
    {
        if(burnTimer % 1 == 0)
        {
            takeDamage(burnDamage);
            burnParticles.Play();
        }
        burnTimer -= Time.deltaTime;
        if(burnTimer <= 0)
        {
            isBurning = false;
            burnTimer = burnTime;
        }
    }
    //~~~~~~~~~~~\\

    public virtual void destroyEnemy() //Method virtual so enemies can have unique death animations, however if not overriden destroy the enemy
    {
        //James' work\\
        Instantiate(soulParticles, transform.position, soulParticles.transform.rotation);
        DropPowerUp();
        //~~~~~~~~~~~~\\
        Destroy(gameObject);
    }

    //James' work\\
    public void DropPowerUp()
    {
        //Generates a random number between 1 and 100
       int randInt = Random.Range(1, 100);
        //If the random number is less than the value of the chance to drop, a drop is spawned
        if(randInt <= chanceToDrop)
        {
            //If the value generated is even, a healthpack is dropped
            if(randInt <= chanceToDropPickup && randInt > chanceToDropPowerUp && randInt % 2 == 0)
            {
                Instantiate(healthPack, transform.position, Quaternion.identity);
            }
            //if it is odd, an ammo box is dropped
            else if(randInt <= chanceToDropPickup && randInt > chanceToDropPowerUp && randInt % 2 != 0)
            {
                Instantiate(ammoBox, transform.position, Quaternion.identity);
            }
            else if(randInt <= chanceToDropPowerUp && randInt % 2 == 0)
            {
                Instantiate(invulnerabilityDrop, transform.position, Quaternion.identity);
            }
            else if(randInt <= chanceToDropPowerUp && randInt % 2 != 0)
            {
                Instantiate(speedBoostDrop, transform.position, Quaternion.identity);
            }
        }
    }

    public GameObject fireProjectile()
    {
        GameObject instancedProjectile = Instantiate(projectileToFire, transform.position + new Vector3(0, 0.5f), transform.rotation);
        instancedProjectile.GetComponent<Rigidbody>().AddForce((nearestPlayer.transform.position - transform.position).normalized * (projectileSpeed * 100));
        instancedProjectile.GetComponent<Rigidbody>().AddForce((Physics.gravity/2));
        return instancedProjectile;

    }

    public bool detectPlayer()
    {
        if (detectionRadius >= getNearestPlayer())
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
                Debug.Log(gameObject.name + "Cannot see player! " + "Enemy hunger for food grr");
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
            if (_farthestDistance >= Vector3.Distance(transform.position, player.transform.position) && player.GetComponent<PlayerBase>().playerState != PlayerBase.PlayerStates.deadState)
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
        nearestPlayer.GetComponent<PlayerBase>().TakeDamage(meleeDamage);
        resetTimer(true);
    }

    protected void resetTimer(bool meleeAttack)
    {
        if (meleeAttack)
        {
            currentTimer = attackMinTimer;
        }
        else
        {
            currentTimer = Random.Range(attackMinTimer, attackMaxTimer);
        }
    }

    public void changePowerup(stoneBuffs newBuff)
    {
        Debug.Log("Buffing an enemy with " + newBuff.ToString());
        if(newBuff == enemyPowerup)
        {
            return;
        }

        if(enemyPowerup == stoneBuffs.healthBuff) //If the old buff is changing to a new buff, take back the health
        {
            enemyHP -= healthBuffAmount;

        }

        if (enemyPowerup == stoneBuffs.speedBuff) //If the old buff is changing to a new buff, take back the speed
        {
            gameObject.GetComponent<NavMeshAgent>().speed -= speedBuffAmount;
            gameObject.GetComponent<NavMeshAgent>().angularSpeed -= (speedBuffAmount*3);
        }

        switch (newBuff)
        {
            case stoneBuffs.noBuff:
                enemyPowerup = stoneBuffs.noBuff;

                break;
            case stoneBuffs.healthBuff:
                enemyHP += healthBuffAmount;
                enemyPowerup = stoneBuffs.healthBuff;
                break;
            case stoneBuffs.speedBuff:
                gameObject.GetComponent<NavMeshAgent>().speed += speedBuffAmount;
                gameObject.GetComponent<NavMeshAgent>().angularSpeed += speedBuffAmount*3;

                enemyPowerup = stoneBuffs.speedBuff;
                break;
            case stoneBuffs.fireBuff:


                enemyPowerup = stoneBuffs.fireBuff;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, detectionRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gameObject.transform.position, minimumProjectileRadius);
            Gizmos.DrawWireSphere(gameObject.transform.position, maximumProjectileRadius);
        }
    }

}
