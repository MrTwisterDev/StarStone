using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseWeaponClass : MonoBehaviour
{
    public string WeaponName;
    protected AudioSource weaponAudioSource;
    public float weaponDamage;
    public float damageFalloff;

    public int totalBullets; //The current amount of ammunition a player has in their inventory
    private int totalBulletsOriginal;
    public int magazineCapacity; //The maximum amount of uses a weapon has in a single clip
    private int magazineCapacityOriginal;
    public int currentBullets; //The current amount of uses a weapon has in it's current clip
    private int currentBulletsOriginal;

    public bool canShoot; //The weapon can be used, set to false whenever the player runs out of currentBullets
    public bool isShooting; //The weapon is currently being fired

    public bool canADS; //The gun does have the ability to ADS (Aim down sights)
    public float weaponSpeedMultiplier; //How much a gun slows the character down when equipped (0 has no effect)
    public GameObject impactDecal; //What decal is left on impact

    public GameObject muzzleFlash;
    public GameObject bulletParticle;
    public Vector3 muzzleFlashOffset;

    private void Start()
    {
        currentBullets = magazineCapacity;

        currentBulletsOriginal = currentBullets;
        magazineCapacityOriginal = magazineCapacity;
        totalBulletsOriginal = totalBullets;
    }

    void Update()
    {
            canShoot = currentBullets > 0;
    }

    // Update is called once per frame
    public virtual void useWeapon()
    {
       /* currentBullets--;
        totalBullets--;
        if(totalBullets <= magazineCapacity)
        {
            magazineCapacity = currentBullets;
        }*/
    }


}
