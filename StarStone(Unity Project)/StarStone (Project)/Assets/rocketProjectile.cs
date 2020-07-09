using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketProjectile : MonoBehaviour
{
    public float rocketSpeed;
    public float angularSpeed;

    public GameObject targetedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startFromEnemy()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetedPlayer.transform);
        transform.Translate(transform.forward * rocketSpeed);
    }
}
