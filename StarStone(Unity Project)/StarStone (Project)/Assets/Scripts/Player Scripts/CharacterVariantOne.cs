using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariantOne : PlayerBase
{

    public override void UseLeftAbility()
    {
        Instantiate(leftAbilityPrefab, cameraTransform.position, Quaternion.identity);
        canUseLeftAbility = false;
    }

    public override void UseRightAbility()
    {
        Instantiate(rightAbilityPrefab, cameraTransform.position, Quaternion.identity);
    }

}
