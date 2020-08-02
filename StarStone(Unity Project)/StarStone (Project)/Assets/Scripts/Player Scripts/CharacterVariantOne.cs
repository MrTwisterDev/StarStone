using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariantOne : PlayerBase
{

    public int maxActiveMines;
    public int currentActiveMines;

    public void Awake()
    {
        maxActiveMines = 3;
    }

    public override void UseLeftAbility()
    {
        BlinkBallController blinkBall = Instantiate(leftAbilityPrefab, cameraTransform.position, Quaternion.identity).GetComponent<BlinkBallController>();
        blinkBall.playerObject = gameObject;
        canUseLeftAbility = false;
    }

    public override void UseRightAbility()
    {
        if (currentActiveMines < maxActiveMines)
        {
            mineScript newMine = Instantiate(rightAbilityPrefab, cameraTransform.position, Quaternion.identity).GetComponent<mineScript>();
            newMine.playerScript = this;
            newMine.uiController = gameObject.GetComponent<UIController>();
            currentActiveMines++;
            newMine.GetComponent<mineScript>().mineNumber = currentActiveMines;
        }
        else
        {
            AudioSource.PlayClipAtPoint(actionFailed, transform.position);
        }
    }

}
