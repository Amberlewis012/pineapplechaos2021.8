using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject winPanel;
    public GameObject losePanel;
    bool hasWon = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void PlayerDeath()
    {
        StartCoroutine(RestartGame());
        if (!hasWon)
        {
            losePanel.SetActive(true);
        }
    }
    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PlayerWin()
    {
        StartCoroutine(LoadNextLevel());
        winPanel.SetActive(true);
        hasWon = true;
    }
    IEnumerator LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level03")
        {
            yield return new WaitForSecondsRealtime(5f);
            SceneManager.LoadScene("Menu");
        }
        else
        {
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
