using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzleFlash : MonoBehaviour
{
    public float timeTillDeactivate;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        currentTime = timeTillDeactivate;
        gameObject.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Random.Range(0, 90));
    }
}
