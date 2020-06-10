using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUnderWater : MonoBehaviour
{
    private RaycastHit waterCheck;
    public string waterLayer;
    private LayerMask waterLayerMask;


    // Start is called before the first frame update
    void Start()
    {
        waterLayerMask = LayerMask.GetMask(waterLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnderwater())
        {
            Debug.Log("Yes underwater");
        }
        else
        {
            Debug.Log("Not underwater");
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
