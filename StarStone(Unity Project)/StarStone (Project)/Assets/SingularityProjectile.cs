using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityProjectile : PrototypeProjectileBase
{
    public float fuseLength;
    private bool hasDetonated;

    // Start is called before the first frame update
    void Start()
    {
        if(fuseLength == 0) { fuseLength = 3f; }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDetonated)
        {
            fuseLength -= Time.deltaTime;
            if(fuseLength <= 0)
            {
                hasDetonated = true;
                rigidBody.isKinematic = false;
            }
        }
        else
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        
    }

}
