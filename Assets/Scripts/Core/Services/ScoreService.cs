using System;

namespace DiceGame.Core
{
    public class ScoreService : IScoreService
    {
        private int _currentScore;
        
        public int CurrentScore => _currentScore;
        
        public event Action<int> OnScoreChanged;
        
        public ScoreService(int initialScore = 0)
        {
            _currentScore = initialScore;
        }
        
        public void AddScore(int points)
        {
            if (points == 0) return;
            
            _currentScore += points;
            OnScoreChanged?.Invoke(_currentScore);
        }
        
        public void SetScore(int score)
        {
            if (_currentScore == score) return;
            
            _currentScore = score;
            OnScoreChanged?.Invoke(_currentScore);
        }
        
        public void ResetScore()
        {
            SetScore(0);
        }
        
        public int GetScoreForDiceResult(int diceResult)
        {
            return diceResult == 6 ? 1 : 0;
        }
    }
}
