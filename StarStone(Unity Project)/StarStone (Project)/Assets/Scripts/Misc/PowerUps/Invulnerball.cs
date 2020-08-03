using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerball : scr_Collectable
{   
    //Makes the player that picked up the item invulnerable for a short time
    public override void pickupCollectable(GameObject playerObject)
    {
        playerObject.GetComponent<PlayerBase>().isInvulnerable = true;
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Destroy(gameObject);
    }

}
