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
            GoToColorView = 0xFF,
            UIMenu = 0xE
        }

        private MenuType _current_menu;
        private InterfaceElem _current;
        private InterfaceElem[] _listed_elements;
        private Dictionary<MenuType, InterfaceElem> _sorted_elements;

        private Dictionary<InterfaceElem, Renderer[]> _renderers;

        public MenuType CurrentMenu
        {
            get { return _current_menu; }
        }
        public InterfaceElem element(MenuType type)
        {
            return _sorted_elements[type];
        }
        public bool test(MenuType bitfield)
        {
            MenuType bitand = _current_menu & bitfield;
            return bitand != MenuType.None;
        }

        public void GoTo(MenuType menu)
        {
            _current.Hide();
            _current = _sorted_elements[menu];
            _current_menu = menu;
            _current.Show();
        }
        private void Awake()
        {
            _renderers = new Dictionary<InterfaceElem, Renderer[]>();

            _listed_elements = GetComponentsInChildren<InterfaceElem>(true);
            _sorted_elements = new Dictionary<MenuType, InterfaceElem>();

            foreach( InterfaceElem elem in _listed_elements )
            {
                if ( elem.Type == MenuType.Regular )
                {
                    elem.Show();

                    _current_menu = MenuType.Regular;
                    _current = elem;
                }
                else
                {
                    elem.Hide();
                }
                elem.OnCreate.Invoke();
                _sorted_elements[elem.Type] = elem;
            }
        }
    }
}