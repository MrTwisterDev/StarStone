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

    private GameObject parentOfWeapon; //Used for optimisation purposes. Used if the weapon itself does not rotate, but it's parent does (Effecting trajectory)

    public float gunAccuracy; //How accurate is this weapon, 0 Being perfect dead on the crosshair, anything more will randomly veer away from the center
    public float roundsPerSecond; //How many rounds a second this gun will fire
    public float gunRecoil; //How much shooting knocks camera back

    private float timeTillBullet;
    private float currentTimeTillBullet;

    [HideInInspector]public bool spreadShotLock; //If true don't allow shooting until mouse0 is lifted

    public GameObject projectileFired; //What projectile should this weapon fire
    public int bulletsInSpread; //How many projectiles should be fired in a spreadShot


    void Start()
    {
        timeTillBullet = 1/roundsPerSecond; //Calculates the fire rate
        currentTimeTillBullet = 0;//Makes the first show not have any fire time


        switch (typeOfWeapon) //A case by case basis on how a weapon should be initialised
        {
            case typesOfWeapon.singleShot:

                break;
            case typesOfWeapon.singleShotProject:

                break;
            case typesOfWeapon.spreadShot:
                spreadShotLock = false;
                break;
            case typesOfWeapon.spreadShotProject:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        canShoot = currentBullets > 0 && totalBullets > 0;

        switch (typeOfWeapon) //How should each weapon differ per frame
        {
            case typesOfWeapon.singleShot:

                break;
        }
    }

    public override void useWeapon()
    {
        if (canShoot)
        {

            currentTimeTillBullet -= Time.deltaTime; //Counts down the timer until the next "bullet" is fired
            if (currentTimeTillBullet <= 0) //Bullet Fired
            {
                currentTimeTillBullet = timeTillBullet; //Resets the time before the next "bullet"

                switch (typeOfWeapon)
                {
                    case typesOfWeapon.singleShot:
                        currentBullets--;

                        //Accuracy Calculation (X and Y)
                        Vector3 _baseDirection = transform.parent.gameObject.transform.forward; //100% Accurate direction
                        float _degreeOfAccuracy = Random.Range(0, gunAccuracy);
                        float bulletAngle = Random.Range(0, 360f);

                        _baseDirection = Quaternion.AngleAxis(_degreeOfAccuracy, transform.parent.gameObject.transform.up) * _baseDirection;
                        _baseDirection = Quaternion.AngleAxis(bulletAngle, transform.parent.gameObject.transform.forward) * _baseDirection;

                     
                        //Firing a bullet at the previously calculated angle
                        RaycastHit shotTarget;
                        if (Physics.Raycast(transform.position, _baseDirection, out shotTarget))
                        {
                            Debug.DrawRay(transform.position, transform.parent.gameObject.transform.forward * 10, Color.red, 2);
                            Debug.DrawRay(transform.position, _baseDirection*10, Color.yellow, 2);
                          //  Debug.Log(shotTarget.collider.gameObject.name);
                            Debug.Log(transform.parent.gameObject.transform.forward);

                        }

                        //Recoil Application
                        // transform.parent.parent.gameObject.transform.Rotate(new Vector3(gunRecoil, 0, 0));
                        transform.parent.parent.gameObject.GetComponent<PlayerController>().xRotation -= gunRecoil;

                        break;
                    case typesOfWeapon.spreadShot:
                        if (!spreadShotLock)
                        {
                            currentBullets--;

                            for (int i = 0; i < bulletsInSpread; i++)
                            {


                                //Accuracy Calculation (X and Y)
                                Vector3 _baseDirectionSpread = transform.parent.gameObject.transform.forward; //100% Accurate direction
                                float _degreeOfAccuracySpread = Random.Range(0, gunAccuracy);
                                float bulletAngleSpread = Random.Range(0, 360f);

                                _baseDirectionSpread = Quaternion.AngleAxis(_degreeOfAccuracySpread, transform.parent.gameObject.transform.up) * _baseDirectionSpread;
                                _baseDirectionSpread = Quaternion.AngleAxis(bulletAngleSpread, transform.parent.gameObject.transform.forward) * _baseDirectionSpread;


                                //Firing a bullet at the previously calculated angle
                                RaycastHit shotTargetSpread;
                                if (Physics.Raycast(transform.position, _baseDirectionSpread, out shotTargetSpread))
                                {
                                    Debug.DrawRay(transform.position, transform.parent.gameObject.transform.forward * 20, Color.red, 1);
                                    Debug.DrawRay(transform.position, _baseDirectionSpread * 20, Color.yellow, 1);
                                    Debug.Log(transform.parent.gameObject.transform.forward);

                                    Quaternion decalRot = Quaternion.LookRotation(shotTargetSpread.normal);
                                    Quaternion.Inverse(decalRot);
                                    GameObject bulletDecal = Instantiate(impactDecal, shotTargetSpread.point, Quaternion.Inverse(decalRot));
                                    bulletDecal.transform.Translate(Vector3.back/100);

                                }
                            }

                            //Recoil Application
                            // transform.parent.parent.gameObject.transform.Rotate(new Vector3(gunRecoil, 0, 0));
                            transform.parent.parent.gameObject.GetComponent<PlayerController>().xRotation -= gunRecoil;
                            spreadShotLock = true;
                        }
                        break;
                }
            }
        }
    }
}
