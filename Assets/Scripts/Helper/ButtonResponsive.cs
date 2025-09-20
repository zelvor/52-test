using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonResponsive : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private float pressedScale = 0.9f;
    [SerializeField] private float duration = 0.1f;

    private Vector3 originalScale;
    private bool isPressed;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * pressedScale));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            isPressed = false;
            StopAllCoroutines();
            StartCoroutine(ScaleTo(originalScale));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPressed)
        {
            isPressed = false;
            StopAllCoroutines();
            StartCoroutine(ScaleTo(originalScale));
        }
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.localScale = target;
    }
}