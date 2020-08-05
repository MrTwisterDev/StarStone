using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class uiCutscene : MonoBehaviour
{    
    [Header ("Cutscene Creator")]
    public cutScene_Scene[] scenes = new cutScene_Scene[0];

    [Header ("What scene and text is the cutscene currently displaying")]
    public int stringIndex = 0;
    public int sceneIndex = 0;

    [Header ("Cutscene gameobjects")]
    public Image imageCanvas; //Canvas to draw cutSceneImages to
    public TextMeshProUGUI cutsceneTextObj; //Text to draw the cutscene text to

    [Header ("Cutscene events")]
    public UnityEvent onCutsceneFinish;


    [Serializable]
    public class cutScene_Scene //Custom class
    {
        public Sprite cutSceneImage;
        public string[] cutSceneText;
    }




    // Start is called before the first frame update
    void Start()
    {
        imageCanvas.sprite = scenes[sceneIndex].cutSceneImage;
        cutsceneTextObj.text = scenes[sceneIndex].cutSceneText[stringIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (sceneIndex + 1 == scenes.Length && stringIndex + 1 == scenes[sceneIndex].cutSceneText.Length)
            {
                onCutsceneFinish.Invoke();
                Debug.Log("Cutscene end invoked!");
                sceneIndex = 0;
                stringIndex = 0;
            }
            else
            {
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
    }

    void loadScene(int sceneToLoad, int stringToLoad)
    {
        imageCanvas.sprite = scenes[sceneToLoad].cutSceneImage;
        cutsceneTextObj.text = scenes[sceneToLoad].cutSceneText[stringToLoad];
    }

    public void beginCutScene()
    {
        gameObject.SetActive(true);
        loadScene(0, 0);
    }
    

}
