using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class VisibleOnlyToScanner : MonoBehaviour {
        public Renderer[] Visible;
        private CameraSystem _camera;

        void Start() {
            _camera = FindObjectOfType<CameraSystem>();
        }

        // Update is called once per frame
        void Update() {
            if (_camera.interpolation < 0.9f)
            {
                foreach(Renderer r in Visible)
                    r.enabled = true;
            }
            else
            {
                foreach (Renderer r in Visible)
                    r.enabled = false;
            }
        }
    }
}