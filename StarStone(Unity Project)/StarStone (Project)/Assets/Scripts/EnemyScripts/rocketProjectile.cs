using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketProjectile : MonoBehaviour
{
   [Range(0,20)]
    public float rocketSpeed;

    [Range(0,5)]
    public float angularSpeed;

    public float rocketExplosionRadius;
    public float rocketExplosionDamage;

    public GameObject explosionParticles;
    public GameObject targetedPlayer;
    public AudioClip rocketExplosionSound;
    public AudioClip rocketFlyingSound;


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

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToPlayer, (angularSpeed) * Time.deltaTime);
        transform.Translate(Vector3.forward * rocketSpeed *Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach (var Object in Physics.OverlapSphere(gameObject.transform.position,rocketExplosionRadius))
        {
            if (Object.tag == "Player")
            {
                Object.GetComponent<PlayerBase>().TakeDamage(rocketExplosionDamage);
            }
        }
        try
        {
           GameObject _pSystem = Instantiate(explosionParticles,transform.position,Quaternion.identity);
            _pSystem.GetComponent<AudioSource>().PlayOneShot(rocketExplosionSound);
        }
        finally
        {
        Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, rocketExplosionRadius);
    }
}
