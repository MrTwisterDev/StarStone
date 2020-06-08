﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float gravityScale;
    private float gravityMultiplier;
    private float groundDistance;

    public float moveSpeed, defaultMoveSpeed, sprintSpeed, crouchSpeed, jumpHeight;

    private float xInput, zInput;

    private float mouseX, mouseY, xRotation, zRotation;

    private float mouseSensitivity;

    private bool isGrounded;

    public Transform weaponHoldPoint;
    public GameObject[] weaponsArray;
    public GameObject activeWeapon;
    private int activeWeaponIndex;
    private float weaponSwapCooldown, timeSinceLastPress, prototypeSwapTimeout;
    private bool hasSwappedWeapon, preparingToSwap;

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

        activeWeapon = Instantiate(weaponsArray[0], weaponHoldPoint);
        activeWeapon.transform.parent = weaponHoldPoint;
        timeSinceLastPress = 0f;
        prototypeSwapTimeout = 0.25f;

        defaultMoveSpeed = 4f;
        moveSpeed = defaultMoveSpeed;
        sprintSpeed = 10f;
        crouchSpeed = 1.5f;

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
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerControls()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * xInput + transform.forward * zInput;
        characterController.Move(movement * moveSpeed * Time.deltaTime);

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
                activeWeaponIndex = weaponsArray.Length -1;
                Destroy(activeWeapon);
                activeWeapon = Instantiate(weaponsArray[activeWeaponIndex], weaponHoldPoint);
                activeWeapon.transform.parent = weaponHoldPoint;
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
            hasSwappedWeapon = true;
            preparingToSwap = false;
            //Thomas' Remastered Code
            activeWeaponIndex++;
            if(activeWeaponIndex == weaponsArray.Length || activeWeaponIndex == weaponsArray.Length - 1)
            {
                activeWeaponIndex = 0;
            }
            Destroy(activeWeapon);
            activeWeapon = Instantiate(weaponsArray[activeWeaponIndex], weaponHoldPoint);
            activeWeapon.transform.parent = weaponHoldPoint;



            //James' Original Code
            /*
            if (activeWeaponIndex == 0)
            {
                activeWeaponIndex = 1;
                Destroy(activeWeapon);
                activeWeapon = Instantiate(weaponsArray[activeWeaponIndex], weaponHoldPoint);
                activeWeapon.transform.parent = weaponHoldPoint;
            }
            else if (activeWeaponIndex == 1 || activeWeaponIndex == 2)
            {
                activeWeaponIndex = 0;
                Destroy(activeWeapon);
                activeWeapon = Instantiate(weaponsArray[activeWeaponIndex], weaponHoldPoint);
                activeWeapon.transform.parent = weaponHoldPoint;
            } */

            //Thomas's Original Rewrite
            /*
            switch (activeWeaponIndex)
            {
                case 0:
                    activeWeaponIndex = 1;
                    Destroy(activeWeapon);
                    activeWeapon = Instantiate(weaponsArray[activeWeaponIndex], weaponHoldPoint);
                    activeWeapon.transform.parent = weaponHoldPoint;
                    break;
                default:
                    activeWeaponIndex = 0;
                    Destroy(activeWeapon);
                    activeWeapon = Instantiate(weaponsArray[activeWeaponIndex], weaponHoldPoint);
                    activeWeapon.transform.parent = weaponHoldPoint;
                    break;

            } */

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
