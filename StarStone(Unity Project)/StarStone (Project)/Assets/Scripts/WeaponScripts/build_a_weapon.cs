using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float roundsPerSecond; //How many rounds a second this gun will fire
    public float gunRecoil; //How much shooting knocks camera back

    private float timeTillBullet;
    private float currentTimeTillBullet;


    [HideInInspector]public bool spreadShotLock; //If true don't allow shooting until mouse0 is lifted

    public GameObject projectileFired; //What projectile should this weapon fire
    public int bulletsInSpread; //How many projectiles should be fired in a spreadShot

    UIController uiController;

    void Start()
    {
        timeTillBullet = 1/roundsPerSecond; //Calculates the fire rate
        currentTimeTillBullet = 0;//Makes the first show not have any fire time
        weaponAudioSource = gameObject.GetComponent<AudioSource>();

        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
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
                        weaponAudioSource.PlayOneShot(gunshotNoises[Random.Range(0, gunshotNoises.Length)]);
                        break;
                    case typesOfWeapon.spreadShot:
                        if (!spreadShotLock)
                        {
                            currentBullets--;
                            totalBullets--;
                            weaponAudioSource.PlayOneShot(gunshotNoises[Random.Range(0, gunshotNoises.Length)]);
                            for (int i = 0; i < bulletsInSpread; i++)
                            {

                                fireBullet();

                            }

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
        transform.parent.parent.gameObject.GetComponent<PlayerController>().xRotation -= gunRecoil;

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
}
