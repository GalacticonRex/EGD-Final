using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastStar
{
    public class WormholeTransport : MonoBehaviour
    {
        public CameraView TargetView;
        public string NextScene;
        public float FadeDelay;
        public float FadeDuration;

        private float _particle_count;
        private bool _hit;
        private CameraStatic _stat_cam;

        private void Start()
        {
            _stat_cam = GetComponent<CameraStatic>();
            _hit = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.cameraSystem.SetTarget(TargetView, 1.0f);
                p.menu.GoTo(InterfaceMenu.MenuType.GoToColorView);
                p.menu.element(InterfaceMenu.MenuType.GoToColorView).
                    GetComponent<GoToColorScreen>().
                    Push(Color.black, NextScene, FadeDelay, FadeDuration);
                _hit = true;
            }
        }
        private void Update()
        {
            if (_hit)
            {
                _stat_cam.Distance -= 2.0f * _stat_cam.Rotation.y * Time.unscaledDeltaTime;
                _stat_cam.Rotation.y += 15.0f * Time.unscaledDeltaTime;
                _stat_cam.RotationOverTime.x += 2.0f * _stat_cam.Rotation.y * Time.unscaledDeltaTime;
            }
        }
    }
}