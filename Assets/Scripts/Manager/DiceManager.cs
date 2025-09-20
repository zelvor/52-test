using UnityEngine;
using System.Collections;

public class DiceManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform diceTransform;

    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 1.2f;
    [SerializeField] private float spinSpeed = 720f;
    [SerializeField] private AnimationCurve spinCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("Settle Settings")]
    [SerializeField] private float settleDuration = 0.5f;
    [SerializeField] private float overshootAngle = 20f;
    [SerializeField] private AnimationCurve settleCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("Baked local rotations for faces 1–6 (set via ContextMenu)")]
    [SerializeField] private Quaternion[] faceRotations = new Quaternion[6];

    private bool isRolling = false;

    public void Roll(System.Action<int> onComplete)
    {
        if (!isRolling)
            StartCoroutine(RollRoutine(onComplete));
    }

    private IEnumerator RollRoutine(System.Action<int> onComplete)
    {
        isRolling = true;

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

        int result = Random.Range(1, 7);
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

        onComplete?.Invoke(result);
        isRolling = false;
    }

#if UNITY_EDITOR
    [ContextMenu("Bake Face 1")] private void Bake1() => Bake(0);
    [ContextMenu("Bake Face 2")] private void Bake2() => Bake(1);
    [ContextMenu("Bake Face 3")] private void Bake3() => Bake(2);
    [ContextMenu("Bake Face 4")] private void Bake4() => Bake(3);
    [ContextMenu("Bake Face 5")] private void Bake5() => Bake(4);
    [ContextMenu("Bake Face 6")] private void Bake6() => Bake(5);

    private void Bake(int index)
    {
        faceRotations[index] = diceTransform.localRotation;
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"Baked Face {index + 1}: {faceRotations[index].eulerAngles}");
    }

    [ContextMenu("Clear All Bakes")]
    private void Clear()
    {
        faceRotations = new Quaternion[6];
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log("Cleared all baked rotations");
    }
#endif
}
