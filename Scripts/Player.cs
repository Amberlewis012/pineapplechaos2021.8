using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Entity
{
    public TMP_Text healthText;

    public override void Die()
    {
        healthText.text = "HEALTH: 0";
        GameManager.instance.PlayerDeath();
        base.Die();
    }
    private void Update()
    {
        int playerHealth = this.health;
        healthText.text = "HEALTH: " + playerHealth.ToString();
    }
}
