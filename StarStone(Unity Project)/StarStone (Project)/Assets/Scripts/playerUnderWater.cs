using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUnderWater : MonoBehaviour
{
    private RaycastHit waterCheck;
    public string waterLayer;

    private LayerMask waterLayerMask;
    private PlayerController componentToModify;
    private bool ChangeMovementOnce;

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
        switch (typeOfEntity)
        {
            case typeOfInteractingEntity.Player:
                ChangeMovementOnce = true;
                componentToModify = gameObject.GetComponent<PlayerController>();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fog = isUnderwater();
        RenderSettings.fogDensity = 0.15f;
        RenderSettings.fogColor = Color.cyan;

        if (isUnderwater()) {
            switch (typeOfEntity)
            {
                case typeOfInteractingEntity.Player:
                    if (ChangeMovementOnce)
                    {
                        componentToModify.moveSpeedMultiplier += componentToModify.swimmingMultiplier;
                        ChangeMovementOnce = false;
                    }
                    
                    break;
                case typeOfInteractingEntity.Enemy:
                    //Do enemy case stuff here;
                    break;
                case typeOfInteractingEntity.Object:
                    //Do Object case stuff here;
                    break;

            }

        }
        else
        {
            switch (typeOfEntity)
            {
                case typeOfInteractingEntity.Player:
                    if (!ChangeMovementOnce)
                    {
                        componentToModify.moveSpeedMultiplier -= componentToModify.swimmingMultiplier;
                        ChangeMovementOnce = true;
                    }
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
