using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Soul Controller                                  |
    // Script Author: James Smale                                    |
    // Purpose: Handle soul movement and guide it to its destination |
    //***************************************************************|

    public Transform soulDestination;
    private Transform startingPosition;

    private float startingTime;
    private float journeyLength;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        soulDestination = GameObject.Find("SoulSuckOMatic3000").transform;
        startingPosition = transform;

        journeyLength = Vector3.Distance(startingPosition.position, soulDestination.position);

        startingTime = Time.time;
        moveSpeed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination();
        if(transform.position == soulDestination.position)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToDestination()
    {
        
        float distanceCovered = (Time.time - startingTime) * moveSpeed;

        float journeyCompleted = distanceCovered / journeyLength;

        transform.position = Vector3.Lerp(transform.position, soulDestination.position, journeyCompleted);
    }

}
