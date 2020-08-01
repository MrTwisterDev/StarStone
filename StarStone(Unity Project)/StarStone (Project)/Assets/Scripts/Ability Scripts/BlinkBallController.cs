using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkBallController : MonoBehaviour
{
    //Physics
    public LayerMask groundLayer;
    private Vector3 teleportTarget;
    private Rigidbody rigidBody;
    //Floats
    public float lifeTimeRemaining;
    public float heightOffset;
    public float teleportSpeed;
    //Audio
    public AudioClip teleportSound;
    //Player Information
    [HideInInspector]
    public GameObject playerObject;
    private Transform cameraTransform;
    //Bools
    private bool isTeleporting;
    //UI
    private UIController uiController;

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
            lifeTimeRemaining -= Time.deltaTime;
            if (lifeTimeRemaining <= 0)
            {
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
        if(collision.gameObject.layer == 8)
        {
            teleportTarget = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
            rigidBody.isKinematic = true;
            uiController.ToggleSpeedLines(true);
            isTeleporting = true;
            AudioSource.PlayClipAtPoint(teleportSound, playerObject.transform.position);
        }
    }

    private void TeleportPlayer()
    {
        float moveDist = teleportSpeed * Time.deltaTime;
        playerObject.transform.position = Vector3.MoveTowards(playerObject.transform.position, teleportTarget, moveDist);
        if(Vector3.Distance(playerObject.transform.position, teleportTarget) < 0.001f)
        {
            uiController.ToggleSpeedLines(false);
            Destroy(gameObject);
        }
    }

}
