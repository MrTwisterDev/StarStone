using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blkHoleStarStone : StarStoneBase
{
    void Start()
    {
        base.Start();
        StoneWeaponAffect = PrototypeWeapon.weaponModes.singularityMode;
        StoneEnemyAffect = enemyBase.stoneBuffs.blackHole;
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
