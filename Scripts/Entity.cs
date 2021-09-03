using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    // public GameObject deathEffect;
    public GameManager gameManager;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        // GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
        // Destroy(effect, 5f);

        if (this.gameObject.name == "BossEnemy01")
        {
            gameManager.PlayerWin();
        }

        Destroy(gameObject);
    }
}
