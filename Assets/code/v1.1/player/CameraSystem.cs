using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CameraSystem : MonoBehaviour
    {
        #region Static Methods
        static public float FrustumHeightAtDistance(Camera cam, float distance)
        {
            return 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }
        static public float FOVForHeightAndDistance(float height, float distance)
        {
            return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
        }
        #endregion

        #region Properties
        public float ratio
        {
            get
            {
                return _camera_lerp * _camera_scanner.ratio + (1.0f - _camera_lerp) * _camera_back_view.ratio;
            }
        }
        public float interpolation
        {
            get
            {
                return _camera_lerp;
            }
        }
        public bool scanMode
        {
            get { return _isometric_mode; }
            set
            {
                _isometric_mode = value;
                if ( _isometric_mode )
                {
                    _camera_back_view.active = false;
                    _camera_scanner.active = true;
                }
                else
                {
                    _camera_back_view.active = true;
                    _camera_scanner.active = false;
                }
            }
        }
        public Vector3 mouseDelta
        {
            get
            {
                Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                return mousePos - _last_mouse_pos;
            }
        }
        #endregion

        #region Private Attributes
        private Camera _player_camera;

        private float _camera_lerp;

        private CameraScanner _camera_scanner;
        private CameraBackView _camera_back_view;

        private Vector2 _last_mouse_pos;

        private bool _isometric_mode;
        #endregion

        void Start()
        {
            _player_camera = GetComponentInChildren<Camera>();
            _camera_scanner = GetComponentInChildren<CameraScanner>();
            _camera_back_view = GetComponentInChildren<CameraBackView>();

            _last_mouse_pos = Input.mousePosition;

            _isometric_mode = false;

            _camera_lerp = 1.0f;

            _camera_back_view.active = true;
            _camera_scanner.active = false;
        }

        void Update()
        {
            Vector2 mouseMove = mouseDelta;

            if (_isometric_mode)
            {
                if (_camera_lerp > 0.0f)
                {
                    _camera_lerp -= 0.05f;
                }
            }
            else
            {
                if (_camera_lerp < 1.0f)
                {
                    _camera_lerp += 0.05f;
                }
            }

            _player_camera.fieldOfView = _camera_scanner.fieldOfView * (1 - _camera_lerp) + _camera_back_view.fieldOfView * _camera_lerp;
            _player_camera.transform.rotation = Quaternion.Lerp(_camera_scanner.rotation, _camera_back_view.rotation, _camera_lerp);

            float arm = (_camera_lerp) * _camera_back_view.cameraDistance + (1 - _camera_lerp) * _camera_scanner.cameraDistance;
            _player_camera.transform.position = transform.position + _player_camera.transform.rotation * new Vector3(0, 0, -arm);
        }
        private void LateUpdate()
        {
            _last_mouse_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}