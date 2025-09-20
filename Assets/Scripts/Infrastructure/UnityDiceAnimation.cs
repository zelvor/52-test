using System;
using System.Collections;
using UnityEngine;
using DiceGame.Core;

namespace DiceGame.Infrastructure
{
    public class UnityDiceAnimation : MonoBehaviour, IDiceAnimation
    {
        [Header("Animation Settings")]
        [SerializeField] private float spinDuration = 1.2f;
        [SerializeField] private float spinSpeed = 720f;
        [SerializeField] private AnimationCurve spinCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [SerializeField] private float settleDuration = 0.5f;
        [SerializeField] private float overshootAngle = 20f;
        [SerializeField] private AnimationCurve settleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("Dice Configuration")]
        [SerializeField] private Transform diceTransform;
        [SerializeField] private Quaternion[] faceRotations = new Quaternion[6];
        
        private bool _isAnimating;
        
        public bool IsAnimating => _isAnimating;
        
        private void Awake()
        {
            ValidateConfiguration();
        }
        
        public void PlayRollAnimation(int result, Action onComplete)
        {
            if (_isAnimating)
            {
                Debug.LogWarning("Animation already in progress");
                onComplete?.Invoke();
                return;
            }
            
            if (!IsValidResult(result))
            {
                Debug.LogError($"Invalid dice result: {result}");
                onComplete?.Invoke();
                return;
            }
            
            StartCoroutine(RollAnimationRoutine(result, onComplete));
        }
        
        private IEnumerator RollAnimationRoutine(int result, Action onComplete)
        {
            _isAnimating = true;
            
            try
            {
                yield return StartCoroutine(SpinPhase());
                yield return StartCoroutine(SettlePhase(result));
            }
            finally
            {
                _isAnimating = false;
                onComplete?.Invoke();
            }
        }
        
        private IEnumerator SpinPhase()
        {
            float elapsed = 0f;
            while (elapsed < spinDuration)
            {
                float t = elapsed / spinDuration;
                float speedFactor = spinCurve.Evaluate(t);
                
                diceTransform.Rotate(
                    spinSpeed * speedFactor * Time.deltaTime,
                    spinSpeed * speedFactor * Time.deltaTime,
                    0,
                    Space.Self
                );
                
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator SettlePhase(int result)
        {
            Quaternion startRotation = diceTransform.localRotation;
            Quaternion targetRotation = faceRotations[result - 1];
            Quaternion overshootRotation = targetRotation * Quaternion.Euler(UnityEngine.Random.insideUnitSphere * overshootAngle);
            
            yield return StartCoroutine(AnimateRotation(startRotation, overshootRotation, settleDuration * 0.6f));
            yield return StartCoroutine(AnimateRotation(overshootRotation, targetRotation, settleDuration * 0.4f));
        }
        
        private IEnumerator AnimateRotation(Quaternion from, Quaternion to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float curveValue = settleCurve.Evaluate(Mathf.Clamp01(t));
                diceTransform.localRotation = Quaternion.Slerp(from, to, curveValue);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            diceTransform.localRotation = to;
        }
        
        private bool IsValidResult(int result)
        {
            return result >= 1 && result <= 6;
        }
        
        private void ValidateConfiguration()
        {
            if (diceTransform == null)
            {
                Debug.LogError("DiceTransform is not assigned!");
                return;
            }
            
            bool hasValidRotations = true;
            for (int i = 0; i < faceRotations.Length; i++)
            {
                if (faceRotations[i] == Quaternion.identity)
                {
                    hasValidRotations = false;
                    break;
                }
            }
            
            if (!hasValidRotations)
            {
                Debug.LogWarning("Face rotations not properly configured. Use Context Menu to bake rotations.");
            }
        }
        
#if UNITY_EDITOR
        [ContextMenu("Bake Face 1")] private void Bake1() => Bake(0);
        [ContextMenu("Bake Face 2")] private void Bake2() => Bake(1);
        [ContextMenu("Bake Face 3")] private void Bake3() => Bake(2);
        [ContextMenu("Bake Face 4")] private void Bake4() => Bake(3);
        [ContextMenu("Bake Face 5")] private void Bake5() => Bake(4);
        [ContextMenu("Bake Face 6")] private void Bake6() => Bake(5);
        
        [ContextMenu("Clear All Bakes")]
        private void ClearAllBakes()
        {
            faceRotations = new Quaternion[6];
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log("Cleared all baked rotations");
        }
        
        private void Bake(int index)
        {
            if (diceTransform == null)
            {
                Debug.LogError("DiceTransform is not assigned!");
                return;
            }
            
            faceRotations[index] = diceTransform.localRotation;
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"Baked Face {index + 1}: {faceRotations[index].eulerAngles}");
        }
#endif
    }
}
