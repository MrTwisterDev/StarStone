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
        float distanceFromPlayer = Vector3.Distance(gameObject.transform.position, playerObject.transform.position);
        if(distanceFromPlayer <= 5f && !textObject.activeSelf)
        {
            textObject.SetActive(true);
        }
        else if(distanceFromPlayer > 5f && textObject.activeSelf)
        {
            textObject.SetActive(false);
        }
        Debug.Log(distanceFromPlayer);
    }
}
