using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationLog : MonoBehaviour {
    private List<Notification> _notification_history;
    public void Push(Notification notif)
    {
        _notification_history.Add(notif);
    }
}
