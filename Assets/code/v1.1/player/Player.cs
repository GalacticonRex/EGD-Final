using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{

    public class Player : MonoBehaviour
    {

        private ObjectSeletor _selector;
        private ResourceManager _resources;
        private CameraSystem _camera;
        private Navigator _navigation;

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

        void Start()
        {
            _selector = GetComponent<ObjectSeletor>();
            _resources = GetComponent<ResourceManager>();
            _camera = GetComponent<CameraSystem>();
            _navigation = GetComponent<Navigator>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _camera.scanMode = !_camera.scanMode;
            }
        }
    }

}