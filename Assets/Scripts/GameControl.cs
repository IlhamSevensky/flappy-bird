using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;
    public GameObject gameOverText, titleStateText, playButton, exitButton, retryButton;
    public enum GameStatus {STARTED, PAUSED, PLAYED, ENDED};
    public GameStatus gameState = GameStatus.STARTED;
    public float scrollSpeed = -1.5f;
    public bool isGameOver = false;
    public Text scoreText;
    private Text titleState;
    private int score = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
           
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
          
    }

    void Start()
    {
        titleState = titleStateText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("state : " + gameState);

        if (gameState == GameStatus.PAUSED || gameState == GameStatus.STARTED)
        {
            ShowPausedMenu(gameState);
        }

        if (gameState == GameStatus.ENDED)
        {
            ShowGameOverMenu();
        }

        if (gameState == GameStatus.PLAYED && Input.GetKeyDown(KeyCode.Escape))
        {
            gameState = GameStatus.PAUSED;
            ShowPausedMenu(gameState);
        }

    }

    public void Score()
    {
        if (isGameOver)
        {  return;}
        score++;
        scoreText.text = "Score: " + score;
    }
    public void Die()
    {
        isGameOver = true;
        gameState = GameStatus.ENDED;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowPausedMenu(GameStatus status)
    {

        if (status == GameStatus.STARTED) { titleState.text = "Flappy Bird"; }
        if (status == GameStatus.PAUSED) { titleState.text = "Paused"; }

        Time.timeScale = 0f;
        gameOverText.SetActive(false);
        exitButton.SetActive(true);
        playButton.SetActive(true);
        retryButton.SetActive(false);
        titleStateText.SetActive(true);
    }

    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f;
        gameOverText.SetActive(true);
        exitButton.SetActive(true);
        playButton.SetActive(false);
        retryButton.SetActive(true);
        titleStateText.SetActive(false);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameState = GameStatus.PLAYED;
        gameOverText.SetActive(false);
        exitButton.SetActive(false);
        playButton.SetActive(false);
        retryButton.SetActive(false);
        titleStateText.SetActive(false);
    }
}
