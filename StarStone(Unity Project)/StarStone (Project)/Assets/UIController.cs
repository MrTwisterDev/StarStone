using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private PlayerController playerController;
    private build_a_weapon activeWeaponController;
    public Text currentMagazineAmmoText;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("PlayerCapsule").GetComponent<PlayerController>();
        activeWeaponController = playerController.activeWeapon.GetComponent<build_a_weapon>();
        currentMagazineAmmoText = GameObject.Find("CurrentMagazineAmmo").GetComponent<Text>();
        currentMagazineAmmoText.text = activeWeaponController.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
