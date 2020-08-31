using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineScript : MonoBehaviour
{
    private bool minePrimed;//Is the mine primed and ready to explode
    public float mineExplosionDistance; //The distance a mine explosion affects

    public float mineDamage; //The amount of damage a mine explosion causes

    public float minePrimeTimer; //How long it takes for a mine to prime itself
    private float currentMineTimer; //The current timer until the mine is primed

    public Color unThrownColour; //What color is the mine if it is static
    public Color unarmedMineUIColour; //What color is the mine in the UI when it is unarmed
    public Color armedMineUIColour; //What color is the mine in the UI when it is armed
    public Light mineLight; //What material should be applied when the mine is armed
    [HideInInspector]
    public UIController uiController; //What UI should this script update

    [HideInInspector]
    public CharacterVariantOne playerScript; //What player uses these mines

    private Rigidbody rigidBody;
    private Transform mainCamera;
    private MeshRenderer meshRenderer;

    public AudioClip explosionSound; //Sound when a mine explodes
    public AudioClip primedClip; //Sound when a mine is primed

    public GameObject explosionEffect; //What particle effect should the mine have when it explodes
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        //James' Work\\
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        currentMineTimer = minePrimeTimer;
        uiController.UpdateMineCounter(playerScript.currentActiveMines, unarmedMineUIColour);
        //~~~~~~~~~~~~\\
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.AddForce(mainCamera.forward * 500f);
    }

    // Update is called once per frame
    void Update()
    {
        //Mine timer
        currentMineTimer -= Time.deltaTime;
        minePrimed = currentMineTimer <= 0;

        if (minePrimed)
        {
            //James' Work\\
            if(!mineLight.enabled)
            {
                mineLight.enabled = true;
                AudioSource.PlayClipAtPoint(primedClip, transform.position, 0.25f);
                uiController.UpdateMineCounter(playerScript.currentActiveMines, armedMineUIColour);
            }
            //~~~~~~~~~~~~\\
            foreach (Collider collider in Physics.OverlapSphere(transform.position, mineExplosionDistance))
            {
                if(collider.gameObject.GetComponent<enemyBase>() != null) //If an enemy is nearby then do, tests if an overlapped object has the enemy script
                {
                    detonateMine(collider);
                }
            }           
        }

    }

    void detonateMine(Collider enemyCollided) //Explode the mine
    {
        enemyCollided.gameObject.GetComponent<enemyBase>().takeDamage(mineDamage);
        GameObject _pSystem = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        _pSystem.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        Destroy(gameObject);

    }

    void ReturnMine()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //Update the ui to reflect the loss of an active mine
        uiController.UpdateMineCounter(playerScript.currentActiveMines, unThrownColour);
        playerScript.currentActiveMines--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mineExplosionDistance);
    }
}
