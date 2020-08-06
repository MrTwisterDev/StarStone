using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixelCrunch : MonoBehaviour
{
    public RenderTexture renderTexture;
    void Start()
    {
        int realRatio = Mathf.RoundToInt(Screen.width / Screen.height);
        renderTexture.width = NearestSuperiorPowerOf2(Mathf.RoundToInt(renderTexture.width * realRatio));
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.depth = 20;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);

    }

    int NearestSuperiorPowerOf2(int n)
    {
        return (int)Mathf.Pow(2, Mathf.Ceil(Mathf.Log(n) / Mathf.Log(2)));
    }
}
