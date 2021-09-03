using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float bulletForce = 60f;
    public int damage = 50;

    public bool enemyBullet = false;
    public bool bossBullet = false;
    // public GameObject hitEffect;

    Rigidbody2D rb;
    Player player;

    private void Start()
    {
        // hitEffect.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();

        if (!bossBullet)
        {
            rb.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
        }
        else
        {
            Vector2 bossShootDirection = (player.transform.position - transform.position).normalized;
            rb.AddForce(bossShootDirection * bulletForce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   

        if (!enemyBullet)
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Damage(enemy);
            }
        }
        else
        {
            Player player = collision.collider.GetComponent<Player>();
            Animator playerAnimator = collision.collider.GetComponent<Animator>();

            if (player != null)
            {
                Damage(player);
                if (SceneManager.GetActiveScene().name == "Level01")
                    playerAnimator.SetTrigger("PlayerHurt");
                else if (SceneManager.GetActiveScene().name == "Level02")
                    playerAnimator.SetTrigger("PlayerHurt02");
            }
        }
        Remove();
    }
    void Damage(Entity target)
    {
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        Remove();
    }
    void Remove()
    {
        // hitEffect.SetActive(true);
        // GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        // Destroy(effect, 5f);
        Destroy(gameObject);
    }
}
