using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class FlareLayerManager : MonoBehaviour
    {

        public float Span;
        private CameraSystem _camera_system;

        void Start()
        {
            _camera_system = FindObjectOfType<CameraSystem>();
        }
        void Update()
        {
            float dist = Vector3.Distance(transform.position, _camera_system.FlareCamera.transform.position);
            _camera_system.flareNearClip = Mathf.Max(0.3f, dist - dist * Span);
            _camera_system.flareFarClip = dist + dist * Span;
        }
    }
}