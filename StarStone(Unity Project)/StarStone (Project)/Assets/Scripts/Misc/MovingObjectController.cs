using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectController : MonoBehaviour
{
    //Destination Vectors
    [Tooltip("An array containing all of the destination vectors that the object should move towards. If the movement is looping, the first entry should be the objects initial position.")]
    public Vector3[] destinationVectors;
    //Booleans
    [Tooltip("Dictates whether or not the object's movement should be looping. Setting this to true after movement has ended will not work.")]
    public bool loopingMovement;
    [Tooltip("Dictates whether or not the object should currently be moving.")]
    public bool isMoving;
    private bool hasAudio;
    //Integers
    [Tooltip("The array index used to determine the object's current destination.")]
    public int destinationIndex;
    //Floats
    [Tooltip("The speed at which the object moves.")]
    public float moveSpeed;
    //Sound
    [Tooltip("The audiosource attached to this object.")]
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Prints an error to the console to warn that no destinations have been defined for this object
        if(destinationVectors.Length == 0) { Debug.LogError(gameObject + " does not have any destinations defined."); }
        //Sets the movement speed to 1 if it is undefined to ensure some movement occurs
        if(moveSpeed <= 0) { moveSpeed = 1f; }
        destinationIndex = 0;
        if(audioSource == null) { hasAudio = false; } else { hasAudio = true; }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //Creates a new float using moveSpeed and deltaTime to make object movement framerate independent
            float moveDistance = moveSpeed * Time.deltaTime;
            //Moves the object one step closer to its destination using the above float
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destinationVectors[destinationIndex], moveDistance);
            if(hasAudio && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            //If the object has reached its destination, the destinationIndex is incremented by one to use the next destination vector
            if(Vector3.Distance(gameObject.transform.position, destinationVectors[destinationIndex]) < 0.001f)
            {
                destinationIndex++;
                //If the new index is outside of the length of the array and the movement isn't looping, then the object stops moving
                if (destinationIndex >= destinationVectors.Length && !loopingMovement)
                {
                    isMoving = false;
                    if (hasAudio)
                    {
                        audioSource.Stop();
                    }
                }
                //Otherwise, if movement is looping, the index is reset to 0 and the object moves back towards its origin
                else if (loopingMovement)
                {
                    destinationIndex = 0;
                }
            }
        }
    }
}
