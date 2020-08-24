using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialTextController : MonoBehaviour
{
    //The GameObject of the player character
    public GameObject[] playerObject;
    public GameObject textObject;
    public float rotationSpeed;

    private Transform targetTransform;

    public void Start()
    {
        if(rotationSpeed == 0){ rotationSpeed = 0.5f; }
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                playerObject = new GameObject[2];
                playerObject[0] = GameObject.Find("PlayerOne");
                playerObject[1] = GameObject.Find("PlayerTwo");
                break;
            case 2:
                playerObject = new GameObject[1];
                playerObject[0] = GameObject.FindGameObjectWithTag("Player");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetActiveOnProximity();
        RotateToFacePlayer();
    }

    private void RotateToFacePlayer()
    {   
        float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, playerObject[0].transform.position);
        if(playerObject.Length > 1 && playerObject[1] != null)
        {
            float distanceFromPlayerTwo = Vector3.Distance(gameObject.transform.position, playerObject[1].transform.position);
            if(distanceFromPlayer < distanceFromPlayerTwo)
            {
                targetTransform = playerObject[1].transform;
            }
            else
            {
                targetTransform = playerObject[0].transform;
            }
        }
        else
        {
            targetTransform = playerObject[0].transform;
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetTransform.position - transform.position);
        float speed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed);
    }

    private void SetActiveOnProximity()
    {
        //Calculates the object's current distance from the player
        float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, playerObject[0].transform.position);
        if(playerObject.Length > 1 && playerObject[1] != null)
        { 
            float distanceFromPlayerTwo = Vector3.Distance(gameObject.transform.position, playerObject[1].transform.position);
            if(distanceFromPlayer <= 5f || distanceFromPlayerTwo <= 5f && !textObject.activeSelf)
            {
                textObject.SetActive(true);
            }
            else if(distanceFromPlayer > 5f && distanceFromPlayerTwo > 5f && textObject.activeSelf)
            {
                textObject.SetActive(false);
            }
        }
        else
        {
            //If the player is 5 meters away or closer and the text is currently disabled, the text object is set to active
            if (distanceFromPlayer <= 5f && !textObject.activeSelf)
            {
                textObject.SetActive(true);
            }
            //If the player is further than 5 meters away and the text is active, it is disabled
            else if (distanceFromPlayer > 5f && textObject.activeSelf)
            {
                textObject.SetActive(false);
            }
        }
    }
}
