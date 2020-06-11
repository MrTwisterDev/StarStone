using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build_a_weapon : baseWeaponClass
{

    public enum typesOfWeapon
    {
        singleShot, //Rifles, Pistols, SMG's, Assault Rifles, Miniguns
        spreadShot, //Shotguns, Blunderbusses etc
        singleShotProject, //Rocket Launchers, Projectile Launchers, Grenade Launchers etc
        spreadShotProject // Shotgun things
    }
    public typesOfWeapon typeOfWeapon;

    public float gunAccuracy; //How accurate is this weapon, 0 Being perfect dead on the crosshair, anything more will randomly veer away from the center
    public float roundsPerSecond; //How many rounds a second this gun will fire

    private float timeTillBullet;
    private float currentTimeTillBullet;

    public GameObject projectileFired; //What projectile should this weapon fire
    public int bulletsInSpread; //How many projectiles should be fired in a spreadShot


    void Start()
    {
        timeTillBullet = 1/roundsPerSecond; //Calculates the fire rate
        currentTimeTillBullet = 0;//Makes the first show not have any fire time


        switch (typeOfWeapon)
        {
            case typesOfWeapon.singleShot:

                break;
            case typesOfWeapon.singleShotProject:

                break;
            case typesOfWeapon.spreadShot:

                break;
            case typesOfWeapon.spreadShotProject:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (typeOfWeapon)
        {
            case typesOfWeapon.singleShot:

                break;
        }
    }

    public override void useWeapon()
    {
        currentTimeTillBullet -= Time.deltaTime;
        if(currentTimeTillBullet <= 0) //Bullet Fired
        {
            currentTimeTillBullet = timeTillBullet; //Resets the time before the next bullet

            switch (typeOfWeapon)
            {
                case typesOfWeapon.singleShot:
                    currentBullets--;
                    RaycastHit shotTarget;
                    if(Physics.Raycast(transform.position, Vector3.forward,out shotTarget))
                    {
                        Debug.Log(shotTarget.collider.gameObject.name);
                    }
                    break;
            }
        }
    }
}
