using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Music Manager                                    |
    // Script Author: James Smale                                    |
    // Purpose: Manages the music played throughout the game.        |
    //***************************************************************|

    //Audio Elements
    #region
    [Header("Music Tracks")]
    [Tooltip("An array containing all of the music tracks for the game. They should be placed in the array index corresponding to the level index they should pay on.")]
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    #endregion
    //Level Index Integers
    #region
    private int currentlyLoadedLevel;
    private int newlyLoadedLevel;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClips[0];
        audioSource.Play();
        currentlyLoadedLevel = SceneManager.GetActiveScene().buildIndex;
        newlyLoadedLevel = currentlyLoadedLevel;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to see if the level currently loaded in the game has changed. If it has, the value of CurrentlyLoadedLevel is updated
        newlyLoadedLevel = SceneManager.GetActiveScene().buildIndex;
        if(newlyLoadedLevel != currentlyLoadedLevel)
        {
            currentlyLoadedLevel = newlyLoadedLevel;
            //The audio clip of the MusicManager's audio source is updated to reflect the level the player is in
            audioSource.clip = audioClips[currentlyLoadedLevel];
            audioSource.Play();
        }
    }
}
