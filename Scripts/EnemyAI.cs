using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public bool mustPatrol;
    Rigidbody2D rb;
    public float moveSpeed = 500f;
    public Transform groundCheck;
    public Transform wallCheck;

    Transform checkPlayerRadius;
    public float playerRadiusCheckValue = 3f;

    bool mustFlipGround;
    bool mustFlipWall;
    bool mustShootPlayer;
    
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public Weapon currentWeapon;
    public Transform firePoint;

    float nextTimeToFire = 0f;

    public int hitPlayerDamage = 1;

    Animator enemyAnimation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkPlayerRadius = this.gameObject.GetComponent<Transform>();

        if (enemyAnimation != null)
            enemyAnimation = GetComponent<Animator>();
        mustPatrol = true;
    }

    private void Update()
    {
        if (mustPatrol)
        {
            Patrol();
            if (enemyAnimation != null)
                enemyAnimation.SetBool("isMoving", true);
        }
        else
        {
            if (enemyAnimation != null)
                enemyAnimation.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustFlipGround = !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            mustFlipWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, groundLayer);
        }

        mustShootPlayer = Physics2D.OverlapCircle(checkPlayerRadius.position, playerRadiusCheckValue, playerLayer);
        if (mustShootPlayer)
            if (Time.time >= nextTimeToFire && firePoint != null)
            {
                currentWeapon.Shoot(firePoint);
                nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
            }
    }

    void Patrol()
    {
        if (mustFlipGround || mustFlipWall)
        {
            Flip();
        }
        rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.Rotate(0f, 180f, 0f);
        moveSpeed *= -1f;
        mustPatrol = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (enemyAnimation != null)
                enemyAnimation.SetTrigger("isHurt");
            GetComponent<Enemy>().TakeDamage(hitPlayerDamage);
        }
        else if (collision.collider.CompareTag("Bullet"))
            if (enemyAnimation != null)
                enemyAnimation.SetTrigger("isHurt");
    }
}
