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
        public float additionalFOV
        {
            get
            {
                return _actual_additional_fov;
            }
            set
            {
                _target_additional_fov = value;
            }
        }
        public float interpolation
        {
            get
            {
                return 1.0f - _scanner_value;
            }
        }
        public float ratio
        {
            get
            {
                return _current_arm_length;
            }
        }
        public bool scanMode
        {
            get { return _target == PlayerScanner; }
            set
            {
                if (value)
                {
                    SetTarget(PlayerScanner, RearToScanner);
                }
                else if (_target == PlayerScanner)
                {
                    SetTarget(PlayerRear, ScannerToRear);
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

        public Light ScannerIlluminator;
        public CameraView InitialView;
        public CameraBackView PlayerRear;
        public CameraScanner PlayerScanner;
        public float RearToScanner = 0.5f;
        public float ScannerToRear = 1.2f;
        public Vector3 ShakeAmount;

        #region Private Attributes
        private InterfaceMenu _menus;
        private PilotedNavigator _nav;
        private Camera _player_camera;

        private CameraView[] _camera_views;

        private float _transition_rate;

        private Vector3 _current_position;
        private Quaternion _current_rotation;
        private float _current_arm_length;
        private float _current_fov;

        private Vector3 _source_position;
        private Quaternion _source_rotation;
        private float _source_arm_length;
        private float _source_fov;

        private CameraView _target;
        private float _view_lerp;
        private float _scanner_value;

        private Vector2 _last_mouse_pos;

        private float _actual_additional_fov;
        private float _target_additional_fov;
        #endregion

        public void SetTarget(CameraView v, float trans)
        {
            print("Set target to " + v.name);

            _transition_rate = trans;
            _source_position = _current_position;
            _source_rotation = _current_rotation;
            _source_arm_length = _current_arm_length;
            _source_fov = _current_fov;

            _target = v;
            _view_lerp = 0.0f;

            if (v != PlayerRear)
                _nav.Block(this);
            else
                _nav.Unblock(this);

        }
        private void UpdateValues()
        {
            _current_fov = _target.fieldOfView * _view_lerp + _source_fov * (1.0f - _view_lerp);
            _current_arm_length = _target.cameraDistance * _view_lerp + _source_arm_length * (1.0f - _view_lerp);
            _current_rotation = Quaternion.Lerp(_source_rotation, _target.rotation, _view_lerp);
            _current_position = Vector3.Lerp(_source_position, _target.transform.position, _view_lerp);

            _player_camera.fieldOfView = _current_fov + _actual_additional_fov;
            _player_camera.transform.rotation = _current_rotation;
            _player_camera.transform.position = _current_position + _current_rotation * new Vector3(0,0, 0.25f * _actual_additional_fov - _current_arm_length);

            _view_lerp = Mathf.Min(1.0f, _view_lerp + Time.unscaledDeltaTime / _transition_rate);
            if ( _target == PlayerScanner )
            {
                _scanner_value = Mathf.Min(1.0f, _scanner_value + Time.unscaledDeltaTime / _transition_rate);
            }
            else
            {
                _scanner_value = Mathf.Max(0.0f, _scanner_value - Time.unscaledDeltaTime / _transition_rate);
            }
        }

        private void Start()
        {
            _nav = FindObjectOfType<PilotedNavigator>();
            _menus = FindObjectOfType<InterfaceMenu>();

            _camera_views = FindObjectsOfType<CameraView>();
            _player_camera = GetComponentInChildren<Camera>();

            _target = InitialView;
            _view_lerp = 0.0f;

            if (_target != PlayerRear)
                _nav.Block(this);
            else
                _nav.Unblock(this);

            _source_fov = _current_fov = _target.fieldOfView;
            _source_arm_length = _current_arm_length = _target.cameraDistance;
            _source_rotation = _current_rotation = _target.rotation;
            _source_position = _current_position = _target.transform.position;

            _player_camera.fieldOfView = _current_fov + _actual_additional_fov;
            _player_camera.transform.rotation = _current_rotation;
            _player_camera.transform.position =
                Vector3.Scale(Random.onUnitSphere, ShakeAmount) +
                _current_position +
                _current_rotation * new Vector3(0, 0, 0.25f * _actual_additional_fov - _current_arm_length);

            _last_mouse_pos = Input.mousePosition;

            _actual_additional_fov = 0.0f;
            _target_additional_fov = 0.0f;
        }

        private void Update()
        {
            Vector2 mouseMove = mouseDelta;

            _actual_additional_fov = _target_additional_fov * 0.1f + _actual_additional_fov * 0.9f;

            UpdateValues();
        }
        private void LateUpdate()
        {
            ScannerIlluminator.intensity = _scanner_value;
            _last_mouse_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}