using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedStarStone : StarStoneBase
{
    void Start()
    {
        base.Start();
        StoneWeaponAffect = PrototypeWeapon.weaponModes.minigunMode;
        StoneEnemyAffect = enemyBase.stoneBuffs.speedBuff;
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
