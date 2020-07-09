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
    [Header("Flamethrower Stats")]
    [Tooltip("The amount of charge the minigun uses per shot.")]
    public float flamethrowerChargeUsage;
    [Space]
    #endregion

    #region
    [Header("Singularity Stats")]
    [Tooltip("The amount of charge the singularity mode uses per shot.")]
    public float singularityChargeUsage;
    [Space]
    #endregion

    private PlayerController playerController;

    [Tooltip("The layer on which the enemies exist.")]
    public LayerMask enemyLayer;

    public enum weaponModes
    {
        minigunMode,
        flamethrowerMode,
        vampireMode,
        singularityMode
    }

    [Space]
    [Tooltip("The current mode the prototype weapon is in.")]
    public weaponModes currentWeaponMode;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();

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
                case weaponModes.flamethrowerMode:
                    //flamethrower code
                    break;
                case weaponModes.vampireMode:
                    FireVampireMode();
                    break;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(currentWeaponMode == weaponModes.singularityMode)
            {
                //singularity code
            }
        }
        //Placeholder mode-switching code. Will not be used in final prototype.\\
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponMode = weaponModes.minigunMode;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponMode = weaponModes.flamethrowerMode;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponMode = weaponModes.vampireMode;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeaponMode = weaponModes.singularityMode;
        }
    }
}
