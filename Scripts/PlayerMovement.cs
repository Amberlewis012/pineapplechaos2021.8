using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    CharacterController2D controller;
    Animator animator;
    public GameManager gameManager;

    public TMP_Text coinText;
    public int eachCoinAmount = 1;
    public int totalCoins = 0;

    public float moveSpeed = 40f;
    public float runSpeed = 70f;
    float horizontalMove = 0f;
    bool isJumping = false;
    bool isRunning = false;

    public int initialCoinToHealthAmount = 5;
    int coinToHealthAmount;

    string currentScene;

    Player player;
    public int enemyHitDamage = 1;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Level01")
            animator.SetBool("isFirstLevel", true);
        else if (currentScene == "Level02" || currentScene == "Level03")
            animator.SetBool("isFirstLevel", false);
        coinToHealthAmount = initialCoinToHealthAmount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isRunning)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }
        if (!isRunning)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }
        else
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }

        if (currentScene == "Level01")
        {
            animator.SetFloat("MoveSpeed", Mathf.Abs(horizontalMove));
        }
        else if (currentScene == "Level02" || currentScene == "Level03")
        {
            animator.SetFloat("MoveSpeed02", Mathf.Abs(horizontalMove));
        }


        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            if (currentScene == "Level01")
            {
                animator.SetBool("isJumping", true);
            }
            else if (currentScene == "Level02" || currentScene == "Level03")
            {
                animator.SetBool("isJumping02", true);
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime, false, isJumping);
        isJumping = false;
    }

    public void OnLanding()
    {
        if (currentScene == "Level01")
            animator.SetBool("isJumping", false);
        else if (currentScene == "Level02" || currentScene == "Level03")
            animator.SetBool("isJumping02", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string playerCollider = collision.collider.tag;

        switch (playerCollider)
        {
            case "Enemy":
                if (currentScene == "Level01")
                    animator.SetTrigger("PlayerHurt");
                else if (currentScene == "Level02" || currentScene == "Level03")
                    animator.SetTrigger("PlayerHurt02");
                player.TakeDamage(enemyHitDamage);
                break;
            case "Lava":
                player.Die();
                break;
            case "Win":
                gameManager.PlayerWin();
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coinToHealthAmount--;
            Destroy(collision.gameObject);
            totalCoins += eachCoinAmount;
            coinText.text = "COINS: " + totalCoins.ToString();
            if (coinToHealthAmount <= 0)
            {
                player.health++;
                coinToHealthAmount = initialCoinToHealthAmount;
            }
        }
    }
}
