using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeWeapon : MonoBehaviour
{

    public float weaponCharge;

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
        weaponCharge = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            switch (currentWeaponMode)
            {
                case weaponModes.minigunMode:
                    //minigun code
                    break;
                case weaponModes.flamethrowerMode:
                    //flamethrower code
                    break;
                case weaponModes.vampireMode:
                    //vampire code
                    break;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(currentWeaponMode == weaponModes.singularityMode)
            {
                //singularity code
            }
        }
    }
}
