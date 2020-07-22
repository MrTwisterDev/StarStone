using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenBase : MonoBehaviour
{
    public menuSwitcher menuController;
    public GameObject screenToChange;

    public void changeMenu()
    {
        menuController.switchScreen(screenToChange);
    }


}
