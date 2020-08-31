using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStarStoneMaterial : MonoBehaviour
{
    public Material[] stoneMaterials;
    public MeshRenderer starstoneMesh;

    void Start()
    {
        starstoneMesh.material = stoneMaterials[Random.Range(0,stoneMaterials.Length)];
        gameObject.transform.GetChild(0).gameObject.GetComponent<Light>().color = starstoneMesh.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
