using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketProjectile : MonoBehaviour
{
   [Range(0,20)]
    public float rocketSpeed;

    [Range(0,5)]
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

        Quaternion rotationToPlayer = Quaternion.LookRotation(targetedPlayer.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToPlayer, (angularSpeed*2) * Time.deltaTime);
        transform.Translate(Vector3.forward * rocketSpeed *Time.deltaTime);
    }
}
