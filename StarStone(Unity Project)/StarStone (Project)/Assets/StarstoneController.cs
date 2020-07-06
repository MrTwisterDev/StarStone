﻿using System.Collections;
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

    private GameController gameController;
    private bool isActiveStarstone;

    private float starstoneCharge;

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
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        starstoneCharge = 100f;
    }

    public void Update()
    {
        //If the startsone the script is attached to is the active starstone, its charge is drained each frame
        if (isActiveStarstone)
        {
            starstoneCharge -= Time.deltaTime;
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
            starstoneCharge += Time.deltaTime;
            if(starstoneCharge > 100f)
            {
                starstoneCharge = 100f;
            }
        }
    }

    public void ActivateEffect()
    {
        //Sets the starstone as active and buffs all of the enemies in the scene
        isActiveStarstone = true;
        gameController.BuffEnemies((int)starstoneType);
    }

}
