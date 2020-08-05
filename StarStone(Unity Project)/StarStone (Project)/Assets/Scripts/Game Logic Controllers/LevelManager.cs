using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: Level Manager                                    |
    // Script Author: James Smale                                    |
    // Purpose: Handle scene management and level loading            |
    //***************************************************************|

    public void LoadTutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadNextLevel()
    {
        int levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(levelToLoad);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
