using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBoxes : scr_Collectable
{
    public int ammoAmount;
    // Start is called before the first frame update
    void Start()
    {
        collectableIncreaser = ammoAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void pickupCollectable(GameObject playerObject)
    {
        playerObject.GetComponent<PlayerController>().activeWeapon.GetComponent<baseWeaponClass>().totalBullets += collectableIncreaser;
        Destroy(gameObject);
    }

}
