using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private PlayerController playerController;
    private build_a_weapon activeWeaponController;

    public Text totalAmmoText;
    public Text currentMagazineAmmoText;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();
        activeWeaponController = playerController.activeWeapon.GetComponent<build_a_weapon>();
        totalAmmoText = GameObject.Find("TotalAmmoValue").GetComponent<Text>();
        currentMagazineAmmoText = GameObject.Find("CurrentMagazineAmmo").GetComponent<Text>();
        UpdateAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmoText()
    {
        currentMagazineAmmoText.text = activeWeaponController.currentBullets.ToString() + "/" + activeWeaponController.magazineCapacity.ToString();
        totalAmmoText.text = activeWeaponController.totalBullets.ToString();
    }

}
