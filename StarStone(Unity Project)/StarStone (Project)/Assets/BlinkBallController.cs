﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBallController : MonoBehaviour
{

    public LayerMask groundLayer;

    private GameObject playerObject;

    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("playerCapsule");
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.AddForce(playerObject.transform.forward * 1000f); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log("Collided with the ground!");
            Vector3 blinkTarget = new Vector3(gameObject.transform.position.x, playerObject.transform.position.y, gameObject.transform.position.z);
            playerObject.transform.position = blinkTarget;
            Destroy(gameObject);
        }
        Debug.Log("Collided with something that wasn't the floor.");
    }

}
