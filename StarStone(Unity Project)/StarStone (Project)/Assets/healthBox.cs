using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBox : scr_Collectable
{
    public int healthAmount; //Amount to recover the players health
    // Start is called before the first frame update
    void Start()
    {
        collectableIncreaser = healthAmount;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void pickupCollectable(GameObject playerObject)
    {
        playerObject.GetComponent<PlayerController>().currentHealth += healthAmount;
        Destroy(gameObject);
    }
}
