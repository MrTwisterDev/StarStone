using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Collectable : MonoBehaviour
{
    public int collectableIncreaser; //How much to increase a value based on what collectable it is;
    public AudioClip pickupSound;

    // Start is called before the first frame update
    protected virtual void Start()
    {


    }

    public virtual void pickupCollectable(GameObject playerObject)
    {
        Destroy(gameObject);
    }



    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            pickupCollectable(collision.gameObject);
        }
    }
}
