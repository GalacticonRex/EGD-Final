using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class NotificationLog : MonoBehaviour
    {
        private ObjectLog _local;
        private List<Notification> _notification_history;
        private List<NotificationHandle> _notifications;
        private RectTransform _self;

        public void Push(Notification notif)
        {
            _notification_history.Add(notif);

            GameObject n = _local.Push();

            NotificationHandle nf = n.GetComponent<NotificationHandle>();
            nf.push(notif);

            _notifications.Add(nf);
        }

        private void Start()
        {
            _local = GetComponent<ObjectLog>();
            _notification_history = new List<Notification>();
            _notifications = new List<NotificationHandle>();
            _self = GetComponent<RectTransform>();
        }
    }
}