using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_rotateOnAxis : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform objectTransform;

    [Header("Axis")]
    [Tooltip("Specifies which Axis to move across")]
    public bool X;
    public bool Y;
    public bool Z;
    [Header("Reverse")]
    public bool reverseRotation;
    [Header("Rotation Properties")]
    public float rotationSpeed;

    void Start()
    {
        objectTransform = gameObject.transform;
        if (reverseRotation)
        {
            rotationSpeed = -rotationSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(X == true) { transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0); }
        if(Y == true) { transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); }
        if(Z == true) { transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); }
    }
}
