using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Image HealthBar;
    public float playerHealth = 50f;
    private float playerMaxHealth = 50f;
    

    private void Start()
    {
        HealthBar = GetComponent<Image>();
    }

    public void DamagePlayer()
    {
        playerHealth -= 10;
    }

    void KillPlayer()
    {
        if (playerHealth == 0)
        {
            //Kill Player
        }
    }

    private void Update()
    {
        HealthBar.fillAmount = playerHealth / playerMaxHealth;
    }
}