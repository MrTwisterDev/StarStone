﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariantTwo : PlayerBase
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: CharacterVariantTwo                              |
    // Script Author: James Smale                                    |
    // Purpose: A class that inherits from the PlayerBase, used to   |
    //          implement override methods for unique abilities.     |
    //***************************************************************|

    public override void UseLeftAbility()
    {
        base.UseLeftAbility();
    }

    public override void UseRightAbility()
    {
        Quaternion knifeRotation = Quaternion.AngleAxis(-90f, Vector3.right) * transform.rotation;
        Instantiate(rightAbilityPrefab, cameraTransform.position, knifeRotation);
    }
}
