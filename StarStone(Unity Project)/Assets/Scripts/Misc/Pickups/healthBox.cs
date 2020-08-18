using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBox : scr_Collectable
{

    public float x;
    public float y;
    public float z;

    public int healthAmount; //Amount to recover the players health
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collectableIncreaser = healthAmount;
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
        playerObject.GetComponent<PlayerBase>().RestoreHealth(healthAmount);
        AudioSource.PlayClipAtPoint(pickupSound, gameObject.transform.position);
        Destroy(gameObject);
    }

    public void destoryMe()
    {
        Destroy(gameObject);
    }

}
