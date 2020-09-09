using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: PlayerBase                                       |
    // Script Author: James Smale                                    |
    // Purpose: Handles all aspects of the player, including movement|
    //          , health, camera controls and physics                |
    //***************************************************************|

    //Physics
    #region
    [Header("Physics Variables")]
    [Tooltip("The amount of gravity applied to the player each frame. Should be negative for downward force.")]
    public float gravityForce;
    [Tooltip("The value by which the gravity is multiplied.")]
    public float gravityMultiplier;
    [Tooltip("The distance from the player the floor can be before the player is considered grounded.")]
    public float groundDistance;
    [Tooltip("The layer that, if the player lands on it, will trigger isGrounded to return true.")]
    public LayerMask groundLayer;

    [Tooltip("The layer the ladders are assigned to in the game.")]
    public LayerMask ladderLayer;
    public LayerMask interactiveLayer;
    private RaycastHit interactableObject;
    private bool isGrounded;
    private bool hasLanded;
    [Space]
    #endregion
    //Player Movement
    #region
    [Header("Player Movement")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed;
    [Tooltip("The speed at which the player moves while jumping.")]
    public float jumpingMoveSpeed;
    [Tooltip("The height at which the player can jump.")]
    public float jumpHeight;
    [Header("Movement Speed Modifiers")]
    [Tooltip("The speed multiplier used to calculate how fast the player moves.")]
    public float moveSpeedMultiplier;
    [Tooltip("The value added to the movement multiplier while the player is sprinting.")]
    public float sprintSpeedMultiplier;
    [Tooltip("The value added to the movement multiplier while the player is crouching")]
    public float crouchSpeedMultiplier;
    [Tooltip("The value added to the movement multiplier while the player is fully submerged.")]
    public float underwaterSpeedMultiplier;
    [Tooltip("The value added to the movement multiplier while the player is wading through water.")]
    public float wadingSpeedMultiplier;
    [Tooltip("The value added to the movement multiplier while the player is climbing a ladder.")]
    public float climbingMultiplier;
    private float multiplierBeforeJump;
    private float xInput, yInput, zInput;
    private Vector3 movement;
    private Vector3 currentVelocity;
    private bool isSprinting, isWading, isCrouching, isJumping, isSwimming, isClimbing;
    public CharacterController characterController;
    public GameObject playerModel;
    public GameObject movingPartsParent;
    private Vector3 standingEyePosition;
    public Transform crouchingEyePosition;
    [Space]
    #endregion
    //Health Management
    #region
    [Header("Health Management")]
    [Tooltip("The maximum amount of health the player can have.")]
    public float maxHealth;
    [Tooltip("The current amount of health the player has.")]
    [SerializeField]
    private float currentHealth;
    [Tooltip("The amount of health that, after dropping below, the player's health will not regenerate past.")]
    public float healthRegenCutoff;
    [Tooltip("The rate at which the player's health regenerates.")]
    public float regenRate;
    [Tooltip("The amount of time passed since the player last took damage.")]
    public float timeSinceTakenDamage;
    [Tooltip("The amount of time the player must wait before being able to regenerate health after having taken damage.")]
    public float regenWaitAfterDamage;
    [Tooltip("A boolean determining whether or not the player is currently able to regenerate health.")]
    public bool canRegenHealth;
    [Tooltip("A boolean determining whether or not the player's health can regeneate to the maximum health value.")]
    public bool canRegenToMax;
    [Tooltip("The amount of time it takes to revive this player.")]
    public float reviveTime;
    [HideInInspector]
    public float reviveTimer;
    [Tooltip("The amount of health another player will regain when this player revives them.")]
    public float reviveHealthIncrease;
    public bool bloodOverlayActive;
    public bool isDead;
    [Space]
    #endregion
    //Stamina Management
    #region
    [Header("Stamina Management")]
    public float maxSprintStamina;
    public float staminaDrainRate;
    public float sprintStamina;
    public float regenWaitAfterSprint;
    public float staminaRechargeCooldown;
    public bool canRegenStamina;
    [Space]
    #endregion
    //Camera Controls
    #region
    [Header("Camera Input & Controls")]
    [Tooltip("The transform of the camera attached to the player.")]
    public Transform cameraTransform;
    [Tooltip("The sensitivity multiplier of the mouse.")]
    public float mouseSensitivity;
    private float mouseX, mouseY;
    [HideInInspector]
    public float xRotation;
    [Space]
    #endregion
    //Weapons
    #region
    [Header("Weapons")]
    [Tooltip("The positon at which the player's weapon is held while not aiming in.")]
    public Transform weaponHoldPoint;
    [Tooltip("The position at which the player's weapon is help while aiming in.")]
    public Transform adsHoldPoint;
    [Tooltip("The array in which all of the player's weapons are stored.")]
    public GameObject[] weaponsArray;
    [Tooltip("The weapon the player currently has equipped.")]
    public GameObject activeWeapon;
    public GameObject meleeWeapon;
    private int activeWeaponIndex;
    private float timeSinceLastPress;
    private float prototypeSwitchTimeout;
    private bool preparingToSwapWeapon;
    private bool isADS;
    #endregion
    //Abilities
    #region
    [Header("Ability Management")]
    [Tooltip("The amount of time remaining before the player can use their blink ability again.")]
    public float leftAbilityCooldown;
    [HideInInspector]
    public bool canUseLeftAbility;
    [HideInInspector]
    public float leftAbilityCooldownRounded;
    [Tooltip("The amount of time remaining before the player can use their mine ability again.")]
    public float rightAbilityCooldown;
    [HideInInspector]
    public float rightAbilityCooldownRounded;
    [Tooltip("The prefab GameObject of the Blinkball.")]
    public GameObject leftAbilityPrefab;
    [Tooltip("The prefab GameObject of the Mine.")]
    public GameObject rightAbilityPrefab;
    [Space]
    #endregion
    //Powerup Variables
    #region
    [Header("Speedboost Stats")]
    [Tooltip("A boolean determining whether or not the player is currently under the effect of a speedboost.")]
    public bool hasSpeedBoost;
    private bool hasHadSpeedBoosted;
    [Tooltip("The value added to the movement multiplier while the player has gained the speedboost powerup.")]
    public float speedBoostMultiplier;
    public float speedBoostDuration;
    public float speedBoostTimer;
    [Header("Invulnerability Stats")]
    [Tooltip("A boolean determining whether or not the player is currently invulnerable.")]
    public bool isInvulnerable;
    [Tooltip("The length of time for which the player is invulnerable after picking up a power-up.")]
    public float invunlerabilityLength;
    private float invulnerabilityTimer;
    #endregion
    //Sounds
    #region
    [Header("Sounds")]
    [Tooltip("The AudioSource to play while the player is moving.")]
    public AudioSource walkingSound;
    public AudioSource outOfBreath;
    [Tooltip("The AudioClip to play when the player toggles their flashlight.")]
    public AudioClip flashlightSound;
    [Tooltip("The AudioClip to play when the player uses their melee attack.")]
    public AudioClip meleeSound;
    [Tooltip("The AudioClip to play when the player falls to the ground.")]
    public AudioClip[] landingSounds;
    [Tooltip("The sound that plays when an action has failed.")]
    public AudioClip actionFailed;
    [Tooltip("The sound played when the player is attacked while invulnerable.")]
    public AudioClip[] hitWhileInvulnerable;
    public AudioClip[] hitWhileNotInvulnrable;
    [Space]
    #endregion
    //Miscellaneous Variables
    #region
    [Header("Miscellaneous")]
    [Tooltip("The flashlight object attached to the player.")]
    public GameObject flashlight;
    [HideInInspector]
    public string playerNumber;
    private bool flashlightToggle;
    private bool canToggleLight;
    private Animator playerAnimator;
    private GameController gameController;
    public UIController uIController;
    public GameObject pauseMenu;
    private Vector3 standingScale, crouchingScale;
    public Transform playerFeet;
    #endregion
    //Enums
    #region
    public enum PlayerStates
    {
        standardState,
        climbingState,
        deadState,
        pausedState
    }
    [Header("Player States")]
    [Tooltip("The current state that the player is in.")]
    public PlayerStates playerState;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Checks to see if any numerical values are unassigned. If they are, they are assigned a standard value to ensure all mechanics work correctly.
        AssignNullVariables();
        isSprinting = false;
        isCrouching = false;
        if (healthRegenCutoff > maxHealth) { Debug.LogWarning("The health regen cutoff value is greater than the maximum health value of" + gameObject + ", this player's health will regen as normal."); }
        currentHealth = maxHealth;
        //Sets the first weapon in the array of weapons to active, and sets it as the player's active weapon to feed information through to the weapon and the UI Controller.
        weaponsArray[activeWeaponIndex].SetActive(true);
        activeWeapon = weaponsArray[activeWeaponIndex];
        //Assigns the animator component to the player.
        playerAnimator = gameObject.GetComponent<Animator>();
        //Finds and assigns the GameController
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //Finds and assigns the UIController
        uIController = gameObject.GetComponent<UIController>();
        //Sets canBlink to true so that the player is able to use the ability immediately when the game loads.
        canUseLeftAbility = true;
        //Sets the player's flashlight boolean to false as the flashlight starts turned off.
        flashlightToggle = false;
        //Sets the player's standing and crouching sizes and positions
        standingScale = playerModel.transform.localScale;
        standingEyePosition = new Vector3(0, 0, 0);
        crouchingScale = new Vector3(playerModel.transform.localScale.x, playerModel.transform.localScale.y / 2, playerModel.transform.localScale.z);
        //Sets up the UI elements for the currently equipped weapon
        uIController.activeWeaponController = activeWeapon.GetComponent<build_a_weapon>();
        uIController.GetChangedWeapon();
        uIController.UpdateAmmoText();


    }

    private void AssignNullVariables()
    {
        if (gravityForce == 0) { gravityForce = -9.81f; }
        if (gravityMultiplier == 0) { gravityMultiplier = 2f; }
        if (groundDistance == 0) { groundDistance = 0.4f; }
        if (jumpHeight == 0) { jumpHeight = 0.75f; }
        if (jumpingMoveSpeed == 0) { jumpingMoveSpeed = 2f; }
        if (maxHealth == 0) { maxHealth = 100f; }
        if (regenRate == 0) { regenRate = 5f; }
        if (regenWaitAfterDamage == 0) { regenWaitAfterDamage = 5f; }
        if (reviveTime == 0) { reviveTime = 5f; }
        if (reviveHealthIncrease == 0) { reviveHealthIncrease = 25f; }
        if (prototypeSwitchTimeout == 0) { prototypeSwitchTimeout = 0.25f; }
        if (moveSpeed == 0) { moveSpeed = 4f; }
        if (moveSpeedMultiplier == 0) { moveSpeedMultiplier = 1f; }
        if (sprintSpeedMultiplier == 0) { sprintSpeedMultiplier = 1f; }
        if (maxSprintStamina == 0) { maxSprintStamina = 100f; }
        sprintStamina = maxSprintStamina;
        if (regenWaitAfterSprint == 0) { regenWaitAfterSprint = 3f; }
        if (staminaDrainRate == 0) { staminaDrainRate = 2.5f; }
        if (crouchSpeedMultiplier == 0) { crouchSpeedMultiplier = -0.5f; }
        if (wadingSpeedMultiplier == 0) { wadingSpeedMultiplier = -0.3f; }
        if (underwaterSpeedMultiplier == 0) { underwaterSpeedMultiplier = -0.2f; }
        if (mouseSensitivity == 0) { mouseSensitivity = 50f; }
        if (leftAbilityCooldown == 0) { leftAbilityCooldown = 5f; }
        if (invunlerabilityLength == 0) { invunlerabilityLength = 7f; }
        invulnerabilityTimer = invunlerabilityLength;
        if (speedBoostMultiplier == 0) { speedBoostMultiplier = 2f; }
        if (speedBoostDuration == 0) { speedBoostDuration = 7f; }
        speedBoostTimer = speedBoostDuration;
        if (string.IsNullOrEmpty(playerNumber)) { playerNumber = "PlayerOne"; }
        hasLanded = true;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    // Update is called once per frame
    public void PlayerStateHandler()
    {
        if (playerState == PlayerStates.deadState || playerState == PlayerStates.pausedState)
        {
            if (playerNumber == "PlayerOne")
            {
                Cursor.visible = true;                  //Makes the cursor visible and frees it so that the player can interact with any UI elements active
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if (playerState == PlayerStates.climbingState || playerState == PlayerStates.standardState)
        {
            CameraControls();
            CheckCanClimb();
            CheckGrounded();
            HealthManagement();
            PlayerSounds();
            CooldownTimers();
            PowerupTimers();
            WeaponControls();
            MiscControls();
            AmmoAlerts();
            if (preparingToSwapWeapon)
            {
                WeaponSwapTimer();
            }
            StaminaManagement();
            if(isSprinting && canRegenStamina && (xInput != 0 || zInput != 0))
            {
                canRegenStamina = false;
            }
            //Locks the player's cursor to the center of the screen while they play
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (playerState == PlayerStates.climbingState)
        {
            ClimbingControls();
        }
        if (playerState == PlayerStates.standardState)
        {
            MovementControls();
            ApplyGravity();
        }
        if (playerState == PlayerStates.deadState)
        {
            if (!isDead)
            {
               
                KillPlayer();

            }
        }
        PauseControls();
    }

    public virtual void MovementControls()
    {
        if (isGrounded || (!isGrounded && !isJumping))
        {
            //Records the input from the keyboard, or left analog stick on the appropriate controller
            xInput = Input.GetAxis(playerNumber + "Horizontal");
            zInput = Input.GetAxis(playerNumber + "Vertical");
            movement = transform.right * xInput + transform.forward * zInput;
            //Moves the player by the newly calculated movement vector, applying the movement speed and any multipliers and using deltaTime to make movement non-framerate dependent
            characterController.Move(movement * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
        }
        else if (!isGrounded && isJumping)
        {
            //Doesn't get the current input values to ensure the player can't change direction or strafe mid-jump
            movement = transform.right * xInput + transform.forward * zInput;
            //Uses the speed multiplier from the frame before the player jumped to prevent players from changing speed or strafing in mid-air
            characterController.Move(movement * jumpingMoveSpeed * multiplierBeforeJump * Time.deltaTime);
        }
        //If the sprint button is held and the player is not already sprinting, the sprint speed modifier is added to the movement speed multiplier and the isSprinting is set to true
        //Checking to see if the player is already sprinting prevents the speed from being added more than once
        if (Input.GetButtonDown(playerNumber + "Sprint") && !isSprinting && sprintStamina - staminaDrainRate * Time.deltaTime > 0)
        {
            moveSpeedMultiplier += sprintSpeedMultiplier;
            isSprinting = true;
            if (zInput != 0 || xInput != 0)
            {
                canRegenStamina = false;
            }
        }
        //If the player release the sprint button while they are sprinting, the sprint speed modifier is subtracted from the movement speed multiplier and isSprinting is set to false
        //Checking to see if the player is already sprinting prevents the modifier from being subtracted more than once
        if (Input.GetButtonUp(playerNumber + "Sprint") && isSprinting)
        {
            moveSpeedMultiplier -= sprintSpeedMultiplier;
            isSprinting = false;
        }
        //If the player presses the jump button and are currently standing on the ground, their current movement speed multiplier is saved and their vertical velocity is set to
        //the square root of the player's jumpheight doubled, multiplied by the gravity force in order to create a more realistic jump without using Unity physics
        if (Input.GetButtonDown(playerNumber + "Jump") && isGrounded)
        {
            isJumping = true;
            hasLanded = false;
            //Speed multiplier is saved so it can be used to move the player without them being able to change it in mid air
            multiplierBeforeJump = moveSpeedMultiplier;
            currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);
        }
        //If the player presses the crouch button, their scale is set to the crouching scale, and the crouching speed modifier is added to their speed multiplier
        //Checking to see if the player is already crouching prevents the modifier from being added multiple times
        if (Input.GetButtonDown(playerNumber + "Crouch") && !isCrouching)
        {
            isCrouching = true;
            playerModel.transform.localScale = crouchingScale;
            characterController.height /= 2;
            movingPartsParent.transform.position = crouchingEyePosition.transform.position;
            moveSpeedMultiplier += crouchSpeedMultiplier;
        }
        //When the player releases the crouch button, their scale is reseting to full size and the crouching speed modifier is subtracted from their speed multiplier
        //Checking to see if the player is already crouching prevents the modifier from being subtracted multiple times
        if (Input.GetButtonUp(playerNumber + "Crouch") && isCrouching)
        {
            isCrouching = false;
            playerModel.transform.localScale = standingScale;
            characterController.height *= 2;
            movingPartsParent.transform.localPosition = standingEyePosition;
            moveSpeedMultiplier -= crouchSpeedMultiplier;
        }
    }

    public void StaminaManagement()
    {
        if (isSprinting && (xInput != 0 || zInput != 0))
        {
            DrainStamina();
        }
        else if (sprintStamina < maxSprintStamina && canRegenStamina)
        {
            RechargeStamina();
        }
    }

    public void DrainStamina()
    {
        sprintStamina -= staminaDrainRate * Time.deltaTime;
        if(sprintStamina <= 0)
        {
            moveSpeedMultiplier -= sprintSpeedMultiplier;
            isSprinting = false;
            outOfBreath.Play();
        }
        uIController.UpdateStaminaBar(sprintStamina);
    }

    public void RechargeStamina()
    {
        sprintStamina += staminaDrainRate * Time.deltaTime;
        if(sprintStamina > maxSprintStamina)
        {
            sprintStamina = maxSprintStamina;
        }
        uIController.UpdateStaminaBar(sprintStamina);
    }

    public virtual void WeaponControls()
    {
        //Checks that the player's active weapon is not the Prototype weapon, as it has its own firing code
        if (Input.GetAxis(playerNumber + "Fire") > 0 && activeWeapon.tag != "Prototype")
        {
            //Runs the UseWeapon method of the baseWeaponClass to fire the active weapon
            activeWeapon.GetComponent<baseWeaponClass>().useWeapon();
        }
        //If the player releases the fire button and is using a spread shot weapon, the weapon fire lock is lifted
        if (Input.GetAxis(playerNumber + "Fire") == 0 && activeWeapon.tag != "Prototype")
        {
            if (activeWeapon.GetComponent<build_a_weapon>().typeOfWeapon == build_a_weapon.typesOfWeapon.spreadShot)
            {
                activeWeapon.GetComponent<build_a_weapon>().spreadShotLock = false;
            }
        }
        else if (Input.GetAxis(playerNumber + "Fire") > 0 && activeWeapon.tag == "Prototype")
        {
            activeWeapon.GetComponent<PrototypeWeapon>().Fire();
        }
        if (Input.GetAxis(playerNumber + "Fire") == 0 && activeWeapon.tag == "Prototype")
        {
            activeWeapon.GetComponent<PrototypeWeapon>().singleShotLock = false;
            if (activeWeapon.GetComponent<PrototypeWeapon>().weaponSound.isPlaying && activeWeapon.GetComponent<PrototypeWeapon>().weaponSound.loop == true)
            {
                activeWeapon.GetComponent<PrototypeWeapon>().GetComponent<PrototypeWeapon>().weaponSound.Stop();
            }

        }
        if (Input.GetAxis(playerNumber + "Aim") > 0 && activeWeapon.GetComponent<build_a_weapon>())
        {
            activeWeapon.GetComponent<build_a_weapon>().aimWeapon(true);
            isADS = true;
        }

        if (Input.GetAxis(playerNumber + "Aim") == 0 && activeWeapon.GetComponent<build_a_weapon>())
        {
            activeWeapon.GetComponent<build_a_weapon>().aimWeapon(false);
            isADS = false;
        }
        else if (Input.GetAxis(playerNumber + "Aim") > 0 && activeWeapon.tag == "Prototype")
        {
            //Moves the prototype weapon to its ADS position
            activeWeapon.transform.position = adsHoldPoint.position;
            activeWeapon.GetComponent<PrototypeWeapon>().AimingIn();
        }
        if (Input.GetAxis(playerNumber + "Aim") == 0 && activeWeapon.tag == "Prototype")
        {
            //Moves the prototype weapon back to its hipfire location
            activeWeapon.transform.position = weaponHoldPoint.position;
        }
        if (Input.GetButtonDown(playerNumber + "Melee"))
        {
            //Plays the melee animation of the player and the appropriate sound
            playerAnimator.SetTrigger("Punch");
            AudioSource.PlayClipAtPoint(meleeSound, transform.position);
        }
        if (Input.GetButtonDown(playerNumber + "ChangeWeapon"))
        {
            //Resets the value of timeSinceLastPress so the timer can start from 0
            timeSinceLastPress = 0f;
            //If the player isn't already preparing to swap their weapon, they are now
            if (!preparingToSwapWeapon)
            {
                preparingToSwapWeapon = true;
            }
            //If the player is already preparing to swap their weapon, and they press the button before the time since their last press surpasses the timeout value,
            //their active weapon is changed to be the Prototype Weapon
            else if (preparingToSwapWeapon && timeSinceLastPress <= prototypeSwitchTimeout)
            {
                weaponsArray[activeWeaponIndex].SetActive(false);
                activeWeaponIndex = weaponsArray.Length - 1;
                weaponsArray[activeWeaponIndex].SetActive(true);
                activeWeapon = weaponsArray[activeWeaponIndex];
                preparingToSwapWeapon = false;
                timeSinceLastPress = 0f;
            }


        }
        if (Input.GetButtonDown(playerNumber + "Reload") ) 
        {
            if(activeWeapon.tag != "Prototype" && activeWeapon.GetComponent<baseWeaponClass>().currentBullets != activeWeapon.GetComponent<baseWeaponClass>().magazineCapacity)
            {
            activeWeapon.GetComponent<Animator>().Play("Reload");
            uIController.UpdateAmmoText();
            }
        }
    }

    public virtual void MiscControls()
    {

        if (Input.GetKeyDown(KeyCode.O) && gameObject.GetComponent<CharacterVariantOne>() != null)
        {
            bool toggleBool = gameObject.GetComponent<CharacterVariantOne>().pixelMode.activeSelf;
            gameObject.GetComponent<CharacterVariantOne>().pixelMode.SetActive(!toggleBool);
        }
        //Interacting Controls
        if (Input.GetButtonDown(playerNumber + "Interact"))
        {
            if (CanInteract())
            {
                //Checks to see what script is attached to the object being interacted with and runs the necessary method
                if (interactableObject.collider.gameObject.GetComponentInParent<StarstoneController>() != null)
                {
                    if (interactableObject.collider.gameObject.GetComponentInParent<StarstoneController>().genEnabled == true)
                    {
                        //Activates the effect of the Starstone the player has interacted with, buffing all of the enemies in the scene and those that spawn later on
                        interactableObject.collider.gameObject.GetComponentInParent<StarstoneController>().ActivateEffect();

                    }
                    else
                    {
                        gameController.connectNewGenerator(interactableObject.collider.gameObject.transform.parent.gameObject);
                    }
                }
                else if (interactableObject.collider.gameObject.GetComponentInParent<StarStoneBase>() != null)
                {
                    //Activates the effect of the Starstone the player has interacted with, buffing all of the enemies in the scene and those that spawn later on
                    interactableObject.collider.gameObject.GetComponentInParent<StarStoneBase>().ActivateStarStone();
                }
                else if (interactableObject.collider.gameObject.GetComponent<mineScript>() != null && gameObject.GetComponent<CharacterVariantOne>() != null)
                {
                    //Destroys the mine being interacted with so that the player can throw another down elsewhere
                    Destroy(interactableObject.collider.gameObject);
                }
                else if (interactableObject.collider.gameObject.GetComponent<ThrowingKnife>() != null && gameObject.GetComponent<CharacterVariantTwo>() != null)
                {
                    Destroy(interactableObject.collider.gameObject);
                }
                else if(interactableObject.collider.gameObject.GetComponent<TeleportBeacon>() != null && gameObject.GetComponent<CharacterVariantTwo>() != null)
                {
                    Destroy(interactableObject.collider.gameObject);
                }
            }
        }
        if (Input.GetButton(playerNumber + "Interact"))
        {
            if (CanInteract())
            {
                //If the player is trying to interact with another player and that player is currently dead, they can be revived
                if (interactableObject.collider.gameObject.GetComponentInParent<PlayerBase>() != null)
                {
                    PlayerBase player = interactableObject.collider.gameObject.GetComponentInParent<PlayerBase>();
                    if (player.playerState == PlayerStates.deadState)
                    {
                        if (!uIController.reviveUI.activeSelf)
                        {
                            uIController.ToggleReviveUI(true);
                        }
                        //Increases the value of the reviveTimer by the amount of time passed since the last frame
                        player.reviveTimer += Time.deltaTime;
                        uIController.UpdateReviveSlider(player.reviveTimer);
                        //If the revive timer reaches the value of reviveTime, the player has health restored and is set to the standard state
                        //The value of the reviveTimer is also reset to 0 so it can be used again
                        if (player.reviveTimer >= player.reviveTime)
                        {
                            uIController.UpdateReviveSlider(0f);
                            uIController.ToggleReviveUI(false);
                            player.RestoreHealth(reviveHealthIncrease);
                            player.playerState = PlayerStates.standardState;
                            player.reviveTimer = 0f;
                            player.uIController.ToggleDeathCanvas(false);
                            player.isDead = false;
                            gameController.deadPlayers--;
                        }
                    }
                }
                else if (uIController.reviveUI.activeSelf)
                {
                    uIController.UpdateReviveSlider(0f);
                    uIController.ToggleReviveUI(false);
                }
            }
        }
        //Left Ability controls
        if (Input.GetButtonDown(playerNumber + "LeftAbility") && canUseLeftAbility)
        {
            UseLeftAbility();
        }
        //Right Ability Controls
        if (Input.GetButtonDown(playerNumber + "RightAbility"))
        {
            UseRightAbility();
        }
        //Flashlight Controls
        if (Input.GetAxis(playerNumber + "Flashlight") == 1 && canToggleLight)
        {
            //Toggles the flashlight boolean and plays the toggle sound, setting the flashlight to active or inactive depending on the value of flashlightToggle
            flashlightToggle = !flashlightToggle;
            AudioSource.PlayClipAtPoint(flashlightSound, flashlight.transform.position);
            flashlight.SetActive(flashlightToggle);
            canToggleLight = false;
        }
        if (Input.GetAxis(playerNumber + "Flashlight") == 0 && !canToggleLight)
        {
            canToggleLight = true;
        }
    }

    public virtual void PauseControls()
    {
        //Pausing Controls
        if (Input.GetButtonDown(playerNumber + "StartButton"))
        {
            if (playerState == PlayerStates.pausedState)
            {
                UnpausePlayer();
            }
            else if (playerState != PlayerStates.deadState)
            {
                PausePlayer();
            }
        }
    }

    public void ToggleMeleeWeaponActive()
    {
        bool isActive = !meleeWeapon.activeSelf;
        Debug.Log(isActive);
        meleeWeapon.SetActive(isActive);
    }

    public void UnpausePlayer()
    {
        //If the player has a gamecontroller attached, the UnpauseAllPlayers method is run
        if (gameController != null)
        {
            gameController.UnpauseAllPlayers();
        }
        else
        {
            //Otherwise, the passage of time continues, the player returns to its standard state and the pause menu is closed
            Time.timeScale = 1;
            playerState = PlayerStates.standardState;
            pauseMenu.SetActive(false);
        }
    }

    public void PausePlayer()
    {
        //If the player has a gamecontroller attached, the PauseAllPlayers method is run
        if (gameController != null)
        {
            gameController.PauseAllPlayers();
        }
        else
        {
            //Otherwise, the passage of time is paused, the player enters the paused state and the pause menu opens
            Time.timeScale = 0;
            playerState = PlayerStates.pausedState;
            pauseMenu.SetActive(true);
        }
    }

    public void KillPlayer()
    {
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false; // Am I causing problems? I think so. 
        if (gameController != null)
        {
            gameController.deadPlayers++;
        }
        else
        {
            Time.timeScale = 0;
            
            uIController.ToggleDeathCanvas(true);
        }
        isDead = true;
    }

    public void WeaponSwapTimer()
    {
        //The value of timeSinceLastPress is incremented every frame
        timeSinceLastPress += Time.deltaTime;
        //If it surpasses the value of prototypeSwitchTimeout, the player's weapon is swapped
        if (timeSinceLastPress > prototypeSwitchTimeout)
        {
            //The currently active weapon is disabled, and the array index is incremented by 1
            weaponsArray[activeWeaponIndex].SetActive(false);
            activeWeaponIndex++;
            //If the index matches that of the prototype weapon or is out of range, it is set to 0
            if (activeWeaponIndex >= weaponsArray.Length - 1)
            {
                activeWeaponIndex = 0;
            }
            //The newly selected weapon is activated and set as the current weapon
            weaponsArray[activeWeaponIndex].SetActive(true);
            activeWeapon = weaponsArray[activeWeaponIndex];
            //The new values are acquired by the UI Controller and the UI elements are updated to use them
            uIController.GetChangedWeapon();
            uIController.UpdateAmmoText();
            //Preparing to swap is set to false and the timer is reset, so that the weapon changing mechanism is reset
            preparingToSwapWeapon = false;
            timeSinceLastPress = 0f;
        }
    }

    public virtual void UseLeftAbility()
    {
        //Abilities should be programmed unique to a player character, using the method from the base class produces an error
        Debug.LogError("No override method has been defined for this character's left ability.");
    }

    public virtual void UseRightAbility()
    {
        //Abilities should be programmed unique to a player character, using the method from the base class produces an error
        Debug.LogError("No override method has been created for this character's right ability.");
    }

    public void ApplyGravity()
    {
        //Adds the value of gravity to the player's current velocity in the Y axis
        currentVelocity.y += gravityForce * Time.deltaTime;
        //Moves the player downwards on the Y axis by the value of current velocity, using the gravity multiplier to add any gravity affects and using deltaTime to make gravity non-framerate dependent
        characterController.Move(currentVelocity * gravityMultiplier * Time.deltaTime);
    }

    public virtual void ClimbingControls()
    {
        //Records input from the left analog stick and saved them to the necessary variables
        yInput = Input.GetAxis(playerNumber + "Vertical");
        xInput = Input.GetAxis(playerNumber + "Horizontal");

        //Moves the player up and down the Y axis to climb ladders
        Vector3 movement = transform.right * xInput + transform.up * yInput;
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    public virtual void CameraControls()
    {
        //Input from the right analog stick is recorded and saved to the necessary variables
        mouseX = Input.GetAxis(playerNumber + "CameraX");
        mouseY = Input.GetAxis(playerNumber + "CameraY");
        //If this is Player One, mouse inputs are recorded as well
        xRotation -= mouseY;
        //Locks the camera from going above or below 90 degrees in the Y direction
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //Rotates all of the necessary GameObjects to match camera rotation
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        weaponHoldPoint.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // adsHoldPoint.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //Rotates the player object left and right based on player input
        transform.Rotate(Vector3.up * mouseX);
    }

    public void PlayerSounds()
    {
        //The pitch of the AudioSource is changed to match the tempo with the movement speed
        walkingSound.pitch = moveSpeedMultiplier;
        //If the player is moving in any direction, the walking sound is played
        if ((xInput != 0 || zInput != 0) && isGrounded && !walkingSound.isPlaying)
        {
            walkingSound.Play();
        }
        //When the player stops moving, the walking sound is stopped
        else
        {
            walkingSound.Stop();
        }
    }

    public void CooldownTimers()
    {
        //If the player can't use their left ability, a cooldown timer is run each frame
        if (!canUseLeftAbility)
        {
            leftAbilityCooldown -= Time.deltaTime;
            //The value is rounded to two decimal places so that it can be used in the ability's UI element
            leftAbilityCooldownRounded = Mathf.Round(leftAbilityCooldown * 100) / 100;
            //Once the timer reaches 0, the player is able to use their ability again
            if (leftAbilityCooldown <= 0)
            {
                canUseLeftAbility = true;
                leftAbilityCooldown = 5f;
            }
            uIController.UpdateBlinkTimer();
        }
        //If the player cannot regenerate health, a cooldown timer is run each frame
        if (!canRegenHealth)
        {
            //The time since they last took damage is incremented each frame
            timeSinceTakenDamage += Time.deltaTime;
            //If the time since the player last took damage reaches the value of regenWaitAfterDamage, the player is then able to regen health again
            if (timeSinceTakenDamage >= regenWaitAfterDamage)
            {
                canRegenHealth = true;
            }
        }
        if ((!canRegenStamina && !isSprinting) || !canRegenStamina && isSprinting && xInput == 0 && zInput == 0)
        {
            staminaRechargeCooldown += Time.deltaTime;
            if(staminaRechargeCooldown >= regenWaitAfterSprint)
            {
                canRegenStamina = true;
                staminaRechargeCooldown = 0f;
            }
        }
    }

    public void PowerupTimers()
    {
        //If the player is invulnerable, then a timer is run each frame
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            //When the timer reaches zero, their invulnerability wears off and the timer is reset
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
                invulnerabilityTimer = invunlerabilityLength;
            }
        }
        //If the player has a speed boost, a timer is run each frame
        if (hasSpeedBoost)
        {
            speedBoostTimer -= Time.deltaTime;
            //When the timer reaches zero, their speed bost is removed and the timer is reset
            if (speedBoostTimer <= 0f)
            {
                moveSpeedMultiplier -= speedBoostMultiplier;
                hasSpeedBoost = false;
                speedBoostTimer = speedBoostDuration;
            }
        }
    }

    public void TakeDamage(float damageDealt)
    {
        if (!isInvulnerable)
        {
            // play sound 
            currentHealth -= damageDealt;
            canRegenHealth = false;
            timeSinceTakenDamage = 0f;
        }
        else
        {
            int randInt = UnityEngine.Random.Range(0, hitWhileInvulnerable.Length - 1);
            AudioSource.PlayClipAtPoint(hitWhileInvulnerable[randInt], transform.position);
        }
    }

    public void RestoreHealth(float restoreAmount)
    {
        currentHealth += restoreAmount;
    }

  public void CheckGrounded()
    {
        //A spherical area at the player's feet is checked. If it contains a GameObject on the ground layer, the player is set as grounded
        isGrounded = Physics.CheckSphere(playerFeet.position, groundDistance, groundLayer);
        if (!isGrounded) { hasLanded = false; }
        if (isGrounded && currentVelocity.y < 0)
        {
            isJumping = false;
            //Prevents a downward force from being built up on the player while they are on the ground, so that if they step off of a ledge they do not plummet
            currentVelocity.y = -2f;
            if (!hasLanded)
            {
                int randInt = UnityEngine.Random.Range(0, landingSounds.Length - 1);
                AudioSource.PlayClipAtPoint(landingSounds[randInt], transform.position, 0f);
                hasLanded = true;
            }
        } }
    

    public void CheckCanClimb()
    {
        //A raycast is sent out from the player's feet. If it collides with an object on the ladder layer, the player enters climing mode
        //The raycast comes from the player's feet to ensure they are in climbing mode until they reach the top and can step off
        if (Physics.Raycast(playerFeet.position, playerFeet.forward, 0.5f, ladderLayer))
        {
            playerState = PlayerStates.climbingState;
            moveSpeedMultiplier += climbingMultiplier;
        }
        //If the raycast doesn't hit a ladder, and the player is not already in the standard state, they are set to standard state
        //The ladder climbing speed modifier is subtracted from their movement speed
        else
        {
            if (playerState != PlayerStates.standardState)
            {
                playerState = PlayerStates.standardState;
                moveSpeedMultiplier -= climbingMultiplier;
            }
        }
    }

    public bool CanInteract()
    {
        //Fires out a raycast from the camera. If it collides with something, it returns true 
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out interactableObject, 2.5f))
        {
            return true;
        }
        //If it collides with something not on the interactive layer, it returns false
        else
        {
            return false;
        }
    }

    public void MeleeAttack()
    {
        meleeWeapon.GetComponent<MeleeWeapon>().ToggleHitDetection();
    }

    public void AmmoAlerts()
    {
        if (activeWeapon.GetComponent<baseWeaponClass>() != null)
        {
            baseWeaponClass activeWeaponScript = activeWeapon.GetComponent<baseWeaponClass>();
            if (activeWeaponScript.currentBullets <= activeWeaponScript.magazineCapacity * 0.25 && activeWeaponScript.totalBullets > 0 && uIController.reloadAlert.activeSelf == false)
            {
                uIController.ToggleReloadAlert(true);
            }
            else if (activeWeaponScript.currentBullets > activeWeaponScript.magazineCapacity * 0.25 && uIController.reloadAlert.activeSelf == true)
            {
                uIController.ToggleReloadAlert(false);
            }
            if (activeWeaponScript.totalBullets == 0)
            {
                uIController.ToggleChangeWeaponAlert(true);
                uIController.ToggleReloadAlert(false);
            }
            else if (activeWeaponScript.totalBullets > 0 && uIController.swapWeaponAlert.activeSelf == true)
            {
                uIController.ToggleChangeWeaponAlert(false);
            }
        }
    }

    public void HealthManagement()
    {
        //If the player's health is reduced past the regen cutoff, they can no longer regenerate to maximum health
        if (currentHealth <= healthRegenCutoff)
        {
            canRegenToMax = false;
        }
        //If their health is greater than the cutoff, they can regenerate to max
        else
        {
            canRegenToMax = true;
        }
        //If the player is able to regenerate their health to the maximum value, it is incremented by the regen rate every frame
        if (canRegenHealth && currentHealth < maxHealth && canRegenToMax)
        {
            currentHealth += regenRate * Time.deltaTime;
            //If it surpasses the maximum health the player can have, it is set to the maximum value
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        //If the player is only able to regenerate their health to the cutoff point, it is still incremented every frame
        else if (canRegenHealth && currentHealth < healthRegenCutoff)
        {
            currentHealth += regenRate * Time.deltaTime;
            //But if it surpasses the cutoff point, it is set to that value
            if (currentHealth > healthRegenCutoff)
            {
                currentHealth = healthRegenCutoff;
            }
        }
        if (currentHealth <= 50 && !bloodOverlayActive)
        {
            bloodOverlayActive = true;
            uIController.ActivateBloodOverlay(true);
        }
        else if (currentHealth > 50 && bloodOverlayActive)
        {
            bloodOverlayActive = false;
            uIController.ActivateBloodOverlay(false);
        }
        if (bloodOverlayActive)
        {
            float alphaValue = 1 - currentHealth / 100;
            Color newAlpha = new Color(0.85f, 0.8f, 0.8f, alphaValue);
            uIController.UpdateBloodAlpha(newAlpha);
        }
        //If the player's health drops past 0, it is set to equal 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        
          playerState = PlayerStates.deadState;
      
        }
        uIController.UpdateHealthbar();
    }

}

