using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
