using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale;
    public float gravityMultiplier;
    private float groundDistance;

    public float moveSpeed, defaultMoveSpeed, sprintSpeed, crouchSpeed, underWaterSpeed, wadingSpeed, jumpHeight;

    private float xInput, zInput;

    private float mouseX, mouseY, xRotation, zRotation;

    public float mouseSensitivity;

    private bool isGrounded;

    public Transform weaponHoldPoint, adsHoldPoint;
    public GameObject[] weaponsArray;
    private int activeWeaponIndex;
    private float weaponSwapCooldown, timeSinceLastPress, prototypeSwapTimeout;
    private bool hasSwappedWeapon, preparingToSwap;

    public GameObject blinkBall;

    public LayerMask groundLayer;

    private Vector3 currentVelocity, standingScale, crouchingScale;

    public Transform groundChecker, cameraTransform;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        gravityScale = -9.81f * 2;
        gravityMultiplier = 1f;
        groundDistance = 0.4f;

        if(jumpHeight == 0) {jumpHeight = 3f;};

        timeSinceLastPress = 0f;
        prototypeSwapTimeout = 0.25f;

        weaponsArray[activeWeaponIndex].SetActive(true);

        defaultMoveSpeed = 4f;
        moveSpeed = defaultMoveSpeed;
        sprintSpeed = 10f;
        crouchSpeed = 2.5f;
        wadingSpeed = 3f;
        underWaterSpeed = 2f;

        mouseSensitivity = 100f;

        standingScale = transform.localScale;
        crouchingScale = new Vector3(standingScale.x, standingScale.y / 2, standingScale.z);

        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraControls();
        PlayerControls();
        CheckGrounded();
        ApplyGravity();
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

    private void CameraControls()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        zRotation -= mouseX;

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
        characterController.Move(movement * moveSpeed * Time.deltaTime);

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = defaultMoveSpeed;
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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            characterController.height /= 2;
            transform.localScale = crouchingScale;
            moveSpeed = crouchSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height *= 2;
            transform.localScale = standingScale;
            moveSpeed = defaultMoveSpeed;
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
