using UnityEngine;
using UnityEngine.UI;
using DiceGame.Core;
using DiceGame.Infrastructure;

namespace DiceGame.Presentation
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button rollButton;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip diceRollSFX;

        [Header("Service Container")]
        [SerializeField] private DiceGameContainer gameContainer;

        private IGameService _gameService;

        private void Start()
        {
            InitializeServices();
            SetupUI();
        }

        private void InitializeServices()
        {
            if (gameContainer == null)
            {
                Debug.LogError("GameContainer is not assigned!");
                return;
            }

            _gameService = gameContainer.GameService;
            if (_gameService == null)
            {
                Debug.LogError("GameService is not available!");
                return;
            }

            _gameService.OnDiceRolled += OnDiceRolled;
        }

        private void SetupUI()
        {
            if (rollButton == null)
            {
                Debug.LogError("RollButton is not assigned!");
                return;
            }

            rollButton.onClick.AddListener(OnRollButtonClicked);
        }

        private void OnRollButtonClicked()
        {
            if (_gameService == null) return;

            if (audioSource != null && diceRollSFX != null)
            {
                audioSource.PlayOneShot(diceRollSFX);
            }

            _gameService.RollDice(OnDiceRollComplete);
        }

        private void OnDiceRolled(int result)
        {
            Debug.Log($"Dice rolled: {result}");
        }

        private void OnDiceRollComplete(int result)
        {
            Debug.Log($"Dice roll completed: {result}");
        }

        private void OnDestroy()
        {
            if (_gameService != null)
            {
                _gameService.OnDiceRolled -= OnDiceRolled;
            }

            if (rollButton != null)
            {
                rollButton.onClick.RemoveListener(OnRollButtonClicked);
            }
        }
    }
}