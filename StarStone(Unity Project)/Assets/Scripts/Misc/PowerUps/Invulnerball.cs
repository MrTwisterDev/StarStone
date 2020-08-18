using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invulnerball : scr_Collectable
{

    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Invulneraball                                    |
    // Script Author: James Smale                                    |
    // Purpose: Manages the different fire modes and charge level of |
    //          the game's prototype weapon                          |
    //***************************************************************|

    private void Start()
    {
        Invoke("destoryMe", 10f);
    }


    public override void pickupCollectable(GameObject playerObject)
    {
        UIController.isImmune = true; // Lewis' work 
        //Finds and assigns the PlayerBase of the player that collided with the object and sets its invulnerability boolean to true
        playerObject.GetComponent<PlayerBase>().isInvulnerable = true;
        //Plays the object's powerup sound and destroys the object
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Destroy(gameObject);
        
    }

    public void destoryMe()
    {
        Destroy(gameObject);
    }


}
