using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageController : MonoBehaviour
{
    public int health;
    public Action Hurt;
    public Action Death;

    public void takeDamage(int damage)
    {
        //Decrease current health by damage value
        health -= damage;
        if (this.CompareTag("Player")) {
            PlayerInteractHealth.currentHealth -= damage;
            health = PlayerInteractHealth.currentHealth;
            Player_Inputs.enabled = false;
        }
     
        if (health <= 0)
        {
            if (Death != null)
            {
                Death();
            }
        }
        else
        {
            if (Hurt != null)
            {
                Hurt();
            }
        }
    }
}
