using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mediumEnemy : enemyBase
{
    public GameObject projectileToFire;
    public GameObject nearestPlayer;
    private NavMeshAgent enemyAgent;
    private GameObject[] players;

    public float minimumDetectionRadius; 
    public float minimumProjectileRadius;//If 0 then the player can be anywhere within the maximum radius to throw projectiles
    public float maximumProjectileRadius;//The distance of which the enemy will try and "shoot" projectiles at the enemy

    public float projectileSpeed;//The speed a projectile will travel
    public float projectileMaxTimer;//The maximum time it takes for a new projectile to fire
    public float projectileMinTimer;//The minimum time it takes for a AI to fire a projectile
    [SerializeField] private float currentTimer;


    [SerializeField] private bool showDebugGizmos = false;


    void Start()
    {
        enemyState = enemyStates.idleState;
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
                if (currentTimer <= 0)
                {
                    fireProjectile();
                    resetTimer();
                }

                break;
        }
    }

    void fireProjectile()
    {
        GameObject instancedProjectile = Instantiate(projectileToFire,transform.position + new Vector3(0,0.5f),transform.rotation);
        instancedProjectile.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * (projectileSpeed * 100));

    }

    bool detectPlayer()
    {
        if(minimumDetectionRadius >= getNearestPlayer()){
            Vector3 _Direction = nearestPlayer.transform.position - transform.position;
            RaycastHit hitObject;
            Physics.Raycast(transform.position, _Direction, out hitObject);
            if(hitObject.collider.gameObject.tag == "Player")
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

    float getNearestPlayer() //Gets the closest player, and returns the distance the nearest player is
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
