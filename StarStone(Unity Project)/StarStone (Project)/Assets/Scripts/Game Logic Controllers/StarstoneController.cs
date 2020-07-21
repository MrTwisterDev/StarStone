using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarstoneController : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Starstone Controller                             |
    // Script Author: James Smale                                    |
    // Purpose: Controls the active effects of the starstones, as    |
    //          well as their charge level                           |
    //***************************************************************|

    public GameController gameController;
    public GameObject playerObject;

    [Tooltip("Boolean that determines whether or not the selected Starstone is active.")]
    public bool isActiveStarstone;
    [Space]

    [Header("Starstone Charge Variables")]
    [Tooltip("The current charge level of the selected Starstone.")]
    public float starstoneCharge;
    [Tooltip("The multiplier used to calculate the discharge rate of the selected Starstone.")]
    public float dischargeMultiplier;
    [Tooltip("The multiplier used to calculate the recharge rate of the selected Starstone.")]
    public float rechargeMultiplier;

    public enum starstoneTypes
    {
        speedStarstone,
        fireStarstone,
        healthStarstone,
        buffStarstone
    }

    public starstoneTypes starstoneType;

    public void Start()
    {
        if (gameController == null) { gameController = GameObject.Find("GameController").GetComponent<GameController>(); }
        playerObject = GameObject.FindGameObjectWithTag("Player");
        starstoneCharge = 100f;

        if(dischargeMultiplier == 0) { dischargeMultiplier = 1; }
        if(rechargeMultiplier == 0) { rechargeMultiplier = 1; }
    }

    public void Update()
    {
        //If the startsone the script is attached to is the active starstone, its charge is drained each frame
        if (isActiveStarstone)
        {
            starstoneCharge -= Time.deltaTime * dischargeMultiplier;
            //If the charge reaches 0, the starstone is deactivated, its charge is set to 0, and the Game Controller chooses the next highest charged starstone to activate
            if(starstoneCharge <= 0)
            {
                isActiveStarstone = false;
                starstoneCharge = 0;
                gameController.ActivateNewStarstone();
            }
        }
        //If the starstone is not active, and its charge is less than 100, it regains charge every frame until it is at 100% charge
        else if(starstoneCharge < 100f)
        {
            starstoneCharge += Time.deltaTime * rechargeMultiplier;
            if(starstoneCharge > 100f)
            {
                starstoneCharge = 100f;
            }
        }
        if(Vector3.Distance(gameObject.transform.position, playerObject.transform.position) <= 5f && !gameController.hasFoundGenerator)
        {
            gameController.hasFoundGenerator = true;
        }

    }

    public void ActivateEffect()
    {
        //Sets the starstone as active and buffs all of the enemies in the scene
        isActiveStarstone = true;
        if(gameController == null) { gameController = GameObject.Find("GameController").GetComponent<GameController>(); }
        switch (starstoneType)
        {
            case starstoneTypes.speedStarstone:
                gameController.currentStarstone = GameController.starstoneEffects.speedEffect;
                break;
            case starstoneTypes.healthStarstone:
                gameController.currentStarstone = GameController.starstoneEffects.healthEffect;
                break;
            case starstoneTypes.fireStarstone:
                gameController.currentStarstone = GameController.starstoneEffects.fireEffect;
                break;
            case starstoneTypes.buffStarstone:
                gameController.currentStarstone = GameController.starstoneEffects.buffEffect;
                break;
        }
        gameController.BuffEnemies();
    }

}
