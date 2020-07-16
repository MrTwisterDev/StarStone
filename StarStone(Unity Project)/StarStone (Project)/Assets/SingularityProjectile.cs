using System.Collections;
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
            Destroy(gameObject);
        }
        else
        {
            Collider[] enemiesAffected = Physics.OverlapSphere(transform.position, areaOfEffect);
            foreach(Collider enemyToAffect in enemiesAffected)
            {
                if(enemyToAffect.GetComponent<enemyBase>() != null)
                {
                    enemyBase currentEnemyController = enemyToAffect.GetComponent<enemyBase>();
                    Rigidbody currentEnemyRigidbody = enemyToAffect.GetComponent<Rigidbody>();
                    GameObject currentEnemy = enemyToAffect.gameObject;

                    Vector3 direction = (transform.position - currentEnemy.transform.position).normalized;
                    Vector3 position = currentEnemy.transform.position;
                    Debug.DrawLine(position, position + direction * 10);

                    currentEnemyController.takeDamage(damageToDeal);
                    currentEnemyRigidbody.AddForce(direction * 10);
                }
            }
        }
    }

}
