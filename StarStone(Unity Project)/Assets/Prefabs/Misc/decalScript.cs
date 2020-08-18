using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decalScript : MonoBehaviour
{
    public float decalDecayTimer;
    private float currentTimer;
    private Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
        currentTimer = decalDecayTimer;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a -= Time.deltaTime/currentTimer;
        gameObject.GetComponent<MeshRenderer>().material.color = color; 


        if (color.a <= 0)

        {
            Destroy(gameObject);
        }
    }
}
