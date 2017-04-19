using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastStar
{
    public class Cheats : MonoBehaviour
    {
        public KeyCode FillEnergy = KeyCode.None;
        public KeyCode CreateOre = KeyCode.None;
        public KeyCode ReturnToMainMenu = KeyCode.None;
        public KeyCode LetterBoxMe = KeyCode.None;
        public KeyCode JumpToAsteroid = KeyCode.None;

        private ResourceManager _resources;
        private CameraSystem _camera;
        private InterfaceMenu _interface;
        private InterfaceMenu.MenuType _last_menu;
        private CameraStatic _cam_static;

        private void Start()
        {
            _camera = FindObjectOfType<CameraSystem>();
            _resources = FindObjectOfType<ResourceManager>();
            _interface = FindObjectOfType<InterfaceMenu>();
            _cam_static = FindObjectOfType<CameraStatic>();
        }
        void Update()
        {
            if (_resources != null && Input.GetKeyDown(FillEnergy))
            {
                _resources.RequestEnergy(-100.0f);
            }
            if (_resources != null && Input.GetKeyDown(CreateOre))
            {
                _resources.RequestStorage(100.0f);
            }
            if ( Input.GetKeyDown(ReturnToMainMenu) )
            {
                SceneManager.LoadScene("title_screen");
            }
            if ( Input.GetKeyDown(LetterBoxMe) )
            {
                if (_interface.CurrentMenu == InterfaceMenu.MenuType.LetterBoxView)
                {
                    _interface.GoTo(_last_menu);
                    _camera.SetTarget(_camera.PlayerRear, 1.5f);
                }
                else
                {
                    _last_menu = _interface.CurrentMenu;
                    _interface.GoTo(InterfaceMenu.MenuType.LetterBoxView);
                    _camera.SetTarget(_cam_static, 1.5f);
                }
            }
        }
    }
}