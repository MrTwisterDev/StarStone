using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariantOne : PlayerBase
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: PlayerBase                                       |
    // Script Author: James Smale                                    |
    // Purpose: A class that inherits from the PlayerBase, used to   |
    //          implement override methods for unique abilities.     |
    //***************************************************************|

    [Header("Abilities")]
    [Tooltip("The maximum number of active mines this character can have.")]
    public int maxActiveMines;
    [Tooltip("The number of mines this character has currenty placed.")]
    public int currentActiveMines;

    public GameObject pixelMode;
    public void Awake()
    {
        //Sets the maximum number of mines to 3 if it is left undefined
        if (maxActiveMines == 0) { maxActiveMines = 3; }
    }

    public override void UseLeftAbility()
    {
        //Instantiates a new blinkBall prefab and finds its attached script to assign it to a local variable
        BlinkBallController blinkBall = Instantiate(leftAbilityPrefab, cameraTransform.position, Quaternion.identity).GetComponent<BlinkBallController>();
        //Assigns this gameObject to the new blinkBall so it can change its position
        blinkBall.playerObject = gameObject;
        //Disables the player's ability so that the cooldown timer can run
        canUseLeftAbility = false;
    }

    public override void UseRightAbility()
    {
        //Checks that the player's number of active mines is less than the maximum before creating a new mine
        if (currentActiveMines < maxActiveMines)
        {
            //Instantiates a new mine prefab and finds its attached script to assign it to a local variable
            mineScript newMine = Instantiate(rightAbilityPrefab, cameraTransform.position, Quaternion.identity).GetComponent<mineScript>();
            //Assigns this instance of this script to the mine's playerScript variable
            newMine.playerScript = this;
            //Assigns the UIController script to the mine so it can affect the appropriate UI
            newMine.uiController = gameObject.GetComponent<UIController>();
            //Increments the number of current active mines
            currentActiveMines++;
        }
        //If the player can't throw down any more mines, an indicative sound is played
        else
        {
            AudioSource.PlayClipAtPoint(actionFailed, transform.position);
        }
    }

    private void Update()
    {
        PlayerStateHandler();
    }

}
