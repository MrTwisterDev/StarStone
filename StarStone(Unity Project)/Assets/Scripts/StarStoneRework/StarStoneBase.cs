using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarStoneBase : MonoBehaviour
{
    [Tooltip("Boolean that determines whether or not the selected Starstone is active.")]
    public bool isActiveStarstone;
    [Space]

    [Header("Starstone Charge Variables")]
    [Tooltip("The current charge level of the selected Starstone.")]
    public float starstoneCharge;
    [Tooltip("The multiplier used to calculate the discharge rate of the selected Starstone.")]
    public float dischargeMultiplier;
    [Tooltip("The multiplier used to calculate the recharge rate of the selected Starstone.")]
    public float rechargeMultiplier;

    public GameObject starStoneController;
    public GameObject[] weaponsToEffect = new GameObject[1];

    protected PrototypeWeapon.weaponModes StoneWeaponAffect;
    protected enemyBase.stoneBuffs StoneEnemyAffect;

    public void Start()
    {
        starstoneCharge = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveStarstone)
        {
            starstoneCharge -= Time.deltaTime * dischargeMultiplier;

            if (starstoneCharge <= 0)
            {
                isActiveStarstone = false;
                starstoneCharge = 0;
                //Then pick highestChargeStone
            }


        }
        else
        {
            if (starstoneCharge < 100)
            {
                starstoneCharge += Time.deltaTime * rechargeMultiplier;
            }

        }
    }

    public virtual void ActivateStarStone()
    {
        isActiveStarstone = true;
        foreach (GameObject weapon in weaponsToEffect)
        {
            if (weapon.GetComponent<PrototypeWeapon>() != null)
            {
                weapon.GetComponent<PrototypeWeapon>().currentWeaponMode = StoneWeaponAffect;
                starStoneController.GetComponent<starStoneControllerRework>().changeElement(StoneEnemyAffect);
            }
        }
    }

}
