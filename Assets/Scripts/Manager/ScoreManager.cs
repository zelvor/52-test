using UnityEngine;
using TMPro;
using DiceGame.Core;
using DiceGame.Infrastructure;

namespace DiceGame.Presentation
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text scoreText;
        
        [Header("Service Reference")]
        [SerializeField] private UnityScoreService scoreService;
        
        private void Start()
        {
            if (scoreService == null)
            {
                Debug.LogError("ScoreService is not assigned!");
                return;
            }
            
            scoreService.OnScoreChanged += UpdateScoreDisplay;
            UpdateScoreDisplay(scoreService.CurrentScore);
        }
        
        private void UpdateScoreDisplay(int newScore)
        {
            if (scoreText != null)
            {
                scoreText.text = newScore.ToString();
            }
        }
        
        public void SetScore(int score)
        {
            if (scoreService != null)
            {
                scoreService.SetScore(score);
            }
        }
        
        public void AddScore(int value)
        {
            if (scoreService != null)
            {
                scoreService.AddScore(value);
            }
        }
        
        private void OnDestroy()
        {
            if (scoreService != null)
            {
                scoreService.OnScoreChanged -= UpdateScoreDisplay;
            }
        }
    }
}