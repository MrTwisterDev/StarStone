using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Tutorial Controller                              |
    // Script Author: James Smale                                    |
    // Purpose: Manages the text prompts within the tutorial scene,  |
    //          as well as the moveable objects in the scene.        |
    //***************************************************************|

    [Header("Tutorial Text")]
    [Tooltip("An array containing the texts that should be displayed during the tutorial.")]
    public string[] tutorialTexts;
    [Tooltip("The text element displayed on screen in the tutorial. This shows the tutorial text.")]
    public Text tutorialText;
    [Tooltip("An integer determining what index in the text array to access. This determines which text is displayed on-screen.")]
    public int textIndex;
    [Tooltip("The length of the delay between completing a tutorial prompt, and the prompt changing to the next one (in seconds).")]
    public float tutorialTextUpdateDelayLength;
    [Space]

    [Header("Moving Walls")]
    [Tooltip("The moving walls that move to reveal the combat training area of the tutorial.")]
    public GameObject[] firstSectionDoors;
    [Tooltip("The moving walls that move to reveal the museum area of the tutorial.")]
    public GameObject[] secondSectionDoors;

    //The delay between the tutorial prompting the player to move rooms and the tutorial text changing
    private float nextRoomDelay;
    //A boolean used to determine whether or not the text update timer should be run
    private bool runTextUpdateTimer;
    //The timer used to delay the tutorial text update
    private float tutorialTextUpdateDelay;

    private enum tutorialStates
    {
        movementTutorial,
        combatTutorial,
        abilityTutorial,
        museumTutorial
    }

    private tutorialStates currentTutorialState;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the tutorial's current state the the movement tutorial state, as it is the first in the array
        currentTutorialState = tutorialStates.movementTutorial;
        //If there is no text in the array, an error is logged
        if (tutorialTexts.Length == 0) { Debug.LogError("There are no text entries in the tutorial texts array. " + gameObject); }
        //If the length of the update delay is still 0, it has not been set and so is assigned a default value of 2
        if (tutorialTextUpdateDelayLength == 0) { tutorialTextUpdateDelayLength = 2f; }
        //Sets the delay between being told to move rooms and the next tutorial starting to 5 seconds
        nextRoomDelay = 5f;
        //Sets the index to the start of the array
        textIndex = 0;
        //Runs the text updating timer to progress the tutorial from the welcome text
        runTextUpdateTimer = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        //Checks to see what part of the tutorial the player is on and runs the appropriate methods
        switch (currentTutorialState)
        {
            case tutorialStates.movementTutorial:
                MovementTutorial();
                break;
            case tutorialStates.combatTutorial:
                CombatTutorial();
                break;
            case tutorialStates.abilityTutorial:
                AbilityTutorial();
                break;
            case tutorialStates.museumTutorial:
                MuseumTutorial();
                break;
        }
        //If the timer is running, its value is reduced by the amount of time passed since the last frame each frame
        if (runTextUpdateTimer)
        {
            tutorialTextUpdateDelay -= Time.deltaTime;
            //When the timer reaches zero, the tutorial text is updated and the timer is reset
            if(tutorialTextUpdateDelay <= 0)
            {
                UpdateTutorialText();
                runTextUpdateTimer = false;
                tutorialTextUpdateDelay = tutorialTextUpdateDelayLength;
            }
        }
    }

    public void UpdateTutorialText()
    {
        //Increments the text index by one to access the next tutorial text
        textIndex++;
        //Sets the text displayed to the string defined in the new array index
        tutorialText.text = tutorialTexts[textIndex];
    }

    public void MovementTutorial()
    {
        //Checks the value of the textIndex and looks for the relevant input
        switch (textIndex)
        {
            case 1:
                if (Input.GetAxis("PlayerOneHorizontal") > 0 || Input.GetAxis("PlayerOneVertical") > 0)
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 2:
                if (Input.GetAxis("PlayerOneCameraX") > 0 || Input.GetAxis("PlayerOneCameraY") > 0)
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 3:
                if (Input.GetButton("PlayerOneSprint") && Input.GetAxis("PlayerOneHorizontal") > 0 || Input.GetAxis("PlayerOneVertical") > 0)
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 4:
                if (Input.GetButtonDown("PlayerOneCrouch"))
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 5:
                if (Input.GetButtonDown("PlayerOneJump"))
                {
                    runTextUpdateTimer = true;
                    //Iterates through the objects in the firstSectionDoors array and triggers them to move, opening a new area
                    for (int i = 0; i < firstSectionDoors.Length; i++)
                    {
                        firstSectionDoors[i].GetComponent<MovingObjectController>().isMoving = true;
                    }
                }
                break;
            case 6:
                //Runs the nextRoomDelay timer until it reaches zero
                nextRoomDelay -= Time.deltaTime;
                if (nextRoomDelay <= 0)
                {
                    //Updates the tutorial text and sets the tutorialState to the next state, the combat tutorial
                    runTextUpdateTimer = true;
                    currentTutorialState = tutorialStates.combatTutorial;
                }
                break;
        }

    }

    public void CombatTutorial()
    {
        //Checks the value of the textIndex and looks for the relevant input
        switch (textIndex)
        {
            case 7:
                if (Input.GetAxis("PlayerOneFire") > 0)
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 8:
                if (Input.GetAxis("PlayerOneAim") > 0)
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 9:
                if (Input.GetButtonDown("PlayerOneChangeWeapon"))
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 10:
                if (Input.GetButtonDown("PlayerOneMelee"))
                {
                    runTextUpdateTimer = true;
                    UpdateTutorialText();
                }
                break;
            case 11:
                if (Input.GetButtonDown("PlayerOneReload"))
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 12:
                if (Input.GetButtonDown("PlayerOneChangeWeapon"))
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 13:
                //Runs the nextRoomDelay timer until it reaches zero
                nextRoomDelay -= Time.deltaTime;
                if (nextRoomDelay <= 0)
                {
                    //Starts the text update timer and sets the tutorialState to the next state, the ability tutorial
                    runTextUpdateTimer = true;
                    currentTutorialState = tutorialStates.abilityTutorial;
                }
                break;
        }
    }

    public void AbilityTutorial()
    {
        switch (textIndex)
        {
            //Checks the value of the textIndex and looks for the relevant input
            case 14:
                if (Input.GetButtonDown("PlayerOneLeftAbility"))
                {
                    runTextUpdateTimer = true;
                }
                break;
            case 15:
                if (Input.GetButtonDown("PlayerOneRightAbility"))
                {
                    runTextUpdateTimer = true;
                    //Iterates through the moveable walls in the secondSectionDoors array and triggers them to move, opening the museum area
                    for (int i = 0; i < secondSectionDoors.Length; i++)
                    {
                        secondSectionDoors[i].GetComponent<MovingObjectController>().isMoving = true;
                    }
                    currentTutorialState = tutorialStates.museumTutorial;
                }
                break;
        }
    }

    public void MuseumTutorial()
    {
        //If the player presses space, the tutorial text is disabled
        if (Input.GetButton("PlayerOneJump") && tutorialText.gameObject.activeSelf)
        {
            tutorialText.gameObject.SetActive(false);
        }
    }

}
