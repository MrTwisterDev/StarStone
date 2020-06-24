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

    public Text blinkTimerText;

    public Text waveTimerText;
    public Text waveNumberText;
    public Image timerBar;
    private float initialTime;

    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("playerCapsule").GetComponent<PlayerController>();
        activeWeaponController = playerController.activeWeapon.GetComponent<build_a_weapon>();

        totalAmmoText = GameObject.Find("TotalAmmoValue").GetComponent<Text>();
        currentMagazineAmmoText = GameObject.Find("CurrentMagazineAmmo").GetComponent<Text>();

        blinkTimerText = GameObject.Find("BlinkCooldownTimer").GetComponent<Text>();

        waveTimerText = GameObject.Find("WaveTimer").GetComponent<Text>();
        waveNumberText = GameObject.Find("WaveNumber").GetComponent<Text>();
        timerBar = GameObject.Find("TimerBar").GetComponent<Image>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        UpdateAmmoText();
    }

    public void GetChangedWeapon()
    {
        activeWeaponController = playerController.activeWeapon.GetComponent<build_a_weapon>();
    }

    public void UpdateBlinkTimer()
    {
        if (playerController.blinkCooldownTime == 5)
        {
            blinkTimerText.text = "Blink Ready";
        }
        else
        {
            blinkTimerText.text = playerController.blinkCooldownTimeRounded.ToString();
        }
    }

    public void UpdateAmmoText()
    {
        currentMagazineAmmoText.text = activeWeaponController.currentBullets.ToString() + "/" + activeWeaponController.magazineCapacity.ToString();
        totalAmmoText.text = activeWeaponController.totalBullets.ToString();
    }

    public void SetBaseTimerValue(float totalTime) { initialTime = totalTime; }

    public void UpdateWaveTimer(float timeRemaining)
    {
        int minutes = (int)timeRemaining / 60;
        int seconds = (int)timeRemaining % 60;
        waveTimerText.text = (minutes + ":" + seconds);
        timerBar.rectTransform.localScale = new Vector2(timerBar.rectTransform.localScale.x - Time.deltaTime / initialTime, timerBar.rectTransform.localScale.y);
    }

    public void UpdateWaveNumber(int waveNumber)
    {
        waveNumberText.text = "Wave " + waveNumber; 
    }

    public void UpdateHealthbar()
    {
        healthBar.value = playerController.currentHealth / 100;
    }

}
