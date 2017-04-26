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

        private Stack<MenuType> _current_menu = new Stack<MenuType>();
        private Dictionary<MenuType, InterfaceElem> _sorted_elements = new Dictionary<MenuType, InterfaceElem>();
        private InterfaceElem _current;
        private InterfaceElem[] _listed_elements;

        private Dictionary<InterfaceElem, Renderer[]> _renderers = new Dictionary<InterfaceElem, Renderer[]>();

        public MenuType CurrentMenu
        {
            get { return (_current_menu.Count==0)?MenuType.None:_current_menu.Peek(); }
        }
        public InterfaceElem element(MenuType type)
        {
            return _sorted_elements[type];
        }
        public bool test(MenuType bitfield)
        {
            MenuType bitand = _current_menu.Peek() & bitfield;
            return bitand != MenuType.None;
        }

        public void Push(MenuType menu)
        {
            _current_menu.Push(menu);

            if ( _current != null )
                _current.Hide();
            _current = _sorted_elements[menu];
            _current.Show();
        }
        public void Pop()
        {
            _current_menu.Pop();

            _current.Hide();
            if (_current_menu.Count > 0)
            {
                _current = _sorted_elements[_current_menu.Peek()];
                _current.Show();
            }
        }
        private void Awake()
        {
            _listed_elements = GetComponentsInChildren<InterfaceElem>(true);

            foreach ( InterfaceElem elem in _listed_elements )
            {
                if ( elem.Type == MenuType.Regular )
                {
                    _current = elem;
                    _current.Show();
                    _current_menu.Push(MenuType.Regular);
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