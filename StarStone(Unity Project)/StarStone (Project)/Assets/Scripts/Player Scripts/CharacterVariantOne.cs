using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariantOne : PlayerBase
{

    public override void UseLeftAbility()
    {
        BlinkBallController blinkBall = Instantiate(leftAbilityPrefab, cameraTransform.position, Quaternion.identity).GetComponent<BlinkBallController>();
        blinkBall.playerObject = gameObject;
        canUseLeftAbility = false;
    }

    public override void UseRightAbility()
    {
        Instantiate(rightAbilityPrefab, cameraTransform.position, Quaternion.identity);
    }

}
