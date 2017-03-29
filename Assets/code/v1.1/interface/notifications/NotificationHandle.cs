using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class NotificationHandle : MonoBehaviour
    {

        public UnityEngine.UI.Text Header;
        public UnityEngine.UI.Text Subheader;
        public UnityEngine.UI.Text Body;

        private Notification _current = null;

        public void push(Notification notif)
        {
            Header.text = notif.HeaderLabel;
            Subheader.text = notif.SubHeaderLabel;
            Body.text = notif.BodyText;
            _current = notif;
        }
    }
}