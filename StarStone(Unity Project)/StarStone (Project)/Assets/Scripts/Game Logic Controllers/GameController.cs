using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public enum gameDifficulty
    {
        easyDifficulty,
        normalDifficulty,
        hardDifficulty

    };

    gameDifficulty currentGameDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDifficulty(int difficulty)
    {
        currentGameDifficulty = (gameDifficulty)difficulty;
        Debug.Log(currentGameDifficulty);
    }

}
