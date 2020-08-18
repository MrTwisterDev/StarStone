using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starStoneControllerRework : MonoBehaviour
{
    public GameObject protoTypeWeapon;

    public GameObject[] enemyList;
    public GameObject[] starStones = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        foreach (var _starStone in starStones)
        {
            _starStone.GetComponent<StarStoneBase>().starStoneController = gameObject;
            _starStone.GetComponent<StarStoneBase>().weaponsToEffect[0] = protoTypeWeapon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeElement(enemyBase.stoneBuffs enemyBuff)
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemyList)
        {
            enemy.GetComponent<enemyBase>().changePowerup(enemyBuff);
        }
    }

    public void newHighestStone()
    {
        //Finding the highest charged starstone
        float highestCharge = 0;
        GameObject chargeStarstone = new GameObject();
        foreach (var _starStone in starStones)
        {
            if(_starStone.GetComponent<StarStoneBase>().starstoneCharge >= highestCharge)
            {
                highestCharge = _starStone.GetComponent<StarStoneBase>().starstoneCharge;
                chargeStarstone = _starStone;
            }
        }

        chargeStarstone.GetComponent<StarStoneBase>().ActivateStarStone();

    }
}
