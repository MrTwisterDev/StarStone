using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class sensitivitySlider : MonoBehaviour
{
    public float aimSensitivity;
    public Slider gameObjectSlider;
    public TextMeshProUGUI sensitivityDisplay;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectSlider = gameObject.GetComponent<Slider>();
        sensitivityDisplay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        sensitivityDisplay.SetText(gameObjectSlider.value.ToString("F2"));

        if(gameObjectSlider.value == gameObjectSlider.maxValue)
        {
            sensitivityDisplay.SetText(gameObjectSlider.value.ToString("F2") + " Pro gamer mode");

        }
    }
}
