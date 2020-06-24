using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarstoneController : MonoBehaviour
{

    private GameController gameController;
    private bool isActiveStarstone;

    public enum starstoneTypes
    {
        speedStarstone,
        fireStarstone,
        healthStarstone,
        buffStarstone
    }

    public starstoneTypes starstoneType;

    public void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void ActivateEffect()
    {
        isActiveStarstone = true;
        gameController.BuffEnemies((int)starstoneType);
    }

}
