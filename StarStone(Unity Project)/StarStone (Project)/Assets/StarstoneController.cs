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
        if (isActiveStarstone)
        {
            starstoneCharge -= Time.deltaTime;
            if(starstoneCharge <= 0)
            {
                isActiveStarstone = false;
                starstoneCharge = 0;
                gameController.ActivateNewStarstone();
            }
        }
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
        Debug.Log(starstoneType + "active!");
        isActiveStarstone = true;
        gameController.BuffEnemies((int)starstoneType);
    }

}
