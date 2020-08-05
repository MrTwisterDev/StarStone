using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextController : MonoBehaviour
{
    //The GameObject of the player character
    public GameObject playerObject;
    public GameObject textObject;

    // Update is called once per frame
    void Update()
    {
        //Calculates the object's current distance from the player
        float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, playerObject.transform.position);
        //If the player is 5 meters away or closer and the text is currently disabled, the text object is set to active
        if(distanceFromPlayer <= 5f && !textObject.activeSelf)
        {
            textObject.SetActive(true);
        }
        //If the player is further than 5 meters away and the text is active, it is disabled
        else if(distanceFromPlayer > 5f && textObject.activeSelf)
        {
            textObject.SetActive(false);
        }
    }
}
