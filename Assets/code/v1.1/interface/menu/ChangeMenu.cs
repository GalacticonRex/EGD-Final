using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ChangeMenu : MonoBehaviour {

        public InterfaceMenu.MenuType Target = InterfaceMenu.MenuType.Regular;
        public InterfaceMenu MainMenu;

        public void OpenMenu()
        {
            MainMenu.GoTo(Target);
        }

    }
}