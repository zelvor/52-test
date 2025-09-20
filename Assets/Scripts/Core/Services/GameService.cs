using System;
using UnityEngine;

namespace DiceGame.Core
{
    public class GameService : IGameService
    {
        private readonly IDiceRoller _diceRoller;
        private readonly IDiceAnimation _diceAnimation;
        private readonly IScoreService _scoreService;
        
        public event Action<int> OnDiceRolled;
        public bool IsRolling => _diceAnimation.IsAnimating;
        
        public GameService(IDiceRoller diceRoller, IDiceAnimation diceAnimation, IScoreService scoreService)
        {
            _diceRoller = diceRoller ?? throw new ArgumentNullException(nameof(diceRoller));
            _diceAnimation = diceAnimation ?? throw new ArgumentNullException(nameof(diceAnimation));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
        }
        
        public void RollDice(Action<int> onComplete)
        {
            if (IsRolling)
            {
                Debug.LogWarning("Dice roll already in progress");
                return;
            }
            
            int result = _diceRoller.Roll();
            
            _diceAnimation.PlayRollAnimation(result, () =>
            {
                int pointsToAward = _scoreService.GetScoreForDiceResult(result);
                if (pointsToAward > 0)
                {
                    _scoreService.AddScore(pointsToAward);
                }
                
                OnDiceRolled?.Invoke(result);
                onComplete?.Invoke(result);
            });
        }
    }
}
