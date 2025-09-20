using System;

namespace DiceGame.Core
{
    public interface IGameService
    {
        event Action<int> OnDiceRolled;
        void RollDice(Action<int> onComplete);
        bool IsRolling { get; }
    }
}
