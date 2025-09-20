using UnityEngine;
using DiceGame.Core;

namespace DiceGame.Infrastructure
{
    public class DiceGameContainer : MonoBehaviour
    {
        [Header("Service References")]
        [SerializeField] private UnityDiceAnimation diceAnimation;
        [SerializeField] private UnityScoreService scoreService;
        
        [Header("Configuration")]
        [SerializeField] private int randomSeed = -1;
        
        private IDiceRoller _diceRoller;
        private IScoreService _coreScoreService;
        private IGameService _gameService;
        
        public IGameService GameService => _gameService;
        public IScoreService ScoreService => _coreScoreService;
        
        private void Awake()
        {
            InitializeServices();
        }
        
        private void InitializeServices()
        {
            if (diceAnimation == null)
            {
                Debug.LogError("DiceAnimation is not assigned!");
                return;
            }
            
            if (scoreService == null)
            {
                Debug.LogError("ScoreService is not assigned!");
                return;
            }
            
            _diceRoller = new DiceRollService(randomSeed == -1 ? null : randomSeed);
            _coreScoreService = scoreService;
            _gameService = new GameService(_diceRoller, diceAnimation, _coreScoreService);
            
            Debug.Log("Dice game services initialized successfully");
        }
        
        public T GetService<T>() where T : class
        {
            if (typeof(T) == typeof(IGameService)) return _gameService as T;
            if (typeof(T) == typeof(IScoreService)) return _coreScoreService as T;
            if (typeof(T) == typeof(IDiceRoller)) return _diceRoller as T;
            
            return null;
        }
        
        private void OnValidate()
        {
            if (diceAnimation == null)
                diceAnimation = GetComponentInChildren<UnityDiceAnimation>();
            
            if (scoreService == null)
                scoreService = GetComponentInChildren<UnityScoreService>();
        }
    }
}
