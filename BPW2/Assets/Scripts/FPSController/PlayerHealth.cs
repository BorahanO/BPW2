using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] 
    private int playerHealth;

    public void DamagePlayer()
    {
        playerHealth -= 10;
        Debug.Log(playerHealth);
    }

    void KillPlayer()
    {
        if (playerHealth == 0)
        {
            //Kill Player
        }
    }
}
