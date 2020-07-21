﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityProjectile : PrototypeProjectileBase
{
    public float fuseLength;
    public float blackHoleDuration;
    private bool hasDetonated;

    // Update is called once per frame
    void Update()
    {
        if (!hasDetonated)
        {
            fuseLength -= Time.deltaTime;
            if(fuseLength <= 0)
            {
                hasDetonated = true;
                rigidBody.isKinematic = true;
                rigidBody.detectCollisions = false;
                AudioSource.PlayClipAtPoint(detonationSound, transform.position);
            }
        }
        else
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        blackHoleDuration -= Time.deltaTime;
        if(blackHoleDuration <= 0)
        {
            //Loops through all the enemies affected by the black hole and disables the physics simulation of their rigidbodies to prevent strange physics interactions
            Collider[] enemiesAffected = Physics.OverlapSphere(transform.position, areaOfEffect);
            foreach(Collider enemyToAffect in enemiesAffected)
            {
                if(enemyToAffect.GetComponent<enemyBase>() != null)
                {
                    enemyToAffect.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            //Destroys the Singulairty gameobject
            Destroy(gameObject);
        }
        else
        {
            //Loops through all enemies within a sphere of influence each frame
            Collider[] enemiesAffected = Physics.OverlapSphere(transform.position, areaOfEffect);
            foreach(Collider enemyToAffect in enemiesAffected)
            {
                if(enemyToAffect.GetComponent<enemyBase>() != null)
                {
                    enemyBase currentEnemyController = enemyToAffect.GetComponent<enemyBase>();
                    Rigidbody currentEnemyRigidbody = enemyToAffect.GetComponent<Rigidbody>();
                    GameObject currentEnemy = enemyToAffect.gameObject;

                    //Determines the direction in which the black hole is in relation to the enemy
                    Vector3 direction = (transform.position - currentEnemy.transform.position).normalized;
                    Vector3 position = currentEnemy.transform.position;
                    Debug.DrawLine(position, position + direction * 10);

                    //Enables physics simulation of the enemy's rigidbody
                    if (currentEnemyRigidbody.isKinematic)
                    {
                        currentEnemyRigidbody.isKinematic = false;
                    }

                    //Damages the enemy and pulls them towards the center of the black hole
                    currentEnemyController.takeDamage(damageToDeal);
                    currentEnemyRigidbody.AddForce(direction * 10);
                }
            }
        }
    }

}
