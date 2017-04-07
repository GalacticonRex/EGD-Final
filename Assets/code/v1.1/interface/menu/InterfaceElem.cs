using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class InterfaceElem : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent OnCreate;
        public UnityEngine.Events.UnityEvent OnShow;
        public UnityEngine.Events.UnityEvent OnHide;

        public void Show()
        {
            gameObject.SetActive(true);
            OnShow.Invoke();
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide.Invoke();
        }
    }
}