using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuSwitcher : MonoBehaviour
{
    [Header("Screens, the first is set active")]
    public GameObject[] menuScreens;


    // Start is called before the first frame update
    void Start()
    {
        menuScreens[0].SetActive(true);
        foreach (var screen in menuScreens)
        {
            screen.SetActive(false);
        }
        menuScreens[0].SetActive(true);
    }

    // Update is called once per frame
    public void switchScreen(GameObject screenToSwitch)
    {
        foreach (var menu in menuScreens)
        {
            if(menu.name == screenToSwitch.name)
            {
                screenToSwitch.SetActive(true);
            }
            else
            {
                menu.SetActive(false);
            }
        }
    }
}
