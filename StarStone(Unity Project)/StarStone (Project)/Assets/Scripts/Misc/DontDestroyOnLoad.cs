using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    private static DontDestroyOnLoad dontDestroyScript;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

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
