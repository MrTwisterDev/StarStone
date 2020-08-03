using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedball : scr_Collectable
{
    public override void pickupCollectable(GameObject playerObject)
    {
        PlayerBase player = playerObject.GetComponent<PlayerBase>();
        player.moveSpeedMultiplier += player.speedBoostMultiplier;
        player.hasSpeedBoost = true;
    }
}
