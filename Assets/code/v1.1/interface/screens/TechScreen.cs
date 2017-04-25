using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TechScreen : MonoBehaviour
    {
        private ObjectLog _obj_log;
        private string _target_data;
        private InterfaceMenu _menus;
        private Coroutine _process;

        public void Init()
        {
            Start();
        }

        private void Start()
        {
            if (_obj_log != null)
                return;

            _menus = FindObjectOfType<InterfaceMenu>();
            _obj_log = GetComponentInChildren<ObjectLog>();
        }
        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.TechRecombination)
                return;

            Time.timeScale = 0.0f;
        }
    }
}