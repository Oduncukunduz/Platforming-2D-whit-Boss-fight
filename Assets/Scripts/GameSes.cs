using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSes : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI youWinText;
    public PlayerMove playerMove;

    void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSes>().Length;
        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
        gameOverText.gameObject.SetActive(false);
        youWinText.gameObject.SetActive(false);
    }

    public void processPlayerDeath()
    {
        gameOverText.gameObject.SetActive(true);
        Vector2 kick = playerMove.deadkick;
        //Time.timeScale = 0f; // Oyunu dondur

    }
    public void BossDefeated()
    {
        youWinText.gameObject.SetActive(true);
        // Time.timeScale = 0f;
    }

    public void AddToScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
        scoreText.text = playerScore.ToString();
    }
}