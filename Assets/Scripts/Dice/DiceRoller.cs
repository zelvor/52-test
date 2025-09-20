using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] private Transform diceTransform;
    [SerializeField] private float spinDuration = 1.2f;
    [SerializeField] private float spinSpeed = 720f;
    [SerializeField] private AnimationCurve spinCurve = AnimationCurve.EaseInOut(0,1,1,0);
    [SerializeField] private float settleDuration = 0.5f;
    [SerializeField] private float overshootAngle = 20f;
    [SerializeField] private AnimationCurve settleCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private Quaternion[] faceRotations = new Quaternion[6];

    private bool isRolling;

    public void PlayRoll(int result, Action onComplete)
    {
        if (!isRolling)
            StartCoroutine(RollRoutine(result, onComplete));
    }

    private IEnumerator RollRoutine(int result, Action onComplete)
    {
        isRolling = true;

        // spin
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

        // settle
        Quaternion start = diceTransform.localRotation;
        Quaternion target = faceRotations[result - 1];
        Quaternion overshoot = target * Quaternion.Euler(Random.insideUnitSphere * overshootAngle);

        float tA = 0f;
        float durA = settleDuration * 0.6f;
        while (tA < 1f)
        {
            tA += Time.deltaTime / durA;
            float e = settleCurve.Evaluate(Mathf.Clamp01(tA));
            diceTransform.localRotation = Quaternion.Slerp(start, overshoot, e);
            yield return null;
        }

        float tB = 0f;
        float durB = settleDuration * 0.4f;
        while (tB < 1f)
        {
            tB += Time.deltaTime / durB;
            float e = settleCurve.Evaluate(Mathf.Clamp01(tB));
            diceTransform.localRotation = Quaternion.Slerp(overshoot, target, e);
            yield return null;
        }

        isRolling = false;
        onComplete?.Invoke();
    }
}
