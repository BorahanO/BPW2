using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

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

    public DialogueRunner dialogueRunner;

    private void Start()
    {
        playerHealth = playerMaxHealth;
        //GetComponent<DialogueRunner>().AddFunction("damage", () => {return DamagePlayer();} );
    }

    [YarnCommand("damage")]
    public void DamagePlayer()
        {
            HealthBar.SetActive(true);
            playerHealth -= 10;
            HealthBarFill.fillAmount = playerHealth / playerMaxHealth;
            Invoke("SetUIOff", 3.0f);
            DamageSound.Play();
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