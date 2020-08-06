﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splitScreen : MonoBehaviour
{
    public bool isHorizontalSplit; //Is the screen split horizontally or vertically (Used in two player scenario's)
    private bool isHorizontalSplitOriginal; //Used to check against self so that the camera's can reset if the split is changed

    public Camera[] playerCharacters; //The cameras of each playable character

    [SerializeField] private float screenWidth;
    [SerializeField] private float screenHeight;


    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        isHorizontalSplitOriginal = isHorizontalSplit;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the screen resolution values if the resolution changes
        //Allows the multiple cameras to dynamically resize themselves depending on resolution
        if (screenHeight != Screen.height || screenWidth != Screen.width || isHorizontalSplit != isHorizontalSplitOriginal)
        {
            updateScreenResolution();
        }


    }

    public void updateScreenResolution()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        isHorizontalSplitOriginal = isHorizontalSplit;

        if (isHorizontalSplit)
        {
            for (int i = 0; i < playerCharacters.Length; i++)
            {
                playerCharacters[i].rect = new Rect((float)i / playerCharacters.Length, 0f, 1.0f / playerCharacters.Length, 1.0f);

            }
        }
        else
        {
            for (int i = 0; i < playerCharacters.Length; i++)
            {
                playerCharacters[i].rect = new Rect(0f, (float)i / playerCharacters.Length, 1.0f, 1.0f / playerCharacters.Length);

            }
        }
    }

}
