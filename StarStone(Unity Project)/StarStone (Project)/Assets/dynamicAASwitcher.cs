using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dynamicAASwitcher : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public void changeAAdynamically()
    {
        string antiAliasLevelString = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log(antiAliasLevelString + " level of AA selected");

        antiAliasLevelString = antiAliasLevelString.Replace("x", "");

        QualitySettings.antiAliasing = int.Parse(antiAliasLevelString);
    }
}
