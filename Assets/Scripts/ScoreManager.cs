using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public Text scoreText;

    void Start()
    {
        score = 0; 
        UpdateScoreUI();
    }

    public static void AddPoints(int points)
    {
        score += points;

        var scoreManagers = FindObjectsOfType<ScoreManager>();
        foreach (var manager in scoreManagers)
        {
            manager.UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString(); 
        }
    }
}