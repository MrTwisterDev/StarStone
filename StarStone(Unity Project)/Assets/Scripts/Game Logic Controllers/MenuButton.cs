using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: MenuButton                                       |
    // Script Author: James Smale                                    |
    // Purpose: Manages button functions related to GameObjects that |
    //          persist through scenes
    //***************************************************************|

    [Header("Logic Controllers")]
    public LevelManager levelManager;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("SceneManager").GetComponent<LevelManager>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void ToggleCoOp()
    {
        gameController.isCoOp = !gameController.isCoOp;
    }

    public void LoadNextLevel()
    {
        levelManager.LoadNextLevel();
    }

    public void LoadTutorial()
    {
        levelManager.LoadTutorial();
    }

    public void ChangeDifficulty(int i)
    {
        gameController.ChangeDifficulty(i);
    }

    public void LoadMainMenu()
    {
        levelManager.LoadMainMenu();
    }

}
