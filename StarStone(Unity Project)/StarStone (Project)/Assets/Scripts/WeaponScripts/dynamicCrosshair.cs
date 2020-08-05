using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicCrosshair : MonoBehaviour
{
    
    private GameObject[] crossHairParts = new GameObject[4];
    private Vector3[] crossHairOrigins = new Vector3[4];
    public float crossHairChangeSpeed;
    [Header("CrosshairOffsets")]
    [Tooltip("Affects all of the crosshair")]
    public float masterOffset;
    private float masterOffsetOrigin;
    [Tooltip("Affects the horizontal parts of the crosshair")]
    public float horizontalOffset;
    [Tooltip("Affects the vertical parts of the crosshair")]
    public float verticalOffset;

    public GameObject crosshairCentre;

    // Start is called before the first frame update
    void Start()
    {
        masterOffsetOrigin = masterOffset;
        for (int i = 0; i < crossHairParts.Length; i++)
        {
            crossHairParts[i] = transform.GetChild(i).gameObject;
            crossHairOrigins[i] = crossHairParts[i].transform.localPosition;
            Debug.Log("Got " + crossHairParts[i].name + " and assigned it to crossHairParts!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(masterOffsetOrigin != masterOffset)
        {
            masterOffsetOrigin = masterOffset;
        }

        for (int i = 0; i < crossHairParts.Length; i++)
        {
            switch (i)
            {
                case 0: verticalChange(verticalOffset, i);
                    break;
                case 1: verticalChange(-verticalOffset, i);
                    break;
                case 2: horizontalChange(horizontalOffset, i);
                    break;
                case 3: horizontalChange(-horizontalOffset, i);
                    break;
            }
        }
    }

    void horizontalChange(float offSetX, int I)
    {
        float _AddedMaster;

        if (I == 2)
        {
            _AddedMaster = masterOffset;
        }
        else
        {
            _AddedMaster = -masterOffset;

        }
        crossHairParts[I].GetComponent<RectTransform>().anchoredPosition =
        Vector3.Slerp(crossHairParts[I].transform.position, (Vector2)crossHairOrigins[I] + new Vector2(offSetX + _AddedMaster,0),crossHairChangeSpeed);
    }

    void verticalChange(float offSetY, int I)
    {
        float _AddedMaster;
        if(I == 0)
        {
            _AddedMaster = masterOffset;
        }
        else
        {
            _AddedMaster = -masterOffset;

        }


        crossHairParts[I].GetComponent<RectTransform>().anchoredPosition =
        Vector3.Slerp(crossHairParts[I].transform.position, (Vector2)crossHairOrigins[I] + new Vector2(0, offSetY + _AddedMaster), crossHairChangeSpeed);

    }


}
