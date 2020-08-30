using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : PrototypeProjectileBase
{

    public AudioClip knifeHitEnvironment;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponentInParent<PlayerBase>() == null)
        {
            if (collision.collider.gameObject.GetComponent<enemyBase>() != null)
            {
                AudioSource.PlayClipAtPoint(detonationSound, transform.position, 0.25f);
                collision.collider.gameObject.GetComponent<enemyBase>().takeDamage(damageToDeal);
            }
            else
            {
                AudioSource.PlayClipAtPoint(knifeHitEnvironment, transform.position, 0.25f);
            }
            transform.parent = collision.collider.gameObject.transform;
            rigidBody.isKinematic = true;
        }
    }
}
