using UnityEngine;
using TMPro;

public class ScoreGameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Remplace Text par TextMeshProUGUI
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreUI();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Ton score: " + ScoreManager.score.ToString();
        }
    }
}
