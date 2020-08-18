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

    private GameController gameController;

    private float startingTime;
    private float journeyLength;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //If the soul's destination is undefined, it finds the object in the scene
        if (soulDestination == null) { soulDestination = GameObject.FindGameObjectWithTag("MainGenerator").transform; }
        //Finds and assigns the game controller's script to the soul
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //Sets the initial transform as the starting position
        startingPosition = transform;
        //Calculates the full length of the journey based on the start and end positions
        journeyLength = Vector3.Distance(startingPosition.position, soulDestination.position);
        //Defines the starting time
        startingTime = Time.time;
        //Defines the soul's move speed
        moveSpeed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDestination();
    }

    private void MoveToDestination()
    {
        //Calculates the amount of time passed since the journey began
        float distanceCovered = (Time.time - startingTime) * moveSpeed;
        //calculates the amount of the journey completed so far
        float journeyCompleted = distanceCovered / journeyLength;
        //moves the soul through the scene using the previously calculated value
        transform.position = Vector3.Lerp(transform.position, soulDestination.position, journeyCompleted);
    }

    private void OnTriggerEnter(Collider other)
    {
        //When the soul collides with the main generator, the number of souls inside the generator is incremented and the soul object is destroyed
        if(other.transform.position == soulDestination.transform.position)
        {
            gameController.soulsInGenerator++;
            Destroy(gameObject);
        }
    }

}
