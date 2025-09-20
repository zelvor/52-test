using System;
using UnityEngine;

namespace DiceGame.Core
{
    public class DiceRollService : IDiceRoller
    {
        private readonly System.Random _random;
        
        public int MinValue { get; } = 1;
        public int MaxValue { get; } = 6;
        
        public DiceRollService(int? seed = null)
        {
            _random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        }
        
        public int Roll()
        {
            return _random.Next(MinValue, MaxValue + 1);
        }
        
        public bool IsValidResult(int result)
        {
            return result >= MinValue && result <= MaxValue;
        }
    }
}
