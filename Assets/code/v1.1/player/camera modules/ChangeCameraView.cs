using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ChangeCameraView : MonoBehaviour {

        public CameraView Target;
        private CameraSystem _system;

        public void SetView()
        {
            _system.SetTargetIndex(Target);
        }
        private void Start()
        {
            _system = FindObjectOfType<CameraSystem>();
        }
    }
}