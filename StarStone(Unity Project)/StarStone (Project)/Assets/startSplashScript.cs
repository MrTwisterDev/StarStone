using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class startSplashScript : UIScreenBase
{
    public KeyCode keyToStart;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Press " + keyToStart.ToString() + " to start.";
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToStart))
        {
            menuController.switchScreen(screenToChange);
        }
    }

}
