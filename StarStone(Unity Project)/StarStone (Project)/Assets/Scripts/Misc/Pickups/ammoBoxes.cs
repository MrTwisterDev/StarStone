using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBoxes : scr_Collectable
{
    public int ammoAmount;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collectableIncreaser = ammoAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void pickupCollectable(GameObject playerObject)
    {
        playerObject.GetComponent<PlayerBase>().activeWeapon.GetComponent<baseWeaponClass>().totalBullets += collectableIncreaser;
        AudioSource.PlayClipAtPoint(pickupSound, gameObject.transform.position);
        Destroy(gameObject);
    }

}
