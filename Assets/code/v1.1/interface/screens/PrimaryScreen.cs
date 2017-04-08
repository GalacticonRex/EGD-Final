using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class PrimaryScreen : MonoBehaviour {

        public GameObject DockButton;
        private BaseManager _manager;
        private InterfaceMenu _menus;

        public void AllowDocking()
        {
            DockButton.SetActive(true);
        }
        public void DisallowDocking()
        {
            DockButton.SetActive(false);
        }

        public void CommenceDocking()
        {
            if (_manager != null)
            {
                _manager.Dock();
                _menus.GoTo(InterfaceMenu.MenuType.Docking);
            }
        }

        private void Start() {
            _manager = FindObjectOfType<BaseManager>();
            _menus = FindObjectOfType<InterfaceMenu>();
        }
        private void Update() {

        }
    }
}