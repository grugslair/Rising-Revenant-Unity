using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotificationBehaviourElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Slider timeBar;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private RawImage icon;
    [SerializeField] private GameObject actualParent; // The parent object that will shrink and be destroyed

    public void OnPointerClick(PointerEventData eventData)
    {
        // When notification is clicked, start the shrink and destroy process
        StartCoroutine(ShrinkAndDestroy());
    }

    public void InitializedNotification(string message, Texture iconTexture, float duration)
    {
        messageText.text = message;
        icon.texture = iconTexture;
        StartCoroutine(Countdown(duration));
    }

    private IEnumerator Countdown(float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        timeBar.value = 1;

        while (Time.time < endTime)
        {
            float remainingTime = endTime - Time.time;
            timeBar.value = remainingTime / duration;
            yield return null;
        }

        // After countdown, start shrinking before destroying
        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        Vector3 originalScale = actualParent.transform.localScale;
        float shrinkDuration = 0.5f; // Duration of the shrink effect in seconds
        float elapsedTime = 0;

        while (elapsedTime < shrinkDuration)
        {
            // Smoothly interpolate the scale of the actualParent to zero
            actualParent.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // Destroy the parent object after shrinking
    }
}
