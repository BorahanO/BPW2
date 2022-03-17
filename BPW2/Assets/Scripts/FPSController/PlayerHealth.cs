using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image HealthBarFill;
    [SerializeField] 
    private GameObject HealthBar;

    private float playerHealth;
    private float playerMaxHealth = 50f;

    private void Start()
    {
        playerHealth = playerMaxHealth;
    }

    public void DamagePlayer()
    {
        HealthBar.SetActive(true);
        playerHealth -= 10;
        HealthBarFill.fillAmount = playerHealth / playerMaxHealth;
        Invoke("SetUIOff", 3.0f);
    }

    void SetUIOff()
    {
        HealthBar.gameObject.SetActive(false);
    }

    void KillPlayer()
    {
        if (playerHealth == 0)
        {
            //Kill Player
        }
    }
}