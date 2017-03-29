using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class NotificationLog : MonoBehaviour
    {
        public GameObject NotificationInstance;

        private List<Notification> _notification_history;
        private List<NotificationHandle> _notifications;
        private RectTransform _self;

        public void push(Notification notif)
        {
            _notification_history.Add(notif);

            GameObject n = Instantiate(NotificationInstance);
            RectTransform r = n.GetComponent<RectTransform>();
            r.SetParent(transform);

            // >>>>>>>>>>>> GROSS >>>>>>>>>>>>>>>>>>>>>>>>>>>>
            r.anchoredPosition = new Vector2(0, 0);
            r.offsetMin = new Vector2(0, -80);
            r.offsetMax = new Vector2(0, 0);
            r.localScale = new Vector3(1, 1, 1);
            // <<<<<<<<<<<< GROSS <<<<<<<<<<<<<<<<<<<<<<<<<<<<

            NotificationHandle nf = n.GetComponent<NotificationHandle>();
            nf.push(notif);

            foreach (NotificationHandle hnd in _notifications)
            {
                hnd.transform.localPosition = hnd.transform.localPosition - new Vector3(0, 80, 0);
            }
            _notifications.Add(nf);
        }

        private void Start()
        {
            _notification_history = new List<Notification>();
            _notifications = new List<NotificationHandle>();
            _self = GetComponent<RectTransform>();
        }
    }
}