using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthStarStone : StarStoneBase
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StoneWeaponAffect = PrototypeWeapon.weaponModes.vampireMode;
        StoneEnemyAffect = enemyBase.stoneBuffs.healthBuff;

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
