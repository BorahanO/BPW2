using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image HealthBarFill;
    [SerializeField] 
    private GameObject HealthBar;
    [SerializeField] 
    private AudioSource DamageSound;

    private float playerHealth;
    private float playerMaxHealth = 50f;
    
    private void Start()
    {
        playerHealth = playerMaxHealth;
    }

    [YarnCommand("damage")]
    public void DamagePlayer()
    {
        HealthBar.SetActive(true);
        playerHealth -= 10;
        HealthBarFill.fillAmount = playerHealth / playerMaxHealth;
        Invoke("SetUIOff", 3.0f);
        DamageSound.Play();
        if (playerHealth == 0)
        {
            KillPlayer();
        }
    }
    
    [YarnCommand("damagemore")]
    public void DamagePlayerMore()
    {
        HealthBar.SetActive(true);
        playerHealth -= 20;
        HealthBarFill.fillAmount = playerHealth / playerMaxHealth;
        Invoke("SetUIOff", 3.0f);
        DamageSound.Play();
        if (playerHealth <= 0)
        {
            KillPlayer();
        }
    }
    
    [YarnCommand("instakill")]
    public void InstakillPlayer()
    {
        KillPlayer();
    }

    void SetUIOff()
    {
        HealthBar.gameObject.SetActive(false);
    }

    void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}