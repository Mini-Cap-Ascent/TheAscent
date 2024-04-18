using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public string playerTag = "Player";  // Use this tag to find the player object.

    private Transform playerTransform;
    private float highestYPosition = 0;
    private int highScore = 0;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
        InvokeRepeating("CheckForPlayer", 0.5f, 0.5f); // Periodically check for player
    }

    void CheckForPlayer()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                playerTransform = player.transform;
                highestYPosition = playerTransform.position.y;  // Initialize highestYPosition once player is found
                CancelInvoke("CheckForPlayer"); // Stop checking once the player is found
            }
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            UpdateCurrentScore();
        }
    }

    private void UpdateCurrentScore()
    {
        if (playerTransform.position.y > highestYPosition)
        {
            highestYPosition = playerTransform.position.y;
            currentScoreText.text = "Current Score: " + Mathf.FloorToInt(highestYPosition);
            CheckHighScore();
        }
    }

    private void CheckHighScore()
    {
        int currentScore = Mathf.FloorToInt(highestYPosition);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = "High Score: " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void ResetScore()
    {
        if (playerTransform != null)
        {
            highestYPosition = playerTransform.position.y;
            currentScoreText.text = "Current Score: 0";
        }
    }
}
