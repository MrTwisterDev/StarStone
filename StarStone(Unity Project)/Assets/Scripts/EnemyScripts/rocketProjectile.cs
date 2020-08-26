using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketProjectile : MonoBehaviour
{
   [Range(0,20)]
    public float rocketSpeed; //What speed does the rocket fly at

    [Range(0,5)]
    public float angularSpeed; //How quickly does the rocket turn

    public float rocketExplosionRadius; //How large is the explosion radius of the rocket
    public float rocketExplosionDamage; //How much damage does the rocket deal

    public GameObject explosionParticles; //Particles of the explosion
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

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToPlayer, (angularSpeed) * Time.deltaTime);//Look towards the player
        transform.Translate(Vector3.forward * rocketSpeed *Time.deltaTime); //Fly towards the player
    }


    private void OnCollisionEnter(Collision collision)
    {

        AudioSource.PlayClipAtPoint(rocketExplosionSound, gameObject.transform.position, 0.1f); // Lewis' code 

        foreach (var Object in Physics.OverlapSphere(gameObject.transform.position,rocketExplosionRadius))
        {
            if (Object.tag == "Player")
            {
                Object.GetComponent<PlayerBase>().TakeDamage(rocketExplosionDamage); //If a player is in the explosion radius damage the player
            }
        }
        try
        {
           GameObject _pSystem = Instantiate(explosionParticles,transform.position,Quaternion.identity);
            _pSystem.GetComponent<AudioSource>().PlayOneShot(rocketExplosionSound,  0.1f);
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
