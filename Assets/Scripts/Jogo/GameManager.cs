using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;

    bool gameOver;

    void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}