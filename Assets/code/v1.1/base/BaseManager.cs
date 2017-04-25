using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class BaseManager : MonoBehaviour {

        private InterfaceMenu _menus;
        private Player _player;
        private BaseResources _resources;
        public CameraView StationCamera;
        public float TransitionRate = 2.0f;

        public BaseResources Resources()
        {
            return _resources;
        }
        public void Dock()
        {
            _player.cameraSystem.SetTarget(StationCamera, TransitionRate);
        }
        public void UnDock()
        {
            _player.cameraSystem.SetTarget(_player.cameraSystem.PlayerRear, TransitionRate);
        }

        void Start() {
            _player = FindObjectOfType<Player>();
            _menus = FindObjectOfType<InterfaceMenu>();
            _resources = GetComponent<BaseResources>();
        }
        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.Docking)
                return;

            Time.timeScale = 1.0f;
        }
    }
}