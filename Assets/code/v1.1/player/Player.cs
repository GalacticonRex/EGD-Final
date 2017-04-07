﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{

    public class Player : MonoBehaviour
    {
        private InputManager _inputs;
        private ObjectSeletor _selector;
        private ResourceManager _resources;
        private CameraSystem _camera;
        private Navigator _navigation;
        private BaseManager _base;
        private InterfaceMenu _menus;
        private DroneManager _manager;

        private bool _docked;
        private float _internal_time_scale = 1.0f;

        public InputManager inputs
        {
            get { return _inputs; }
        }
        public ObjectSeletor selector
        {
            get { return _selector; }
        }
        public ResourceManager resourceManager
        {
            get { return _resources; }
        }
        public CameraSystem cameraSystem
        {
            get { return _camera; }
        }
        public Navigator navigator
        {
            get { return _navigation; }
        }
        public DroneManager droneManager
        {
            get { return _manager; }
        }
        public bool docked
        {
            get { return _docked; }
        }
        public float timeScale
        {
            get { return _internal_time_scale; }
        }

        private void OnTriggerEnter(Collider other)
        {
            BaseManager b = other.GetComponent<BaseManager>();
            if ( _base != null && b == _base )
            {
                _menus.GoTo(InterfaceMenu.MenuType.Docking);
                _docked = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            BaseManager b = other.GetComponent<BaseManager>();
            if (_base != null && b == _base)
            {
                _menus.GoTo(InterfaceMenu.MenuType.Regular);
                _docked = false;
            }
        }

        private void Start()
        {
            _inputs = GetComponent<InputManager>();
            _selector = GetComponent<ObjectSeletor>();
            _resources = GetComponent<ResourceManager>();
            _camera = GetComponent<CameraSystem>();
            _navigation = GetComponent<Navigator>();
            _base = FindObjectOfType<BaseManager>();
            _menus = FindObjectOfType<InterfaceMenu>();
            _manager = FindObjectOfType<DroneManager>();
        }

        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.Regular)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _camera.scanMode = !_camera.scanMode;
                if (_camera.scanMode)
                    _internal_time_scale = 0.0f;
                else
                    _internal_time_scale = 1.0f;
            }
            Time.timeScale = _internal_time_scale;
        }
    }

}