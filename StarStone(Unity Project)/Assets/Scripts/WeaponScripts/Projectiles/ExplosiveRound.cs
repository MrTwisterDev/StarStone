using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRound : PrototypeProjectileBase
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: ExplosiveRound                                   |
    // Script Author: James Smale                                    |
    // Purpose: Handles the effects of the explosive round fired from|
    //          the prototype weapon.
    //***************************************************************|

    private void OnCollisionEnter(Collision collision)
    {
        //Checks that the collision isn't the prototype weapon so that the grenade doesn't explode immediately after being fired
        if (collision.gameObject.tag != "Prototype")
        {
            //Creates a sphere of effect with a range dictated by the areaOfEffect variable
            Collider[] enemiesAffected = Physics.OverlapSphere(transform.position, areaOfEffect);
            //iterates through all of the colliders in the array created by the overlap sphere and checks that they have enemy scripts attached
            foreach (Collider enemyCollider in enemiesAffected)
            {   
                //If it is an enemy, the enemy is set to take burning damage
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
                    //The enemy also takes explosion damage
                    enemyToDamage.takeDamage(damageToDeal);
                }
            }
            //An explosion sound plays, and the grenade is destroyed
            AudioSource.PlayClipAtPoint(detonationSound, transform.position);
            Destroy(gameObject);
        }
    }
}
