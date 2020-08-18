using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedball : scr_Collectable
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Speedball                                        |
    // Script Author: James Smale                                    |
    // Purpose: Handles the effects of the Speedball powerup         |
    //***************************************************************|
    private void Start()
    {
        Invoke("destroyMe", 10f);
    }
    public override void pickupCollectable(GameObject playerObject)
    {

        UIController.isFast = true; // Lewis' work
        //Finds and assigns the PlayerBase of the character that collided with the object
        PlayerBase player = playerObject.GetComponent<PlayerBase>();
        if (!player.hasSpeedBoost)
        {
            //Adds the speedboost value to the player's speed multiplier and sets the speedBoost boolean to true in the player
            player.moveSpeedMultiplier += player.speedBoostMultiplier;
            player.hasSpeedBoost = true;
        }
        else
        {
            //if the player already has the speed boost, the timer for it is reset
            player.speedBoostTimer = player.speedBoostDuration;
        }
        Destroy(gameObject);
    }

    public void destoryMe()
    {
        Destroy(gameObject);
    }

}



