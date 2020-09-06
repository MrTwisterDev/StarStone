using System.Collections;
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

        [Header("Abilities")]
    public AudioClip knifeThrow;
    public int maxKnives;
    public int currentActiveKnives;

    public AudioClip teleportSound;
    public float teleportSpeed;
    public bool beaconActive;
    public bool teleportingPlayer;
    public Vector3 teleportBeacon;

    private void Awake()
    {
        maxKnives = 3;
        teleportingPlayer = false;
        if(teleportSpeed == 0) { teleportSpeed = 200f; }
    }

    public override void UseLeftAbility()
    {
        if (beaconActive && !teleportingPlayer && canUseLeftAbility)
        {
            teleportingPlayer = true;
            canUseLeftAbility = false;
            uIController.ToggleSpeedLines(true);
            AudioSource.PlayClipAtPoint(teleportSound, transform.position, 0.25f);
        }
        else if(!beaconActive)
        {
            TeleportBeacon beacon = Instantiate(leftAbilityPrefab, transform.position + transform.forward, Quaternion.identity).GetComponent<TeleportBeacon>();
            beacon.playerScript = this;
            beaconActive = true;
        }
    }

    private void Update()
    {
        PlayerStateHandler();
        if (teleportingPlayer)
        {
            //Determines the distance to move the player this step by multiplying the teleportSpeed by deltaTime to make it framerate independent
            float moveDist = teleportSpeed * Time.deltaTime;
            //Moves the player towards the teleport target by the previously calculated move distance
            transform.position = Vector3.MoveTowards(transform.position, teleportBeacon, moveDist);
            //If the player has essentially reached its target, the speedlines are disabled and the Blinkball destroyed
            if (Vector3.Distance(transform.position, teleportBeacon) < 1f)
            {
                uIController.ToggleSpeedLines(false);
                teleportingPlayer = false;
            }
        }
    }

    public override void UseRightAbility()
    {
        if (currentActiveKnives < maxKnives)
        {
            Quaternion knifeRot = gameObject.transform.rotation;
            ThrowingKnife thrownKnife = Instantiate(rightAbilityPrefab, cameraTransform.position, knifeRot).GetComponent<ThrowingKnife>();
            AudioSource.PlayClipAtPoint(knifeThrow, transform.position, 0.25f);
            currentActiveKnives++;
            thrownKnife.cameraTransform = cameraTransform;
            thrownKnife.playerScript = this;
            thrownKnife.uIController = uIController;
            thrownKnife.UpdateUI();
        }
    }
}
