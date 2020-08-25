using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBoxes : scr_Collectable
{
    public int ammoAmount;
    public float x;
    public float y;
    public float z;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collectableIncreaser = ammoAmount;
        Invoke("destoryMe", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationSpeed = new Vector3(x, y, z);
        transform.Rotate(rotationSpeed);

    }

    public override void pickupCollectable(GameObject playerObject)
    {
        PlayerBase player = playerObject.GetComponent<PlayerBase>();
        for(int i = 0; i < player.weaponsArray.Length; i++)
        {
            if(player.weaponsArray[i].tag != "Prototype")
            {
            player.weaponsArray[i].GetComponent<baseWeaponClass>().totalBullets += ammoAmount;
            }
        }
        AudioSource.PlayClipAtPoint(pickupSound, gameObject.transform.position);
        Destroy(gameObject);
    }

    public void destoryMe()
    {
        Destroy(gameObject);
    }

}
