using UnityEngine;
using System;

public class DiceController : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private DiceRoller roller;

    public void Roll(Action<int> onComplete)
    {
        int result = dice.Roll();
        roller.PlayRoll(result, () => onComplete?.Invoke(result));
    }
}