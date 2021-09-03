using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Animator animator;

    public Transform firePoint;
    public Weapon currentWeapon;

    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask enemyLayers;

    public bool melee;
    public float attackRate;

    public int attackDamage = 1;

    float nextTimeToFire = 0f;

    [HideInInspector]
    public bool hasShot = false;
    [HideInInspector]
    public bool hasAttacked = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (melee == false)
            {
                if (Time.time >= nextTimeToFire)
                {
                    currentWeapon.Shoot(firePoint);
                    hasShot = true;
                    nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
                }
            }
            else
            {
                if (Time.time >= nextTimeToFire)
                {
                    MeleeAttack();
                    animator.SetTrigger("PlayerMeleeAttack01");
                    hasAttacked = true;
                    nextTimeToFire = Time.time + 1f / attackRate;
                }
            }
        }
        else
        {
            hasShot = false;
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
}
