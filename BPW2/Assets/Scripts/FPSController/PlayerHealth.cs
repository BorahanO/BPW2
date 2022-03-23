using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image HealthBarFill;
    [SerializeField] 
    private GameObject HealthBar;

    private float playerHealth;
    private float playerMaxHealth = 50f;

    public DialogueRunner dialogueRunner;

    private void Start()
    {
        playerHealth = playerMaxHealth;
        //GetComponent<DialogueRunner>().AddFunction("damage", () => {return DamagePlayer();} );
    }

    public void Awake()
    {
        dialogueRunner.AddCommandHandler(
            "damage",
            (GameObject target) => {
                DamagePlayer();
            }             
        );
        dialogueRunner.AddCommandHandler(
            "camera_look",
            (GameObject target) => {
                Camera.main.transform.LookAt(target.transform);
            }             
        );
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