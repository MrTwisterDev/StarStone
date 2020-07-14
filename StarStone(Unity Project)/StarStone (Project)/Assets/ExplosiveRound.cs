﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRound : MonoBehaviour
{

    private Rigidbody rigidBody;
    private Transform cameraTransform;

    public float areaOfEffect;
    public float launchForce;
    public float explosionDamage;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        rigidBody.AddForce(cameraTransform.forward * launchForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Prototype")
        {
            Collider[] enemiesAffected = Physics.OverlapSphere(transform.position, areaOfEffect);
            foreach (Collider enemyCollider in enemiesAffected)
            {
                if (enemyCollider.GetComponent<enemyBase>() != null)
                {
                    enemyBase enemyToDamage = enemyCollider.GetComponent<enemyBase>();
                    enemyToDamage.isBurning = true;
                    enemyToDamage.takeDamage(explosionDamage);
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }

}
