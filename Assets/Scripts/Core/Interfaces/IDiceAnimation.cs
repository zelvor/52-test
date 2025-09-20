using System;

namespace DiceGame.Core
{
    public interface IDiceAnimation
    {
        void PlayRollAnimation(int result, Action onComplete);
        bool IsAnimating { get; }
    }
}
