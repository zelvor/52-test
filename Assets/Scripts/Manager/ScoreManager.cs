using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score;

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"{score}";
    }
}