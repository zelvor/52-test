using UnityEngine;
using TMPro;
using DiceGame.Core;

namespace DiceGame.Infrastructure
{
    public class UnityScoreService : MonoBehaviour, IScoreService
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text scoreText;
        
        [Header("Configuration")]
        [SerializeField] private int initialScore = 0;
        
        private ScoreService _coreScoreService;
        
        public int CurrentScore => _coreScoreService.CurrentScore;
        public event System.Action<int> OnScoreChanged
        {
            add => _coreScoreService.OnScoreChanged += value;
            remove => _coreScoreService.OnScoreChanged -= value;
        }
        
        private void Awake()
        {
            InitializeCoreService();
            SetupUI();
        }
        
        private void InitializeCoreService()
        {
            _coreScoreService = new ScoreService(initialScore);
            _coreScoreService.OnScoreChanged += UpdateUI;
        }
        
        private void SetupUI()
        {
            if (scoreText == null)
            {
                Debug.LogError("ScoreText is not assigned!");
                return;
            }
            
            UpdateUI(_coreScoreService.CurrentScore);
        }
        
        public void AddScore(int points)
        {
            _coreScoreService.AddScore(points);
        }
        
        public void SetScore(int score)
        {
            _coreScoreService.SetScore(score);
        }
        
        public void ResetScore()
        {
            _coreScoreService.ResetScore();
        }

        public int GetScoreForDiceResult(int result)
        {
            return _coreScoreService.GetScoreForDiceResult(result);
        }

        private void UpdateUI(int newScore)
        {
            if (scoreText != null)
            {
                scoreText.text = newScore.ToString();
            }
        }
        
        private void OnDestroy()
        {
            if (_coreScoreService != null)
            {
                _coreScoreService.OnScoreChanged -= UpdateUI;
            }
        }
    }
}
