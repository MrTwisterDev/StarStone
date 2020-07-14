using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeWeapon : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: PrototypeWeapon                                  |
    // Script Author: James Smale                                    |
    // Purpose: Manages the different fire modes and charge level of |
    //          the game's prototype weapon                          |
    //***************************************************************|
    #region
    [Header("Weapon Charge Variables")]
    [Tooltip("The amount of charge currently in the prototype weapon.")]
    public float weaponCharge;
    [Tooltip("The multiplier used to calculate the rate at which the weapon recharges.")]
    public float weaponRechargeMultiplier;
    [Space]
    #endregion

    #region
    [Header("Minigun Stats")]
    [Tooltip("The range at which the minigun mode can fire.")]
    public float minigunRange;
    [Tooltip("The amount of charge the minigun uses per shot.")]
    public float minigunChargeUsage;
    [Tooltip("The amount of damage the minigun does per shot.")]
    public float minigunDamage;
    [Space]
    #endregion

    #region
    [Header("Vampire Stats")]
    [Tooltip("The range at which the vampire mode can fire.")]
    public float vampireRange;
    [Tooltip("The amount of charge the vampire mode uses per shot.")]
    public float vampireChargeUsage;
    [Tooltip("The amount of damage the vampire mode does per shot.")]
    public float vampireDamage;
    [Tooltip("The amount of health the player regains upon hittin an enemy with the vampire mode.")]
    public float vampireDrain;
    [Space]
    #endregion

    #region
    [Header("Grenade Launcher Stats")]
    [Tooltip("The amount of charge the grenade launcher uses per shot.")]
    public float grenadeLauncherChargeUsage;
    public GameObject grenadeProjectile;
    [Space]
    #endregion

    #region
    [Header("Singularity Stats")]
    [Tooltip("The amount of charge the singularity mode uses per shot.")]
    public float singularityChargeUsage;
    [Space]
    #endregion

    private PlayerController playerController;
    private UIController uIController;
    private StarstoneController starstoneToChargeFrom;

    public Transform muzzleTransform;
    public float chargeRange;
    public LayerMask starstoneLayer;

    [Tooltip("The layer on which the enemies exist.")]
    public LayerMask enemyLayer;

    public Color speedColour;
    public Color healthColour;
    public Color fireColour;
    public Color singularityColor;

    public enum weaponModes
    {
        minigunMode,
        grenadeLauncherMode,
        vampireMode,
        singularityMode
    }

    [Space]
    [Tooltip("The current mode the prototype weapon is in.")]
    public weaponModes currentWeaponMode;
    private weaponModes newWeaponMode;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();

        speedColour = Color.cyan;
        healthColour = Color.green;
        fireColour = Color.red;
        singularityColor = Color.magenta;

        uIController.UpdatePrototypeSliderColour(speedColour);

        newWeaponMode = currentWeaponMode;

        weaponCharge = 100;
    }

    public void FireMinigunMode()
    {
        //If the weapon's charge minus the discharge rate is greater than or equal to 0, a raycast is sent out
        if (weaponCharge - minigunChargeUsage >= 0)
        {
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * minigunRange, Color.blue, 1);
            //If the raycast hits an enemy, the enemy takes damage
            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out rayHit, minigunRange, enemyLayer))
            {
                Debug.Log("Hit an enemy!");
                rayHit.collider.gameObject.GetComponent<enemyBase>().takeDamage(minigunDamage);
            }
            //The weapon's charge is reduced by the amount of charge the current mode uses
            weaponCharge -= minigunChargeUsage;
        }
    }

    public void FireVampireMode()
    {
        if (weaponCharge - vampireChargeUsage >= 0)
        {
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * vampireRange, Color.green, 1);
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, vampireRange, enemyLayer))
            {
                rayHit.collider.gameObject.GetComponent<enemyBase>().takeDamage(vampireDamage);
                //Increases the player's health by the value of vampireDrain
                playerController.currentHealth += vampireDrain;
            }
            weaponCharge -= vampireChargeUsage;
        }
    }

    public void FireGrenade()
    {
        if (weaponCharge - grenadeLauncherChargeUsage >= 0)
        {
            Instantiate(grenadeProjectile, muzzleTransform.position, Quaternion.identity);
            weaponCharge -= grenadeLauncherChargeUsage;
        }
    }

    public bool IsAimingAtStarstone()
    {
        RaycastHit rayHit;
        if(Physics.Raycast(muzzleTransform.position, transform.forward, out rayHit, chargeRange, starstoneLayer))
        {
            if (rayHit.collider.gameObject.tag == "Starstone")
            {
                starstoneToChargeFrom = rayHit.collider.gameObject.GetComponent<StarstoneController>();
                switch (starstoneToChargeFrom.starstoneType)
                {
                    case StarstoneController.starstoneTypes.speedStarstone:
                        newWeaponMode = weaponModes.minigunMode;
                        break;
                    case StarstoneController.starstoneTypes.healthStarstone:
                        newWeaponMode = weaponModes.vampireMode;
                        break;
                    case StarstoneController.starstoneTypes.fireStarstone:
                        newWeaponMode = weaponModes.grenadeLauncherMode;
                        break;
                    case StarstoneController.starstoneTypes.buffStarstone:
                        newWeaponMode = weaponModes.singularityMode;
                        break;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            switch (currentWeaponMode)
            {
                case weaponModes.minigunMode:
                    FireMinigunMode();
                    break;
                case weaponModes.vampireMode:
                    FireVampireMode();
                    break;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentWeaponMode)
            {
                case weaponModes.grenadeLauncherMode:
                    FireGrenade();
                    break;
                case weaponModes.singularityMode:
                    //singularity code here
                    break;
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (IsAimingAtStarstone())
            {
                if (newWeaponMode != currentWeaponMode)
                {
                    switch (newWeaponMode)
                    {
                        case weaponModes.minigunMode:
                            currentWeaponMode = newWeaponMode;
                            uIController.UpdatePrototypeSliderColour(speedColour);
                            weaponCharge = 0f;
                            break;
                        case weaponModes.vampireMode:
                            currentWeaponMode = newWeaponMode;
                            uIController.UpdatePrototypeSliderColour(healthColour);
                            weaponCharge = 0f;
                            break;
                        case weaponModes.grenadeLauncherMode:
                            currentWeaponMode = newWeaponMode;
                            uIController.UpdatePrototypeSliderColour(fireColour);
                            weaponCharge = 0f;
                            break;
                        case weaponModes.singularityMode:
                            currentWeaponMode = newWeaponMode;
                            uIController.UpdatePrototypeSliderColour(singularityColor);
                            weaponCharge = 0f;
                            break;
                    }
                }
                else if(weaponCharge < 100 && starstoneToChargeFrom.starstoneCharge - Time.deltaTime * weaponRechargeMultiplier >= 0)
                {
                    weaponCharge += Time.deltaTime * weaponRechargeMultiplier;
                    starstoneToChargeFrom.starstoneCharge -= Time.deltaTime * weaponRechargeMultiplier;
                    if(weaponCharge > 100)
                    {
                        weaponCharge = 100;
                    }
                }
            }
        }

        uIController.UpdatePrototypeCharge((int)weaponCharge);

    }
}
