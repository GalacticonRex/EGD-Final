using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class Notification
    {
        public string HeaderLabel;
        public string SubHeaderLabel;
        public string BodyText;
        public Notification() { }
        public Notification(string h, string sh, string b)
        {
            HeaderLabel = h;
            SubHeaderLabel = sh;
            BodyText = b;
        }
    }
}