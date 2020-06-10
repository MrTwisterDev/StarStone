using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseWeaponClass : MonoBehaviour
{
    public string WeaponName;

    public int maxBullets; //The maximum amount of uses a weapon has in a single clip
    public int currentBullets; //The current amount of uses a weapon has in it's current clip

    public bool canShoot; //The weapon can be used, set to false whenever the player runs out of currentBullets

    public bool canADS; //The gun does have the ability to ADS (Aim down sights)
    public float weaponSpeedMultiplier; //How much a gun slows the character down when equipped (0 has no effect)



    void Update()
    {
        if(currentBullets <= 0)
        {
            canShoot = false;
        }
    }

    // Update is called once per frame
    public virtual void useWeapon()
    {
        currentBullets--;
    }
}
