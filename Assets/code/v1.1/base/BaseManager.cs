using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class BaseManager : MonoBehaviour {

        private InterfaceMenu _menus;
        private Player _player;
        private BaseResources _resources;
        private CameraScanner _camera;

        public BaseResources Resources()
        {
            return _resources;
        }
        public void Dock()
        {
            _player.cameraSystem.SetTargetIndex(_camera);
        }
        public void UnDock()
        {
            _player.cameraSystem.SetTargetIndex(_player.cameraSystem.PlayerRear);
        }

        void Start() {
            _player = FindObjectOfType<Player>();
            _menus = FindObjectOfType<InterfaceMenu>();
            _resources = GetComponent<BaseResources>();
            _camera = GetComponent<CameraScanner>();
        }
        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.Docking)
                return;

            Time.timeScale = 1.0f;
        }
    }
}