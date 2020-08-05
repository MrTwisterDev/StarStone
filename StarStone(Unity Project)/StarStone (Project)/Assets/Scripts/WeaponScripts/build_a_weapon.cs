using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class build_a_weapon : baseWeaponClass
{
    public AudioClip[] gunshotNoises;
    public AudioClip[] reloadSound;

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
    private float originGunAccuracy; //The original accuracy of the gun, used so it can be reset.
    public float roundsPerSecond; //How many rounds a second this gun will fire
    public float gunCameraRecoil; //How much shooting knocks camera back
    public float gunAccuracyRecoil; //How much shooting affects the accuracy of the gun. Eventually resets base accuracy
    public float gunAccuracyRecovery; //How quickly the guns accuracy resets to 0

    public float gunAimSpeed; //How quick does the gun aim/ go between the two modes

    private float timeTillBullet;
    private float currentTimeTillBullet;


    public bool spreadShotLock; //If true don't allow shooting until mouse0 is lifted

    public GameObject projectileFired; //What projectile should this weapon fire
    public int bulletsInSpread; //How many projectiles should be fired in a spreadShot


    public Transform aimLocation;
    public Transform holsterLocation;

    public GameObject playerObject;

    public GameObject muzzleFlashChild;//The muzzle flash of the gun
    public GameObject crossHair;//The cross hair of the gun
    public GameObject InstancedCrossHair;//The cross hair of the gun
    public dynamicCrosshair crosshairScript;//The script of the guns crosshair.
    public float crosshairMultiplier; //How much to shrink or expand the crosshair

    UIController uiController;

    void Start()
    {
        timeTillBullet = 1/roundsPerSecond; //Calculates the fire rate
        currentTimeTillBullet = 0;//Makes the first show not have any fire time
        weaponAudioSource = gameObject.GetComponent<AudioSource>();
        uiController = gameObject.GetComponentInParent<UIController>();


        GameObject UICanvas = null;

        foreach (var canvas in uiController.gameObject.GetComponentsInChildren<Canvas>())
        {
            if(canvas.tag == "UICanvas")
            {
                UICanvas = canvas.gameObject;
            }
        }
        
        InstancedCrossHair = Instantiate(crossHair, UICanvas.transform,false);
        crosshairScript = InstancedCrossHair.GetComponentInChildren<dynamicCrosshair>();

        originGunAccuracy = gunAccuracy;

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
        canShoot = currentBullets > 0;
        gunAccuracy = Mathf.MoveTowards(gunAccuracy, originGunAccuracy, gunAccuracyRecovery);
        crosshairScript.masterOffset = (gunAccuracy * 2.5f + gunAccuracyRecoil/2);

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
                        totalBullets--;
                        fireBullet();
                        gunAccuracy += gunAccuracyRecoil;
                        weaponAudioSource.PlayOneShot(gunshotNoises[Random.Range(0, gunshotNoises.Length)]);
                        muzzleFlashChild.SetActive(true);
                        break;
                    case typesOfWeapon.spreadShot:
                        if (!spreadShotLock)
                        {
                            currentBullets--;
                            totalBullets--;
                            weaponAudioSource.PlayOneShot(gunshotNoises[Random.Range(0, gunshotNoises.Length)]);
                            muzzleFlashChild.SetActive(true);

                            for (int i = 0; i < bulletsInSpread; i++)
                            {

                                fireBullet();

                            }
                            gunAccuracy += gunAccuracyRecoil;
                            spreadShotLock = true;
                        }
                        break;
                }
                uiController.UpdateAmmoText();
            }
        }
    }

    void fireBullet()
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
            //Debug.Log(transform.parent.gameObject.transform.forward);

            Quaternion decalRot = Quaternion.LookRotation(shotTargetSpread.normal);
            Quaternion.Inverse(decalRot);
            GameObject bulletDecal = Instantiate(impactDecal, shotTargetSpread.point, Quaternion.Inverse(decalRot));
            GameObject _bulletHitParticle = Instantiate(bulletParticle, shotTargetSpread.point, Quaternion.identity);
            bulletDecal.transform.Translate(Vector3.back / 100);

            if(shotTargetSpread.collider.gameObject.GetComponent<enemyBase>() != null)
            {
                shotTargetSpread.collider.gameObject.GetComponent<enemyBase>().takeDamage(weaponDamage);
            }

        }

        //Recoil Application
        // transform.parent.parent.gameObject.transform.Rotate(new Vector3(gunRecoil, 0, 0));
        playerObject.GetComponent<PlayerBase>().xRotation -= gunCameraRecoil;

        Transform _weaponHoldPoint = gameObject.transform.Find("MuzzlePosition").GetComponent<Transform>();
        GameObject _particleSystem = Instantiate(muzzleFlash, _weaponHoldPoint, false);

    }

    public void reloadWeapon()
    {
        if (magazineCapacity == currentBullets)
        {
            return;
        }

        if (totalBullets >= magazineCapacity)
        {
            currentBullets = magazineCapacity;
        }
        else if (totalBullets < magazineCapacity)
        {
            currentBullets = totalBullets;
        }

        uiController.UpdateAmmoText();
    }

    public void playReloadSound() //Animation event for specific timing
    {
        weaponAudioSource.PlayOneShot(reloadSound[Random.Range(0, reloadSound.Length)]);
    }

    private void OnDisable()
    {
        if (InstancedCrossHair != null)
        {
            InstancedCrossHair.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (InstancedCrossHair != null)
        {
            InstancedCrossHair.SetActive(true);
        }
    }

    public void aimWeapon(bool AimingIn)
    {
        if (AimingIn)
        {
            Debug.Log("Aiming!");
            if (transform.parent.position != aimLocation.position)
            {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, aimLocation.position, gunAimSpeed * Time.deltaTime);
            }

       
            InstancedCrossHair.SetActive(false);
            //transform.position = Vector3.zero;
  
        }
        else
        {

            //         gameObject.transform.rotation = originPosition.rotation;
            if (transform.parent.position != holsterLocation.position)
            {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, holsterLocation.position, gunAimSpeed * Time.deltaTime);
            }


            InstancedCrossHair.SetActive(true);

        }
    }
}
