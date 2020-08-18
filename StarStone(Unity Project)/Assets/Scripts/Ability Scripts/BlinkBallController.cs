using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkBallController : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: BlinkBallController                              |
    // Script Author: James Smale                                    |
    // Purpose: Manages the movement and teleportation effects of    |
    //          the Blinkball
    //***************************************************************|

    //Physics
    #region
    [Header("Physics")]
    [Tooltip("The physics layer of the ground.")]
    public LayerMask groundLayer;
    private Vector3 teleportTarget;
    private Rigidbody rigidBody;
    [Space]
    #endregion
    //Floats
    #region
    [Header("Floats")]
    [Tooltip("The amount of time left before this object is destroyed.")]
    public float lifeTimeRemaining;
    [Tooltip("The vertical offset from the teleportation destination.")]
    public float heightOffset;
    [Tooltip("The speed at which the player is teleported.")]
    public float teleportSpeed;
    [Space]
    #endregion
    //Audio
    #region
    [Header("Audio")]
    [Tooltip("The sound played when the player teleports.")]
    public AudioClip teleportSound;
    [Space]
    #endregion
    //Player Information
    #region
    [HideInInspector]
    public GameObject playerObject;
    private Transform cameraTransform;
    #endregion
    //Bools
    #region
    private bool isTeleporting;
    #endregion
    //UI
    #region
    private UIController uiController;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(teleportSpeed == 0) { teleportSpeed = 200f; }
        uiController = playerObject.GetComponent<UIController>();
        cameraTransform = playerObject.GetComponentInChildren<Camera>().transform;
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.AddForce(cameraTransform.forward * 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTeleporting)
        {
            //Reduces the object's lifetime by the amount of time passed since the last frame
            lifeTimeRemaining -= Time.deltaTime;
            if (lifeTimeRemaining <= 0)
            {
                //When the lifetime reaches zero, the object is destroyed
                Destroy(gameObject);
            }
        }
        else
        {
            TeleportPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the object collides with the floor, its position is saved as the teleport target, with the height offset added to the Y to avoid the player getting stuck in the ground
        if(collision.gameObject.layer == 8)
        {
            teleportTarget = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
            //Stops the blinkBall from simulating physics
            rigidBody.isKinematic = true;
            //Turns on the speed lines on the player's UI to simulate movement
            uiController.ToggleSpeedLines(true);
            //The blinkball is set to teleport the player to the target destination
            isTeleporting = true;
            //Plays the teleport sound at the position
            AudioSource.PlayClipAtPoint(teleportSound, playerObject.transform.position);
        }
    }

    private void TeleportPlayer()
    {
        //Determines the distance to move the player this step by multiplying the teleportSpeed by deltaTime to make it framerate independent
        float moveDist = teleportSpeed * Time.deltaTime;
        //Moves the player towards the teleport target by the previously calculated move distance
        playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, teleportTarget, moveDist);
        //If the player has essentially reached its target, the speedlines are disabled and the Blinkball destroyed
        if(Vector3.Distance(playerObject.transform.position, teleportTarget) < 0.001f)
        {
            uiController.ToggleSpeedLines(false);
            Destroy(gameObject);
        }
    }

}
