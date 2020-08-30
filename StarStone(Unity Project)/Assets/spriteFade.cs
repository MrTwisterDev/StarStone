using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteFade : MonoBehaviour
{
    public float alphaSpeed;
    private Color fadingSprite;
    // Start is called before the first frame update
    void Start()
    {
        fadingSprite = gameObject.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        fadingSprite.a -= alphaSpeed * Time.deltaTime;
        gameObject.GetComponent<Image>().color = fadingSprite;
        if(fadingSprite.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
