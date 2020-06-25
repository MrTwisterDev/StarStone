using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeWeapon : MonoBehaviour
{

    public float weaponCharge;

    public float minigunRange;
    public float minigunChargeUsage;
    public float minigunDamage;

    public float vampireRange;
    public float vampireChargeUsage;
    public float vampireDamage;
    public float vampireDrain;

    private PlayerController playerController;

    public LayerMask enemyLayer;

    public enum weaponModes
    {
        minigunMode,
        flamethrowerMode,
        vampireMode,
        singularityMode
    }

    public weaponModes currentWeaponMode;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();

        weaponCharge = 100;
    }

    public void FireMinigunMode()
    {
        if (weaponCharge > 0)
        {
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * minigunRange, Color.blue, 1);
            if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out rayHit, minigunRange, enemyLayer))
            {
                Debug.Log("Hit an enemy!");
                rayHit.collider.gameObject.GetComponent<enemyBase>().takeDamage(1.5f);
            }
            weaponCharge -= minigunChargeUsage;
        }
    }

    public void FireVampireMode()
    {
        if (weaponCharge > 0)
        {
            RaycastHit rayHit;
            Debug.DrawRay(transform.position, transform.forward * vampireRange, Color.green, 1);
            if (Physics.Raycast(transform.position, transform.forward, out rayHit, vampireRange, enemyLayer))
            {
                rayHit.collider.gameObject.GetComponent<enemyBase>().takeDamage(vampireDamage);
                playerController.currentHealth += vampireDrain;
            }
            weaponCharge -= vampireChargeUsage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            switch (currentWeaponMode)
            {
                case weaponModes.minigunMode:
                    FireMinigunMode();
                    break;
                case weaponModes.flamethrowerMode:
                    //flamethrower code
                    break;
                case weaponModes.vampireMode:
                    FireVampireMode();
                    break;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(currentWeaponMode == weaponModes.singularityMode)
            {
                //singularity code
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponMode = weaponModes.minigunMode;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponMode = weaponModes.flamethrowerMode;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeaponMode = weaponModes.vampireMode;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeaponMode = weaponModes.singularityMode;
        }
    }
}
