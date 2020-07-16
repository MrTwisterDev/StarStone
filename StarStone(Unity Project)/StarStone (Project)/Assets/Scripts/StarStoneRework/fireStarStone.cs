using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireStarStone : StarStoneBase
{
    void Start()
    {
        base.Start();
        StoneWeaponAffect = PrototypeWeapon.weaponModes.flamethrowerMode;
        StoneEnemyAffect = enemyBase.stoneBuffs.fireBuff;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ActivateStarStone()
    {
        base.ActivateStarStone();

    }
}
