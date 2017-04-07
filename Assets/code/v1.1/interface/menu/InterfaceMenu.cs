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
            TechRecombination,
            ArtifactViewing
        }
        public InterfaceElem RegularMenu;
        public InterfaceElem DockingMenu;
        public InterfaceElem TechRecombinationMenu;
        public InterfaceElem ArtifactViewingMenu;

        private MenuType _current_menu;
        private InterfaceElem _current;

        private Dictionary<InterfaceElem, Renderer[]> _renderers;

        public MenuType CurrentMenu
        {
            get { return _current_menu; }
        }

        public void GoTo(MenuType menu)
        {
            _current.Hide();
            switch (menu)
            {
                case MenuType.Regular:
                    _current = RegularMenu;
                    break;
                case MenuType.Docking:
                    _current = DockingMenu;
                    break;
                case MenuType.TechRecombination:
                    _current = TechRecombinationMenu;
                    break;
                case MenuType.ArtifactViewing:
                    _current = ArtifactViewingMenu;
                    break;
                default:
                    break;
            }
            _current_menu = menu;
            _current.Show();
        }
        private void Awake()
        {
            if (RegularMenu == null)
                Destroy(this);

            _renderers = new Dictionary<InterfaceElem, Renderer[]>();

            _current_menu = MenuType.Regular;
            _current = RegularMenu;

            RegularMenu.Show();
            RegularMenu.OnCreate.Invoke();

            if (DockingMenu != null)
            {
                DockingMenu.Hide();
                DockingMenu.OnCreate.Invoke();
            }
            if (TechRecombinationMenu!=null)
            {
                TechRecombinationMenu.Hide();
                TechRecombinationMenu.OnCreate.Invoke();
            }
            if (ArtifactViewingMenu!=null)
            {
                ArtifactViewingMenu.Hide();
                ArtifactViewingMenu.OnCreate.Invoke();
            }
        }
    }
}