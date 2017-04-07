using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ChangeMenu : MonoBehaviour {
        public InterfaceMenu.MenuType Target = InterfaceMenu.MenuType.Regular;

        private InterfaceMenu _mainmenu;

        public void OpenMenu()
        {
            _mainmenu.GoTo(Target);
        }
        private void Start()
        {
            _mainmenu = GetComponentInParent<InterfaceMenu>();
        }
    }
}