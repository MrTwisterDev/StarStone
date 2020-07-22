using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBallController : MonoBehaviour
{

    public LayerMask groundLayer;

    public float lifeTimeRemaining;

    public float heightOffset;

    [HideInInspector]
    public GameObject playerObject;
    private Transform cameraTransform;

    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("playerCapsule");
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.AddForce(cameraTransform.forward * 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimeRemaining -= Time.deltaTime;
        if(lifeTimeRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Vector3 blinkTarget = new Vector3(gameObject.transform.position.x, transform.position.y + heightOffset, gameObject.transform.position.z);
            playerObject.transform.position = blinkTarget;
            Destroy(gameObject);
        }
    }

}
