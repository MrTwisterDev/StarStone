using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale;
    public float gravityMultiplier;
    private float groundDistance;

    public float moveSpeed, jumpHeight;

    public float maxHealth;
    public float currentHealth;
    public float healthRegenCutoff;
    public float regenRate;
    public float timeSinceTakenDamage;
    public bool  canRegen;
    public bool  canRegenToMax;

    public float moveSpeedMultiplier, crouchingMultiplier, sprintingMultiplier, swimmingMultiplier, wadingMultiplier, climbingMultiplier;

    private float xInput, yInput, zInput;

    public float mouseX, mouseY, xRotation;

    public float mouseSensitivity;

    public float blinkCooldownTime, blinkCooldownTimeRounded;

    private bool isGrounded, isSprinting, isCrouching, isWading, isSwimming, isClimbing, canBlink;

    public Transform weaponHoldPoint, adsHoldPoint;
    public GameObject[] weaponsArray;
    public GameObject activeWeapon;
    private int activeWeaponIndex;
    private float timeSinceLastPress, prototypeSwapTimeout;
    private bool preparingToSwap;

    public GameObject blinkBall;

    public Transform fistPosition;
    private Animator fistAnimator;

    public UIController uiController;

    public LayerMask groundLayer, ladderLayer;

    private Vector3 currentVelocity, standingScale, crouchingScale;

    public Transform groundChecker, ladderChecker, cameraTransform;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        gravityScale = -9.81f * 2;
        gravityMultiplier = 1f;
        groundDistance = 0.25f;

        if(jumpHeight == 0) {jumpHeight = 3f;};
        if(healthRegenCutoff == 0) { healthRegenCutoff = 70f; }
        if(maxHealth == 0) { maxHealth = 100; }
        if(healthRegenCutoff > maxHealth) { Debug.LogWarning("The cutoff for health regen is greater than the maximum health value."); }
        if(regenRate == 0) { regenRate = 5f; }

        currentHealth = maxHealth;

        timeSinceLastPress = 0f;
        prototypeSwapTimeout = 0.25f;

        weaponsArray[activeWeaponIndex].SetActive(true);
        activeWeapon = weaponsArray[activeWeaponIndex];

        moveSpeed = 4f;
        moveSpeedMultiplier = 1f;
        sprintingMultiplier = 1f;
        crouchingMultiplier = -0.5f;
        wadingMultiplier = -0.3f;
        swimmingMultiplier = -0.2f;

        mouseSensitivity = 100f;

        fistAnimator = GameObject.Find("Fist").GetComponent<Animator>();

        canBlink = true;
        blinkCooldownTime = 5f;

        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();

        standingScale = transform.localScale;
        crouchingScale = new Vector3(standingScale.x, standingScale.y / 2, standingScale.z);

        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(characterController.velocity);
        CameraControls();
        if (IsClimbingLadder() == false)
        {
            PlayerControls();
            ApplyGravity();
        }
        else
        {
            ClimbingControls();
        }
        CheckGrounded();
        HealthRegen();
        CooldownTimers();

        if (preparingToSwap)
        {
            WeaponSwapTimer();
        }
        if (currentHealth <= healthRegenCutoff)
        {
            canRegenToMax = false;
        }
        else
        {
            canRegenToMax = true;
        }
    }

    private bool IsClimbingLadder()
    {
        if(Physics.Raycast(ladderChecker.position, ladderChecker.forward, 0.5f, ladderLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CooldownTimers()
    {
        if (!canBlink)
        {
            blinkCooldownTime -= Time.deltaTime;
            blinkCooldownTimeRounded = Mathf.Round(blinkCooldownTime * 100) / 100;
            if (blinkCooldownTime <= 0)
            {
                canBlink = true;
                blinkCooldownTime = 5f;
            }
            uiController.UpdateBlinkTimer();
        }
        if (!canRegen)
        {
            timeSinceTakenDamage += Time.deltaTime;
            if (timeSinceTakenDamage >= 5)
            {
                canRegen = true;
            }
        }
    }

    private void ClimbingControls()
    {
        yInput = Input.GetAxis("Vertical");
        xInput = Input.GetAxis("Horizontal");

        Vector3 movement = transform.right * xInput + transform.up * yInput;
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void CameraControls()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        weaponHoldPoint.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        adsHoldPoint.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerControls()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * xInput + transform.forward * zInput;
        characterController.Move(movement * moveSpeed * moveSpeedMultiplier * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            weaponsArray[activeWeaponIndex].GetComponent<baseWeaponClass>().useWeapon();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (weaponsArray[activeWeaponIndex].GetComponent<build_a_weapon>().typeOfWeapon == build_a_weapon.typesOfWeapon.spreadShot)
            {
                weaponsArray[activeWeaponIndex].GetComponent<build_a_weapon>().spreadShotLock = false;
            }
        }


        if (Input.GetMouseButton(1))
        {
            weaponsArray[activeWeaponIndex].transform.position = adsHoldPoint.transform.position;
        }

        if (Input.GetMouseButtonUp(1))
        {
            weaponsArray[activeWeaponIndex].transform.position = weaponHoldPoint.transform.position;
        }

        if (Input.GetMouseButtonDown(2) && canBlink)
        {
            GameObject thrownBall = Instantiate(blinkBall, cameraTransform.position, cameraTransform.rotation);
            canBlink = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting)
        {
            moveSpeedMultiplier += sprintingMultiplier;
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && isSprinting)
        {
            moveSpeedMultiplier -= sprintingMultiplier;
            isSprinting = false;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Thomas' Work~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        if (Input.GetKeyDown(KeyCode.R))
        {
            baseWeaponClass _currentWeaponScript = weaponsArray[activeWeaponIndex].GetComponent<baseWeaponClass>();
            _currentWeaponScript.reloadWeapon();
            uiController.UpdateAmmoText();
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\

        if (Input.GetKeyDown(KeyCode.F))
        {
            fistAnimator.SetTrigger("Punch");
            RaycastHit rayHit;
            Debug.DrawRay(fistPosition.position, transform.forward);
            if(Physics.Raycast(fistPosition.position, transform.forward, out rayHit, 0.75f))
            {
                if(rayHit.collider.gameObject.tag == "Enemy")
                {
                    Debug.Log("Hit an enemy!");
                    Destroy(rayHit.collider.gameObject);
                }
                else
                {
                    Debug.Log("Hit not an enemy!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            timeSinceLastPress = 0f;
            if (!preparingToSwap)
            {
                preparingToSwap = true;
            }
            else if(preparingToSwap && timeSinceLastPress <= prototypeSwapTimeout)
            {
                weaponsArray[activeWeaponIndex].SetActive(false);
                activeWeaponIndex = weaponsArray.Length -1;
                weaponsArray[activeWeaponIndex].SetActive(true);
                preparingToSwap = false;
                timeSinceLastPress = 0f;
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) && !isCrouching)
        {
            characterController.height = 1;
            transform.localScale = crouchingScale;
            moveSpeedMultiplier += crouchingMultiplier;
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            characterController.height = 2;
            transform.localScale = standingScale;
            moveSpeedMultiplier -= crouchingMultiplier;
            isCrouching = false;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
        }

    }

    private void WeaponSwapTimer()
    {
        timeSinceLastPress += Time.deltaTime;
        if(timeSinceLastPress > prototypeSwapTimeout)
        {
            weaponsArray[activeWeaponIndex].SetActive(false);
            activeWeaponIndex++;
            if(activeWeaponIndex >= weaponsArray.Length - 1)
            {
                activeWeaponIndex = 0;
            }
            weaponsArray[activeWeaponIndex].SetActive(true);
            activeWeapon = weaponsArray[activeWeaponIndex];
            uiController.GetChangedWeapon();
            uiController.UpdateAmmoText();
            preparingToSwap = false;
            timeSinceLastPress = 0f;
        }
    }
    private void ApplyGravity()
    {
        currentVelocity.y += gravityScale * Time.deltaTime;
        characterController.Move(currentVelocity * gravityMultiplier * Time.deltaTime);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayer);
        if (isGrounded && currentVelocity.y < 0)
        {
            currentVelocity.y = -2f;
        }
    }

    private void HealthRegen()
    {
        if(canRegen && currentHealth < maxHealth && canRegenToMax)
        {
            currentHealth += regenRate * Time.deltaTime;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        else if(canRegen && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            if(currentHealth > healthRegenCutoff)
            {
                currentHealth = healthRegenCutoff;
            }
        }
        uiController.UpdateHealthbar();
    }
}
