using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale;
    public float gravityMultiplier;
    private float groundDistance;

    public float moveSpeed, jumpHeight;

    public float moveSpeedMultiplier, crouchingMultiplier, sprintingMultiplier, swimmingMultiplier, wadingMultiplier, climbingMultiplier;

    private float xInput, yInput, zInput;

    private float mouseX, mouseY, xRotation;

    public float mouseSensitivity;

    private bool isGrounded, isSprinting, isCrouching, isWading, isSwimming, isClimbing;

    public Transform weaponHoldPoint, adsHoldPoint;
    public GameObject[] weaponsArray;
    private int activeWeaponIndex;
    private float weaponSwapCooldown, timeSinceLastPress, prototypeSwapTimeout;
    private bool hasSwappedWeapon, preparingToSwap;

    public GameObject blinkBall;

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

        timeSinceLastPress = 0f;
        prototypeSwapTimeout = 0.25f;

        weaponsArray[activeWeaponIndex].SetActive(true);

        moveSpeed = 4f;
        moveSpeedMultiplier = 1f;
        sprintingMultiplier = 1f;
        crouchingMultiplier = -0.5f;
        wadingMultiplier = -0.3f;
        swimmingMultiplier = -0.2f;

        mouseSensitivity = 100f;

        standingScale = transform.localScale;
        crouchingScale = new Vector3(standingScale.x, standingScale.y / 2, standingScale.z);

        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (hasSwappedWeapon)
        {
            weaponSwapCooldown -= Time.deltaTime;
            if(weaponSwapCooldown <= 0f)
            {
                hasSwappedWeapon = false;
                weaponSwapCooldown = 0.25f;
            }
        }
        if (preparingToSwap)
        {
            WeaponSwapTimer();
        }
    }

    private bool IsClimbingLadder()
    {
        Debug.DrawRay(ladderChecker.position, ladderChecker.forward);
        if(Physics.Raycast(ladderChecker.position, ladderChecker.forward, 0.5f, ladderLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ClimbingControls()
    {
        yInput = Input.GetAxis("Vertical");

        Vector3 movement = transform.up * yInput;
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


        if (Input.GetMouseButton(1))
        {
            weaponsArray[activeWeaponIndex].transform.position = adsHoldPoint.transform.position;
        }

        if (Input.GetMouseButtonUp(1))
        {
            weaponsArray[activeWeaponIndex].transform.position = weaponHoldPoint.transform.position;
        }

        if (Input.GetMouseButtonDown(2))
        {
            GameObject thrownBall = Instantiate(blinkBall, cameraTransform.position, cameraTransform.rotation);
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            baseWeaponClass _currentWeaponScript = weaponsArray[activeWeaponIndex].GetComponent<baseWeaponClass>();
            _currentWeaponScript.reloadWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !hasSwappedWeapon)
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

}
