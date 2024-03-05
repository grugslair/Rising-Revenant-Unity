using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationPrefab;
    public Transform notificationPanel;

    private void Awake()
    {
        UiEntitiesReferenceManager.notificationManager = this;
    }

    public void CreateNotification(string message, Texture iconTexture, float duration)
    {
        GameObject notification = Instantiate(notificationPrefab, notificationPanel);
        notification.GetComponent<NotificationBehaviourElement>().InitializedNotification(message, iconTexture, duration);
    }
}
