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
    [Tooltip("The amount of gravity applied to the player each frame.")]
    public float gravityForce;
    [Tooltip("The value by which the gravity is multiplied.")]
    public float gravityMultiplier;
    [Tooltip("The distance from the player the floor can be before the player is considered grounded.")]
    public float groundDistance;
    private bool isGrounded;
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
    private CharacterController characterController;
    [Space]
    #endregion
    //Health Management
    #region
    [Header("Health Management")]
    [Tooltip("The maximum amount of health the player can have.")]
    public float maxHealth;
    [Tooltip("The current amount of health the player has.")]
    public float currentHealth;
    [Tooltip("The amount of health that, after dropping below, the player's health will not regenerate past.")]
    public float healthRegenCutoff;
    [Tooltip("The rate at which the player's health regenerates.")]
    public float regenRate;
    [Tooltip("The amount of time passed since the player last took damage.")]
    public float timeSinceTakenDamage;
    [Tooltip("A boolean determining whether or not the player is currently able to regenerate health.")]
    public bool canRegen;
    [Tooltip("A boolean determining whether or not the player's health can regeneate to the maximum health value.")]
    public bool canRegenToMax;
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
    [Tooltip("The position at which the player's melee weapon is held.")]
    public Transform meleeHoldPoint;
    [Tooltip("The array in which all of the player's weapons are stored.")]
    public GameObject[] weaponsArray;
    [Tooltip("The weapon the player currently has equipped.")]
    public GameObject activeWeapon;
    private int activeWeaponIndex;
    private float timeSinceLastPress;
    private float prototypeSwitchTimeout;
    private bool preparingToSwapWeapon;
    #endregion
    //Abilities
    #region
    [Header("Ability Management")]
    [Tooltip("The amount of time remaining before the player can use their blink ability again.")]
    public float blinkCooldownTime;
    private bool canBlink;
    [HideInInspector]
    public float blinkCooldownRounded;
    [Tooltip("The amount of time remaining before the player can use their mine ability again.")]
    public float mineRechargeTime;
    [HideInInspector]
    public float mineRechargeRounded;
    [Tooltip("The prefab GameObject of the Blinkball.")]
    public GameObject blinkballPrefab;
    [Tooltip("The prefab GameObject of the Mine.")]
    public GameObject minePrefab;
    [Space]
    #endregion
    //Sounds
    #region
    [Header("Sounds")]
    [Tooltip("The AudioSource to play while the player is moving.")]
    public AudioSource walkingSound;
    [Tooltip("The AudioClip to play when the player toggles their flashlight.")]
    public AudioClip flashlightSound;
    [Tooltip("The AudioClip to play when the player uses their melee attack.")]
    public AudioClip meleeSound;
    [Tooltip("The AudioClip to play when the player falls to the ground.")]
    public AudioClip landingSound;
    [Space]
    #endregion
    //Miscellaneous Variables
    #region
    [Header("Miscellaneous")]
    [Tooltip("The flashlight object attached to the player.")]
    public GameObject flashlight;
    private bool flashlightToggle;
    private Animator playerAnimator;
    private GameController gameController;
    private UIController uIController;
    private Vector3 standingScale, crouchingScale;
    private Transform playerFeet;
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
        if(healthRegenCutoff > maxHealth) { Debug.LogWarning("The health regen cutoff value is greater than the maximum health value of" + gameObject + ", this player's health will regen as normal."); }
        currentHealth = maxHealth;
        //Sets the first weapon in the array of weapons to active, and sets it as the player's active weapon to feed information through to the weapon and the UI Controller.
        weaponsArray[activeWeaponIndex].SetActive(true);
        activeWeapon = weaponsArray[activeWeaponIndex];
        //Assigns the animator component to the player.
        playerAnimator = gameObject.GetComponent<Animator>();
        //Finds and assigns the UIController
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        //Assigns the CharacterController component of the player.
        characterController = gameObject.GetComponent<CharacterController>();
        //Sets canBlink to true so that the player is able to use the ability immediately when the game loads.
        canBlink = true;
        //Sets the player's flashlight boolean to false as the flashlight starts turned off.
        flashlightToggle = false;
        //Sets the player's standing and crouching sizes
        standingScale = transform.localScale;
        crouchingScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
    }

    private void AssignNullVariables()
    {
        if (gravityForce == 0) { gravityForce = 9.81f; }
        if (gravityMultiplier == 0) { gravityMultiplier = 1f; }
        if (groundDistance == 0) { groundDistance = 0.4f; }
        if (jumpHeight == 0) { jumpHeight = 3f; }
        if (maxHealth == 0) { maxHealth = 100f; }
        if (regenRate == 0) { regenRate = 5f; }
        if (prototypeSwitchTimeout == 0) { prototypeSwitchTimeout = 0.25f; }
        if (moveSpeed == 0) { moveSpeed = 4f; }
        if (moveSpeedMultiplier == 0) { moveSpeedMultiplier = 1f; }
        if (sprintSpeedMultiplier == 0) { sprintSpeedMultiplier = 1f; }
        if (crouchSpeedMultiplier == 0) { crouchSpeedMultiplier = -0.5f; }
        if (wadingSpeedMultiplier == 0) { wadingSpeedMultiplier = -0.3f; }
        if (underwaterSpeedMultiplier == 0) { underwaterSpeedMultiplier = -0.2f; }
        if (mouseSensitivity == 0) { mouseSensitivity = 100f; }
        if (blinkCooldownTime == 0) { blinkCooldownTime = 5f; }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState == PlayerStates.deadState || playerState == PlayerStates.pausedState)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if(playerState == PlayerStates.climbingState || playerState == PlayerStates.standardState)
        {
            CameraControls();
            CheckGrounded();
            HealthRegen();
            PlayerSounds();
            CooldownTimers();
            PlayerInput();
        }
        else if(playerState == PlayerStates.climbingState)
        {
            ClimbingControls();
        }
        else if(playerState == PlayerStates.standardState)
        {
            MovementControls();
            ApplyGravity();
        }
    }

    public virtual void MovementControls()
    {
        if(isGrounded || (!isGrounded && !isJumping))
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            movement = transform.right * xInput + transform.forward * zInput;
            //Moves the player by the newly calculated movement vector, applying the movement speed and any multipliers and using deltaTime to make movement non-framerate dependent
            characterController.Move(movement * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
        }
        else if(!isGrounded && isJumping)
        {
            //Doesn't get the current input values to ensure the player can't change direction or strafe mid-jump
            movement = transform.right * xInput + transform.forward * zInput;
            //Uses the speed multiplier from the frame before the player jumped to prevent players from changing speed or strafing in mid-air
            characterController.Move(movement * jumpingMoveSpeed * multiplierBeforeJump * Time.deltaTime);
        }
        //If the sprint button is held and the player is not already sprinting, the sprint speed modifier is added to the movement speed multiplier and the isSprinting is set to true
        //Checking to see if the player is already sprinting prevents the speed from being added more than once
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            moveSpeedMultiplier += sprintSpeedMultiplier;
            isSprinting = true;
        }
        //If the player release the sprint button while they are sprinting, the sprint speed modifier is subtracted from the movement speed multiplier and isSprinting is set to false
        //Checking to see if the player is already sprinting prevents the modifier from being subtracted more than once
        if(Input.GetButtonDown("Sprint") && isSprinting)
        {
            moveSpeedMultiplier -= sprintSpeedMultiplier;
            isSprinting = false;
        }
        //If the player presses the jump button and are currently standing on the ground, their current movement speed multiplier is saved and their vertical velocity is set to
        //the square root of the player's jumpheight doubled, multiplied by the gravity force in order to create a more realistic jump without using Unity physics
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            //Speed multiplier is saved so it can be used to move the player without them being able to change it in mid air
            multiplierBeforeJump = moveSpeedMultiplier;
            currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);
        }
    }

    public virtual void PlayerInput()
    {

    }

    public void ApplyGravity()
    {
        //Adds the value of gravity to the player's current velocity in the Y axis
        currentVelocity.y += gravityForce;
        //Moves the player downwards on the Y axis by the value of current velocity, using the gravity multiplier to add any gravity affects and using deltaTime to make gravity non-framerate dependent
        characterController.Move(currentVelocity * gravityMultiplier * Time.deltaTime);
    }

    public virtual void ClimbingControls()
    {

    }

    public virtual void CameraControls()
    {

    }

    public void PlayerSounds()
    {

    }

    public void CooldownTimers()
    {

    }

    public void CheckGrounded()
    {

    }

    public void HealthRegen()
    {

    }
}
