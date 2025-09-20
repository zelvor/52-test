using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score;

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = $"{score}";
    }
    
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"{score}";
    }
}