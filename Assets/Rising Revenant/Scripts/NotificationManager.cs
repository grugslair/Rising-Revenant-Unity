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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateNotification("This is a test notification", null, 25f);
        }
    }
}
