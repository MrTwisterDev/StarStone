using System.Collections;
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
        
        defaultMoveSpeed = 12.5f;
        moveSpeed = defaultMoveSpeed;
        sprintSpeed = 17.5f;
        crouchSpeed = 6.25f;

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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            characterController.height /= 2;
            transform.localScale = crouchingScale;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height *= 2;
            transform.localScale = standingScale;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
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
