using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class uiCutscene : MonoBehaviour
{
    public Image imageCanvas; //Canvas to draw cutSceneImages to
    public TextMeshProUGUI cutsceneTextObj; //Text to draw the cutscene text to

    public int stringIndex = 0;
    public int sceneIndex = 0;

    public UnityEvent onCutsceneFinish;


    [Serializable]
    public class cutScene_Scene //Custom class
    {
        public Image cutSceneImage;
        public string[] cutSceneText;
    }


    public cutScene_Scene[] scenes = new cutScene_Scene[0];


    // Start is called before the first frame update
    void Start()
    {
        imageCanvas = scenes[sceneIndex].cutSceneImage;
        cutsceneTextObj.text = scenes[sceneIndex].cutSceneText[stringIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(sceneIndex > scenes.Length)
            {
                onCutsceneFinish.Invoke();
                sceneIndex = 0;
                stringIndex = 0;
            }
            if (stringIndex + 1 < scenes[sceneIndex].cutSceneText.Length)
            {
                stringIndex++;
                loadScene(sceneIndex, stringIndex);
            }
            else
            {
                sceneIndex++;
                stringIndex = 0;
                loadScene(sceneIndex, stringIndex);
            }
        }
    }

    void loadScene(int sceneToLoad, int stringToLoad)
    {
        imageCanvas = scenes[sceneToLoad].cutSceneImage;
        cutsceneTextObj.text = scenes[sceneToLoad].cutSceneText[stringToLoad];
    }

    

}
