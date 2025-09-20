using System;

namespace DiceGame.Core
{
    public interface IDiceRoller
    {
        int Roll();
        int MinValue { get; }
        int MaxValue { get; }
    }
}
