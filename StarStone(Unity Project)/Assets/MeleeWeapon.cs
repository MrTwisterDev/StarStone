using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float meleeDamage;
    private bool detectHits;

    public void ToggleHitDetection()
    {
        detectHits = !detectHits;
    }

    void OnTriggerEnter(Collider enemyCollider)
    {
        if(detectHits && enemyCollider.GetComponent<enemyBase>() != null)
        {
            enemyCollider.GetComponent<enemyBase>().takeDamage(meleeDamage);
        }
    }

}
