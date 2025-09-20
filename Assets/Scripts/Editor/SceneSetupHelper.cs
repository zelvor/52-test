using UnityEngine;
using UnityEditor;
using DiceGame.Infrastructure;
using DiceGame.Presentation;

namespace DiceGame.Editor
{
    public class SceneSetupHelper : EditorWindow
    {
        [MenuItem("DiceGame/Setup Scene")]
        public static void SetupScene()
        {
            SetupDiceGameContainer();
            SetupDiceAnimation();
            SetupScoreService();
            SetupGameManager();
            
            Debug.Log("Scene setup completed!");
        }
        
        private static void SetupDiceGameContainer()
        {
            GameObject container = GameObject.Find("DiceGameContainer");
            if (container == null)
            {
                container = new GameObject("DiceGameContainer");
            }
            
            if (container.GetComponent<DiceGameContainer>() == null)
            {
                container.AddComponent<DiceGameContainer>();
            }
        }
        
        private static void SetupDiceAnimation()
        {
            GameObject dice = GameObject.Find("Dice");
            if (dice == null)
            {
                dice = GameObject.CreatePrimitive(PrimitiveType.Cube);
                dice.name = "Dice";
            }
            
            if (dice.GetComponent<UnityDiceAnimation>() == null)
            {
                dice.AddComponent<UnityDiceAnimation>();
            }
            
            DiceGameContainer container = FindObjectOfType<DiceGameContainer>();
            if (container != null)
            {
                SerializedObject containerSO = new SerializedObject(container);
                SerializedProperty diceAnimationProp = containerSO.FindProperty("diceAnimation");
                diceAnimationProp.objectReferenceValue = dice.GetComponent<UnityDiceAnimation>();
                containerSO.ApplyModifiedProperties();
            }
        }
        
        private static void SetupScoreService()
        {
            GameObject scoreUI = GameObject.Find("ScoreUI");
            if (scoreUI == null)
            {
                scoreUI = new GameObject("ScoreUI");
            }
            
            if (scoreUI.GetComponent<UnityScoreService>() == null)
            {
                scoreUI.AddComponent<UnityScoreService>();
            }
            
            DiceGameContainer container = FindObjectOfType<DiceGameContainer>();
            if (container != null)
            {
                SerializedObject containerSO = new SerializedObject(container);
                SerializedProperty scoreServiceProp = containerSO.FindProperty("scoreService");
                scoreServiceProp.objectReferenceValue = scoreUI.GetComponent<UnityScoreService>();
                containerSO.ApplyModifiedProperties();
            }
        }
        
        private static void SetupGameManager()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                DiceGameContainer container = FindObjectOfType<DiceGameContainer>();
                if (container != null)
                {
                    SerializedObject gameManagerSO = new SerializedObject(gameManager);
                    SerializedProperty containerProp = gameManagerSO.FindProperty("gameContainer");
                    containerProp.objectReferenceValue = container;
                    gameManagerSO.ApplyModifiedProperties();
                }
            }
        }
    }
}
