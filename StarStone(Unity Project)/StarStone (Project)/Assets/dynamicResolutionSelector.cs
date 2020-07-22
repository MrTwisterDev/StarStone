using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class dynamicResolutionSelector : MonoBehaviour
{
    public bool fullscreenToggle;

    private void Start()
    {
        fullscreenToggle = Screen.fullScreen;
    }

    public void changeResolution()
    {
        string screenResolutionString = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log(screenResolutionString + " resolution has been selected!");
        char[] resolutionSplit = new char[] { 'x' };
        string[] screenResolutionSplit = screenResolutionString.Split('x');

        int screenWidth = int.Parse(screenResolutionSplit[0]);
        int screenHeight = int.Parse(screenResolutionSplit[1]);


        Screen.SetResolution(screenWidth, screenHeight, fullscreenToggle);

    }

    public void toggleBool()
    {
        fullscreenToggle = !fullscreenToggle;
        Screen.SetResolution(Screen.width, Screen.height, fullscreenToggle);

    }
}
