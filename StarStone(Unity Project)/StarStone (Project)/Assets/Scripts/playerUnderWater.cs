using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUnderWater : MonoBehaviour
{
    private RaycastHit waterCheck;
    public string waterLayer;
    private LayerMask waterLayerMask;

    public enum typeOfInteractingEntity
    {
        Player,
        Enemy,
        Object
    }

    public typeOfInteractingEntity typeOfEntity;

    // Start is called before the first frame update
    void Start()
    {
        waterLayerMask = LayerMask.GetMask(waterLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnderwater()) {
            switch (typeOfEntity)
            {
                case typeOfInteractingEntity.Player:
                    gameObject.GetComponent<PlayerController>().moveSpeed = gameObject.GetComponent<PlayerController>().underWaterSpeed;
              //      gameObject.GetComponent<PlayerController>().gravityScale = 
                    break;
                case typeOfInteractingEntity.Enemy:
                    //Do enemy case stuff here;
                    break;
                case typeOfInteractingEntity.Object:
                    //Do Object case stuff here;
                    break;

            }
            

        }
    }

    public bool isUnderwater()
    {
        if (Physics.Raycast(transform.position, Vector3.up, 50f, waterLayerMask))
        {
            Debug.DrawRay(transform.position, Vector3.up);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.up);
            return false;
        }
    }

    private void OnDrawGizmos()
    {
     //   Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z));
    }
}
