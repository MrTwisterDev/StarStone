using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRound : PrototypeProjectileBase
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        if (collision.gameObject.tag != "Prototype")
        {
            Debug.Log("Wasn't the weapon!");
            Collider[] enemiesAffected = Physics.OverlapSphere(transform.position, areaOfEffect);
            foreach (Collider enemyCollider in enemiesAffected)
            {
                if (enemyCollider.GetComponent<enemyBase>() != null)
                {
                    enemyBase enemyToDamage = enemyCollider.GetComponent<enemyBase>();
                    if (!enemyToDamage.isBurning)
                    {
                        enemyToDamage.isBurning = true;
                    }
                    else
                    {
                        enemyToDamage.burnTimer = enemyToDamage.burnTime;
                    }
                    enemyToDamage.takeDamage(damageToDeal);
                }
            }
            AudioSource.PlayClipAtPoint(detonationSound, transform.position);
            Destroy(gameObject);
        }
    }
}
