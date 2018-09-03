using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotificationList
{
    public bool AllNotificationsRead;

    [SerializeField]
    private List<Notification> notifications;

    #region <Properties>
    public int Count
    {
        get { return notifications.Count; }
    }
    #endregion

    #region <Constructor>
    public NotificationList()
    {
        notifications = new List<Notification>();

        AllNotificationsRead = true;
    }
    #endregion

    #region <Methods>
    public List<Notification> GetAll()
    {
        return notifications;
    }

    public List<Notification> GetNew()
    {
        List<Notification> nList = new List<Notification>();

        for (int i = 0; i < notifications.Count; i++)
        {
            if (!notifications[i].Read)
                nList.Add(notifications[i]);
        }

        return nList;
    }

    public void Add(string notificationText)
    {
		
        Notification newNotification = new Notification(notificationText);
        int maxNotifications = GameMaster.Instance.MaxStoredNotifications;

        if (notifications.Count == maxNotifications)
            notifications.RemoveAt(0);

        notifications.Add(newNotification);

        AllNotificationsRead = false;

        //TEMP:
        Debug.Log("*New Notification: \"" + notificationText + "\"");

		GameMaster.Instance.GUIManager.note.AddNotifications ();

    }
    #endregion
}
