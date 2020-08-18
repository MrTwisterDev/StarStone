using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorScript : MonoBehaviour
{
    
    public float elevatorSpeed;
    public Transform elevatorEndLocation;
    private Vector3 elevatorStartLocation;

    [Header("Elevator Settings")]
    [Tooltip("Does the elevator return to it's position")]
    public bool returningElevator;
    private bool isReturning = false; //Is the elevator returning from it's position, false as it needs to reach it's destination first


    // Start is called before the first frame update
    void Start()
    {
        elevatorStartLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (returningElevator)
        {
            if (gameObject.transform.position == elevatorEndLocation.transform.position)
            {
                isReturning = true;
            }
            else if (gameObject.transform.position == elevatorStartLocation)
            {
                isReturning = false;
            }

            switch (isReturning)
            {
                case true:
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, elevatorStartLocation, elevatorSpeed * Time.deltaTime);
                    break;

                case false:
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, elevatorEndLocation.transform.position, elevatorSpeed * Time.deltaTime);
                    break;
            }
        }
        else
        {

            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, elevatorEndLocation.transform.position, elevatorSpeed * Time.deltaTime);

            //Depricated code, would never actually travel to the exact position
            //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, elevatorEndLocation.transform.position, Time * elevatorSpeed);
        }

    }
}
