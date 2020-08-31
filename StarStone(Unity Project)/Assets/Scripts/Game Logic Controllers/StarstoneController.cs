using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarstoneController : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Starstone Controller                             |
    // Script Author: James Smale +Thomas Lang (Co-op Implementation)|
    // Purpose: Controls the active effects of the starstones, as    |
    //          well as their charge level                           |
    //***************************************************************|

    //External Scripts
    #region
    public GameController gameController;
    public UIController[] uIController;
    #endregion
    //GameObjects
    #region
    public GameObject[] playerObjects;

    [SerializeField]
    private GameObject interactionBand, disconnectionBand;

    #endregion
    //Starstone Charge Variables
    #region
    [Header("Starstone Charge Variables")]
    [Tooltip("The current charge level of the selected Starstone.")]
    public float starstoneCharge;
    [Tooltip("The multiplier used to calculate the discharge rate of the selected Starstone.")]
    public float dischargeMultiplier;
    [Tooltip("The multiplier used to calculate the recharge rate of the selected Starstone.")]
    public float rechargeMultiplier;
    [Tooltip("Boolean that determines whether or not the selected Starstone is active.")]
    public bool isActiveStarstone;
    [Tooltip("Is the generator enabled?")]
    public bool genEnabled; //Thomas
    [Tooltip("This generator has been selected for disconnection")]
    public selectionType selectedGenerator; //Thomas
    [Tooltip("The type this Starstone is.")]
    public starstoneTypes starstoneType;
    #endregion

    public enum selectionType
    {
        notSelected,
        isSelected,
        disconnecting
    }
    public enum starstoneTypes
    {
        speedStarstone,
        fireStarstone,
        healthStarstone,
        buffStarstone
    }

    public void Start()
    {
        if (gameController == null) { gameController = GameObject.Find("GameController").GetComponent<GameController>(); }
        uIController = gameController.uIController;
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
        starstoneCharge = 100f;

        if(dischargeMultiplier == 0) { dischargeMultiplier = 1; }
        if(rechargeMultiplier == 0) { rechargeMultiplier = 1; }
    }

    public void Update()
    {
        switch (this.selectedGenerator)
        {
            case selectionType.isSelected: interactionBand.SetActive(true);
                disconnectionBand.SetActive(false);
                break;
            case selectionType.disconnecting: disconnectionBand.SetActive(true);
                interactionBand.SetActive(false);
                break;
            default:
                disconnectionBand.SetActive(false);
                interactionBand.SetActive(false);
                break;

      
        }


        //If the startsone the script is attached to is the active starstone, its charge is drained each frame
        if (isActiveStarstone && genEnabled)
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
        else if(starstoneCharge < 100f && genEnabled)
        {
            starstoneCharge += Time.deltaTime * rechargeMultiplier;
            if(starstoneCharge > 100f)
            {
                starstoneCharge = 100f;
            }
        }
        //Checks to see if Player One has gotten close enough for a generator for the game to start
        if (!gameController.hasFoundGenerator)
        {
            for (int i = 0; i < playerObjects.Length; i++)
            {
                if (Vector3.Distance(playerObjects[i].transform.position, gameObject.transform.position) <= 5f)
                {
                    gameController.hasFoundGenerator = true;
                }
            }
        }
        UpdateStoneUI();
    }

    public void UpdateStoneUI()
    {
        switch (starstoneType)
        {   //Updates the UI Slider relating to this Starstone with its latest charge value
            case starstoneTypes.speedStarstone:
                foreach (var controller in uIController)
                {
                    controller.UpdateSpeedCharge(starstoneCharge);
                }
                break;
            case starstoneTypes.healthStarstone:
                foreach (var controller in uIController)
                {
                    controller.UpdateHealthCharge(starstoneCharge);
                }
                break;
            case starstoneTypes.fireStarstone:
                foreach (var controller in uIController)
                {
                    controller.UpdateFireCharge(starstoneCharge);
                }
                break;
            case starstoneTypes.buffStarstone:
                foreach (var controller in uIController)
                {
                    controller.UpdateSingularityCharge(starstoneCharge);
                }
                break;
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

    public void chargePrototypeWeapon(PrototypeWeapon.weaponModes typeToCharge) //Depricated
    {
        if(genEnabled == true)
        {
            foreach (var player in playerObjects)
            {
                if(player.GetComponent<PlayerBase>().activeWeapon.GetComponent<PrototypeWeapon>() != null)
                {
                    PrototypeWeapon proto = player.GetComponent<PlayerBase>().activeWeapon.GetComponent<PrototypeWeapon>();
                   
                }
            }
        }
    }

}
