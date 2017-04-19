using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class InterfaceMenu : MonoBehaviour
    {
        public enum MenuType
        {
            None = 0x0,
            Regular = 0x1,
            Docking = 0x2,
            TechRecombination = 0x4,
            ArtifactViewing = 0x8,
            LetterBoxView = 0x10,
            UIMenu = 0xE
        }
        public InterfaceElem RegularMenu;
        public InterfaceElem DockingMenu;
        public InterfaceElem TechRecombinationMenu;
        public InterfaceElem ArtifactViewingMenu;
        public InterfaceElem LetterBoxMenu;

        private MenuType _current_menu;
        private InterfaceElem _current;

        private Dictionary<InterfaceElem, Renderer[]> _renderers;

        public MenuType CurrentMenu
        {
            get { return _current_menu; }
        }
        public bool test(MenuType bitfield)
        {
            MenuType bitand = _current_menu & bitfield;
            return bitand != MenuType.None;
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
                case MenuType.LetterBoxView:
                    _current = LetterBoxMenu;
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
            if (LetterBoxMenu!=null)
            {
                LetterBoxMenu.Hide();
                LetterBoxMenu.OnCreate.Invoke();
            }
        }
    }
}