using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: DontDestroyOnLoad                                |
    // Script Author: James Smale                                    |
    // Purpose: Prevents objects from being destroyed when a new     |
    //          scene is loaded                                      |
    //***************************************************************|

    private static DontDestroyOnLoad dontDestroyScript;

    // Start is called before the first frame update
    void Start()
    {
        //Prevents the game object this script is attached to from being destroyed
        DontDestroyOnLoad(gameObject);

        //If there is already an instance of this script in the scene, it is destroyed and this one is retained
        if(dontDestroyScript == null)
        {
            dontDestroyScript = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
