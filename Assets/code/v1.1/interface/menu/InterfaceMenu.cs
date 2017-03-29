using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class InterfaceMenu : MonoBehaviour
    {
        public enum MenuType
        {
            Regular,
            Docking,
            TechRecombination
        }
        public InterfaceElem RegularMenu;
        public InterfaceElem DockingMenu;
        public InterfaceElem TechRecombinationMenu;

        private MenuType _current_menu;
        private InterfaceElem _current;

        public void GoTo(MenuType menu)
        {
            switch (menu)
            {
                case MenuType.Regular:
                    _current.Hide();
                    RegularMenu.Show();
                    _current = RegularMenu;
                    break;
                case MenuType.Docking:
                    _current.Hide();
                    DockingMenu.Show();
                    _current = DockingMenu;
                    break;
                case MenuType.TechRecombination:
                    _current.Hide();
                    TechRecombinationMenu.Show();
                    _current = TechRecombinationMenu;
                    break;
                default:
                    break;
            }
        }
        private void Start()
        {
            if (RegularMenu == null)
                Destroy(this);

            _current_menu = MenuType.Regular;
            _current = RegularMenu;

            RegularMenu.Show();
            if ( DockingMenu != null )
                DockingMenu.Hide();
            if (TechRecombinationMenu != null)
                TechRecombinationMenu.Hide();
        }
    }
}