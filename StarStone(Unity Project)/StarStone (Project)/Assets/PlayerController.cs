using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float gravityScale;
    private float gravityMultiplier;
    private float groundDistance;

    private bool isGrounded;

    private LayerMask groundLayer;

    private Vector3 currentVelocity;

    public Transform groundChecker;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        gravityScale = -9.81f;
        gravityMultiplier = 1f;
        groundDistance = 0.4f;

        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        ApplyGravity();
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
