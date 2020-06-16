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


    void Start()
    {
        enemyState = enemyStates.idleState;
        players = GameObject.FindGameObjectsWithTag("Player"); //Array used for multiple player handling (While multiple players aren't originally planned they may be added)
        enemyAgent = GetComponent<NavMeshAgent>();

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
                break;
        }
    }

    void fireProjectile()
    {
        GameObject instancedProjectile = Instantiate(projectileToFire, Vector3.forward,Quaternion.identity);
    }

    bool detectPlayer()
    {
        if(minimumDetectionRadius >= getNearestPlayer()){
            Vector3 _Direction = nearestPlayer.transform.position - transform.position;
            RaycastHit hitObject;
            Physics.Raycast(transform.position, _Direction, out hitObject);
            if(hitObject.collider.gameObject.tag == "Player")
            {
                Debug.Log("HIT!");
                return true;
            }
            else
            {
                Debug.Log("NOT HIT!");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, minimumDetectionRadius);
    }
}
