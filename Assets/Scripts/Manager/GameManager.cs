using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button rollButton;
    [SerializeField] private DiceController diceController;
    [SerializeField] private ScoreManager scoreManager;

    private void Start()
    {
        rollButton.onClick.AddListener(OnRollClicked);
        scoreManager.SetScore(0);
    }

    private void OnRollClicked()
    {
        diceController.Roll(OnDiceResult);
    }

    private void OnDiceResult(int result)
    {
        Debug.Log($"Dice Result: {result}");
        if (result == 6)
            scoreManager.AddScore(1);
    }
}