using System;

namespace DiceGame.Core
{
    public interface IScoreService
    {
        int CurrentScore { get; }
        event Action<int> OnScoreChanged;
        void AddScore(int points);
        void SetScore(int score);
        void ResetScore();
        int GetScoreForDiceResult(int result);
    }
}
