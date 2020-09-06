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

    //Weapon Charge Variables
    #region
    [Header("Weapon Charge Variables")]
    [Tooltip("The amount of charge currently in the prototype weapon.")]
    public float weaponCharge;
    [Tooltip("The multiplier used to calculate the rate at which the weapon recharges.")]
    public float weaponRechargeMultiplier;
    [Tooltip("The range from which the weapon can recharge.")]
    public float chargeRange;
    [Space]
    #endregion
    //Minigun Mode Stats
    #region
    [Header("Minigun Stats")]
    [Tooltip("The range at which the minigun mode can fire.")]
    public float minigunRange;
    [Tooltip("The amount of charge the minigun uses per shot.")]
    public float minigunChargeUsage;
    [Tooltip("The amount of damage the minigun does per shot.")]
    public float minigunDamage;
    public AudioClip minigunSound;
    [Space]
    #endregion
    //Vampire Mode Stats
    #region
    [Header("Vampire Stats")]
    [Tooltip("The range at which the vampire mode can fire.")]
    public float vampireRange;
    [Tooltip("The amount of charge the vampire mode uses per shot.")]
    public float vampireChargeUsage;
    [Tooltip("The amount of damage the vampire mode does per shot.")]
    public float vampireDamage;
    [Tooltip("The amount of health the player regains upon hittin an enemy with the vampire mode.")]
    public int vampireDrain;
    public AudioClip vampireSound;
    [Space]
    #endregion
    //Grenade Launcher Mode Stats
    #region
    [Header("Grenade Launcher Stats")]
    [Tooltip("The amount of charge the grenade launcher uses per shot.")]
    public float grenadeLauncherChargeUsage;
    public GameObject grenadeProjectile;
    public AudioClip grenadeLaunch;
    [Space]
    #endregion
    //Singularity Mode Stats
    #region
    [Header("Singularity Stats")]
    [Tooltip("The amount of charge the singularity mode uses per shot.")]
    public float singularityChargeUsage;
    public GameObject singularityProjectile;
    public AudioClip singularityLaunch;
    [Space]
    #endregion
    //Weapon Firing
    #region
    [Header("Weapon Firing")]
    [Header("Layer Masks")]
    [Tooltip("The physics layer of the Starstones.")]
    public LayerMask starstoneLayer;
    [Tooltip("The layer on which the enemies exist.")]
    public LayerMask enemyLayer;
    [Space]
    [Header("Audio")]
    [Tooltip("The audio source played when the weapon fires.")]
    public AudioSource weaponSound;
    [Space]
    [Tooltip("The transform placed at the end of the weapon.")]
    public Transform muzzleTransform;
    [Space]
    [Tooltip("The current mode the prototype weapon is in.")]
    public weaponModes currentWeaponMode;
    private weaponModes newWeaponMode;
    public AudioClip lowPower;
    [HideInInspector]
    public bool singleShotLock;
    #endregion
    //UI Colours
    #region
    [Header("UI Colours")]
    [Tooltip("The colour the Prototype Weapon's charge bar will turn when it is in minigun mode.")]
    public Color speedColour;
    [Tooltip("The colour the Prototype Weapon's charge bar will turn when it is in vampire mode.")]
    public Color healthColour;                                                     
    [Tooltip("The colour the Prototype Weapon's charge bar will turn when it is in grenade launcher mode.")]
    public Color fireColour;                                                       
    [Tooltip("The colour the Prototype Weapon's charge bar will turn when it is in singularity mode.")]
    public Color singularityColor;
    [Space]
    #endregion
    //Muzzleflash Colours
    #region
    [Header("Muzzle Flash Colours")]
    public Color minigunColour;
    public Color vampireColour;
    public Color grenadeColour;
    public Color singularityColour;
    public Light muzzleFlash;
    private float flashCooldown;
    #endregion
    //External Scripts
    #region
    private PlayerBase playerController;
    public UIController uIController;
    private StarstoneController starstoneToChargeFrom;
    #endregion

    public enum weaponModes
    {
        minigunMode,
        grenadeLauncherMode,
        vampireMode,
        singularityMode
    }

    // Start is called before the first frame update
    void Start()
    {
        //Finds the player and UI controller scripts
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
        uIController = gameObject.GetComponentInParent<UIController>();
        //Assigns the colours used for the prototype weapon UI slider
        speedColour = Color.cyan;
        healthColour = Color.green;
        fireColour = Color.red;
        singularityColor = Color.magenta;
        //Sets the current mode of the weapon to be minigun mode
        currentWeaponMode = weaponModes.minigunMode;
        //Gets the Audiosource attached to the weapon and assigns it to weaponSound, and sets the clip as the minigun sound
        weaponSound = gameObject.GetComponent<AudioSource>();
        weaponSound.clip = minigunSound;
        //Sets the UI Slider colour to the speed colour
        uIController.UpdatePrototypeSliderColour(speedColour);
        //Sets the newWeaponMode to be the same as the currentWeaponMode so it can change modes in the future
        newWeaponMode = currentWeaponMode;
        //Sets the weapon's charge to its maximum
        weaponCharge = 100;
        flashCooldown = 0.25f;
    }

    private void OnEnable()
    {
        uIController.TogglePrototypeUI(true);
    }

    private void OnDisable()
    {
        uIController.TogglePrototypeUI(false);
    }

    private void Update()
    {
        if (muzzleFlash.gameObject.activeSelf)
        {
            flashCooldown -= Time.deltaTime;
            if(flashCooldown <= 0)
            {
                muzzleFlash.gameObject.SetActive(false);
                flashCooldown = 0.25f;
            }
        }
    }

    public void FireMinigunMode()
    {
        //If the weapon's charge minus the discharge rate is greater than or equal to 0, a raycast is sent out
        if (weaponCharge - minigunChargeUsage >= 0)
        {
            muzzleFlash.gameObject.SetActive(true);
            if (!weaponSound.isPlaying)
            {
                weaponSound.Play(); 
            }
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * minigunRange, Color.blue, 1);
            //If the raycast hits an enemy, the enemy takes damage
            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out rayHit, minigunRange, enemyLayer))
            {
                enemyBase.shutUp = true; // enemies won't make sound when taking damage (prevents earspam)
                rayHit.collider.gameObject.GetComponent<enemyBase>().takeDamage(minigunDamage);
            }
            //The weapon's charge is reduced by the amount of charge the current mode uses
            weaponCharge -= minigunChargeUsage;
            if(weaponCharge <= 0)
            {
                weaponSound.Stop();
            }
        }
        else
        {
            AudioSource.PlayClipAtPoint(lowPower, transform.position, 1);
        }
    }

    public void FireVampireMode()
    {
        //If the weapon's charge minus the discharge rate is greater than or equal to 0, then a raycast is sent out
        if (weaponCharge - vampireChargeUsage >= 0 && !singleShotLock)
        {
            muzzleFlash.gameObject.SetActive(true);
            //The sound of the weapon siring is played
            weaponSound.Play();
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * vampireRange, Color.green, 1);
            //If the raycast hits an enemy, it takes damage equal to the value of vampireDamage
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, vampireRange, enemyLayer))
            {
                rayHit.collider.gameObject.GetComponent<enemyBase>().takeDamage(vampireDamage);
                //Increases the player's health by the value of vampireDrain
                playerController.RestoreHealth(vampireDrain);
            }
            //The weapon's charge is reduced by the set value
            weaponCharge -= vampireChargeUsage;
            //Locks the weapon from firing while the fire button is held
            singleShotLock = true;
        }
        else if(!singleShotLock)
        {
            AudioSource.PlayClipAtPoint(lowPower, transform.position, 1);
        }
    }

    public void FireGrenade()
    {
        //If the weapon's charge minus the discharge rate is greater than or equal to 0, then a projectile is launched
        if (weaponCharge - grenadeLauncherChargeUsage >= 0 && !singleShotLock)
        {
            muzzleFlash.gameObject.SetActive(true);
            //The sound of the weapon firing is played
            weaponSound.Play();
            //The grenade projectile is instantiated at the end of the weapon, and has force applied to it in its start function
            ExplosiveRound grenade = Instantiate(grenadeProjectile, muzzleTransform.position, Quaternion.identity).GetComponent<ExplosiveRound>();
            grenade.cameraTransform = playerController.cameraTransform;
            //The weapon's charge is reduced by the set value
            weaponCharge -= grenadeLauncherChargeUsage;
            singleShotLock = true;
        }
        else if(!singleShotLock)
        {
            AudioSource.PlayClipAtPoint(lowPower, transform.position, 1);
        }
    }

    public void FireSingularity()
    {
        //If the weapon's charge minus the discharge rate is greater than or equal to 0, then a projectile is fired
        if (weaponCharge - singularityChargeUsage >= 0 && !singleShotLock)
        {
            muzzleFlash.gameObject.SetActive(true);
            //The sound of the weapon firing is played
            weaponSound.Play();
            //The singulairty projectile is instantiated at the end of the weapon, and has force applied to it in its start function
            SingularityProjectile grenade = Instantiate(singularityProjectile, muzzleTransform.position, Quaternion.identity).GetComponent<SingularityProjectile>();
            grenade.cameraTransform = playerController.cameraTransform;
            //The weapon's charge is reduced by the set value
            weaponCharge -= singularityChargeUsage;
            singleShotLock = true;
        }
        else if (!singleShotLock)
        {
            AudioSource.PlayClipAtPoint(lowPower, transform.position, 1);
        }
    }

    public bool IsAimingAtStarstone()
    {
        RaycastHit rayHit;
        //A raycast is sent out to detect what the player is aiming at
        if(Physics.Raycast(muzzleTransform.position, transform.forward, out rayHit, chargeRange, starstoneLayer))
        {
            //If the raycast hits an object tagged as a starstone, the mode swapping code is executed
            //Used a tag comparison as the generator is made from several different 3D objects, so a GetComponent comparison wouldn't work
            if (rayHit.collider.gameObject.tag == "Starstone")
            {   
                //Sets the starstone to charge from as the StarstoneController component in the parent of the 3D object the raycast collides with
                starstoneToChargeFrom = rayHit.collider.gameObject.GetComponentInParent<StarstoneController>();

                switch (starstoneToChargeFrom.starstoneType)
                {
                    //If the parent object is the Speed Starstone, the new weapon mode is set to minigun
                    case StarstoneController.starstoneTypes.speedStarstone:
                        newWeaponMode = weaponModes.minigunMode;
                        starstoneToChargeFrom.chargePrototypeWeapon(newWeaponMode);
                        muzzleFlash.color = minigunColour;
                        break;
                    //If the parent object is the Health Starstone, the new weapon mode is set to vampire
                    case StarstoneController.starstoneTypes.healthStarstone:                                        //newWeaponMode is used to compare between the current weapon mode, and the one 
                        newWeaponMode = weaponModes.vampireMode;
                        starstoneToChargeFrom.chargePrototypeWeapon(newWeaponMode);
                        muzzleFlash.color = vampireColour;
                        //set by the Starstone being aimed at to determine if the weapon should switch mode
                        break;
                    //If the parent object is the Fire Starstone, the new weapon mode is set to grenade launcher
                    case StarstoneController.starstoneTypes.fireStarstone:
                        newWeaponMode = weaponModes.grenadeLauncherMode;
                        starstoneToChargeFrom.chargePrototypeWeapon(newWeaponMode);
                        muzzleFlash.color = grenadeColour;
                        break;
                    //If the parent object is the Buff Starstone, the new weapon mode is set to singularity
                    case StarstoneController.starstoneTypes.buffStarstone:
                        newWeaponMode = weaponModes.singularityMode;
                        starstoneToChargeFrom.chargePrototypeWeapon(newWeaponMode);
                        muzzleFlash.color = singularityColor;
                        break;
                }
                //Returns the boolean as true
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

    public void Fire()
    {
        //Checks which mode the weapon is currently in and runs the appropriate method
        switch (currentWeaponMode)
        {
            case weaponModes.minigunMode:
                FireMinigunMode();
                break;
            case weaponModes.vampireMode:
                FireVampireMode();
                break;
            case weaponModes.grenadeLauncherMode:
                FireGrenade();
                break;
            case weaponModes.singularityMode:
                FireSingularity();
                break;
        }
        uIController.UpdatePrototypeCharge((int)weaponCharge);
    }

    public void AimingIn()
    {
        //If the player is aiming at a Starstone...
        if (IsAimingAtStarstone())
        {   
            //and the weapon mode associated with it is different to the weapon's current mode...
            if (newWeaponMode != currentWeaponMode && starstoneToChargeFrom.genEnabled)
            {
                //then the new weapon mode is checked and the weapon is set up for the new mode it is being assigned
                switch (newWeaponMode)
                {
                    case weaponModes.minigunMode:
                        //Updates the current weapon mode of the weapon, so that this can be checked next time the player aims at a Starstone
                        currentWeaponMode = newWeaponMode;
                        //Sets the clip of the weapon's AudioSource to the minigun sound
                        weaponSound.clip = minigunSound;
                        //Sets the audio to loop, as the minigun mode is fully automatic
                        weaponSound.loop = true;
                        //Updates the colour of the prototype weapon's charge UI element to match the weapon mode
                        uIController.UpdatePrototypeSliderColour(speedColour);
                        //Resets the charge to 0, as the charge is being released to change modes
                        weaponCharge = 0f;
                        break;
                    case weaponModes.vampireMode:
                        //Updates the current weapon mode of the weapon, so that this can be checked next time the player aims at a Starstone
                        currentWeaponMode = newWeaponMode;
                        //Sets the clip of the weapon's AudioSource to the vampire sound
                        weaponSound.clip = vampireSound;
                        //Sets the AudioSource not to loop, as the vampire weapon is single shot
                        weaponSound.loop = false;
                        //Updates the colour of the prototype weapon's charge UI element to match the weapon mode
                        uIController.UpdatePrototypeSliderColour(healthColour);
                        //Resets the charge to 0, as the charge is being released to change modes
                        weaponCharge = 0f;
                        break;
                    case weaponModes.grenadeLauncherMode:
                        //Updates the current weapon mode of the weapon, so that this can be checked next time the player aims at a Starstone
                        currentWeaponMode = newWeaponMode;
                        //Sets the clip of the weapon's AudioSource to the grenade launcher sound
                        weaponSound.clip = grenadeLaunch;
                        //Sets the AudioSource not to loop, as the grenade launcher is single shot
                        weaponSound.loop = false;
                        //Updates the colour of the prototype weapon's charge UI element to match the weapon mode
                        uIController.UpdatePrototypeSliderColour(fireColour);
                        //Resets the charge to 0, as the charge is being released to change modes
                        weaponCharge = 0f;
                        break;
                    case weaponModes.singularityMode:
                        //Updates the current weapon mode of the weapon, so that this can be checked next time the player aims at a Starstone
                        currentWeaponMode = newWeaponMode;
                        //Sets the clip of the weapon's AudioSource to the singularity launcher sound
                        weaponSound.clip = singularityLaunch;
                        //Sets the AudioSource not to loop, as the singularity launcher is single shot
                        weaponSound.loop = false;
                        //Updates the colour of the prototype weapon's charge UI element to match the weapon mode
                        uIController.UpdatePrototypeSliderColour(singularityColor);
                        //Resets the charge to 0, as the charge is being released to change modes
                        weaponCharge = 0f;
                        break;
                }
            }
            //Otherwise, if the Starstone aimed at is the same as the fire mode of the weapon, has charge left in it, and the weapon isn't fully charged,
            //the recharging code is executed
            else if (weaponCharge < 100 && starstoneToChargeFrom.starstoneCharge - Time.deltaTime * weaponRechargeMultiplier >= 0 && starstoneToChargeFrom.genEnabled)
            {
                //The weapon's charge is increased by the weapon charge rate multiplied by Time.deltaTime to make it framerate independent
                weaponCharge += Time.deltaTime * weaponRechargeMultiplier;
                //Discharges the starstone by the weapon charge rate multiplied by Time.deltaTime to make it framerate independent
                starstoneToChargeFrom.starstoneCharge -= Time.deltaTime * weaponRechargeMultiplier;
                //If the weapon's charge exceeds 100, it is set to 100 as it is the maximum charge value
                if (weaponCharge > 100)
                {
                    weaponCharge = 100;
                }
                starstoneToChargeFrom.UpdateStoneUI();
            }
        }
        //Updates the UI elements for the prototype weapon's charge using the new charge values, casted as an integer to make the UI more readable
        uIController.UpdatePrototypeCharge((int)weaponCharge);
    }
}
