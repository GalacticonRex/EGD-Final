using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ChangeCameraView : MonoBehaviour {

        public CameraView Target;
        public float TransitionRate;
        private CameraSystem _system;

        public void SetView()
        {
            _system.SetTarget(Target, TransitionRate);
        }
        private void Awake()
        {
            _system = FindObjectOfType<CameraSystem>();
        }
    }
}