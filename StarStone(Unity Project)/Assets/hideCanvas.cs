using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideCanvas : MonoBehaviour
{
    public GameObject mainMenuCanvas;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    public void showCanvas()
    {
        mainMenuCanvas.SetActive(true);
    }
    public void hideCanvasEvent()
    {
        mainMenuCanvas.SetActive(false);

    }
}
