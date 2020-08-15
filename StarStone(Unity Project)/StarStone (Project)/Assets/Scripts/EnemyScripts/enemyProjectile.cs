using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : MonoBehaviour
{
    public float projectileDamage;


    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBase>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }

    }
}
