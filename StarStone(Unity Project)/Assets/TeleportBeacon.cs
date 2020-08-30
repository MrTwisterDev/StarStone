using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBeacon : MonoBehaviour
{

    public CharacterVariantTwo playerScript;

    public Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            playerScript.teleportBeacon = transform.position;
            rigidBody.isKinematic = true;
        }
    }

    private void OnDestroy()
    {
        playerScript.beaconActive = false;
        playerScript.leftAbilityCooldown = 0f;
    }

}
