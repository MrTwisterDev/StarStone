using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeProjectileBase : MonoBehaviour
{
    //***************************************************************|
    // Project Name: Temple Imperium                                 |
    // Script Name: PrototypeProjectileBase                          |
    // Script Author: James Smale                                    |
    // Purpose: A base class for the two different prototype weapon  |
    //          projectiles
    //***************************************************************|

    [HideInInspector]
    public Rigidbody rigidBody;
    [HideInInspector]
    public Transform cameraTransform;

    [Header("Audio")]
    [Tooltip("The sound that plays when the projectile detonates.")]
    public AudioClip detonationSound;

    [Header("Projectile Stats")]
    [Tooltip("The amount of force applied to the projectile on instantiation.")]
    public float launchForce;
    [Tooltip("The radius of the sphere of effect of the explosion.")]
    public float areaOfEffect;
    [Tooltip("The amount of damage enemies will take from the projectile's effects.")]
    public float damageToDeal;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        //Launches the projectile in the direction the player is facing with the appropriate amount of force
        rigidBody.AddForce(cameraTransform.forward * launchForce);
    }
}
