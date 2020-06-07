using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    private float mouseX, mouseY, xRotation, zRotation;

    public float mouseSensitivity;

    public Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        zRotation -= mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        zRotation = Mathf.Clamp(zRotation, -160f, 160f);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            transform.localRotation = Quaternion.Euler(xRotation, zRotation, 0f);
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            playerBody.Rotate(Vector3.up * xRotation);
            transform.localRotation = Quaternion.Euler(mouseX, 0f, 0f);
        }
    }
}
