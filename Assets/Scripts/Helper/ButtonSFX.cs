using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerClickHandler
{
    [Header("SFX")]
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private float volume = 1f;

    [SerializeField] private AudioSource audioSource;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null)
        {
            audioSource.PlayOneShot(clickClip, volume);
        }
    }
}