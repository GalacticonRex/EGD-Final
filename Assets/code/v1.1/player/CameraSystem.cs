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
                return 1.0f - PlayerScanner.value;
            }
        }
        public float ratio
        {
            get
            {
                return _arm_length;
            }
        }
        public bool scanMode
        {
            get { return _target_index == _dereference[PlayerScanner]; }
            set
            {
                if (value)
                {
                    SetTargetIndex(PlayerScanner);
                }
                else if (_target_index == _dereference[PlayerScanner])
                {
                    SetTargetIndex(PlayerRear);
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

        public float TransitionRate = 0.05f;
        public Light ScannerIlluminator;
        public CameraBackView PlayerRear;
        public CameraScanner PlayerScanner;

        #region Private Attributes
        private InterfaceMenu _menus;
        private Camera _player_camera;

        private CameraView[] _camera_views;
        private Dictionary<CameraView, int> _dereference;
        private int _target_index;
        private float _arm_length;

        private Vector2 _last_mouse_pos;

        private float _actual_additional_fov;
        private float _target_additional_fov;
        #endregion

        public void SetTargetIndex(CameraView v)
        {
            _target_index = _dereference[v];
            for (int i=0;i<_camera_views.Length;i++ )
            {
                if (i == _target_index)
                    _camera_views[i].active = true;
                else
                    _camera_views[i].active = false;
            }
            Debug.Log("Target : " + _target_index.ToString());
        }
        private void NormalizeValues()
        {
            float total = 0.0f;
            foreach(CameraView cam in _camera_views)
            {
                total += cam.value;
            }
            foreach (CameraView cam in _camera_views)
            {
                cam.value = cam.value / total;
            }
        }
        private Quaternion MixCameras(CameraView a, CameraView b)
        {
            float total = a.value + b.value;
            return Quaternion.Lerp(a.rotation, b.rotation, b.value / total);
        }
        private void UpdateValues()
        {
            CameraView cam = _camera_views[_target_index];

            cam.value += TransitionRate;
            NormalizeValues();

            float fov = 0.0f;
            _arm_length = 0.0f;
            Vector3 pos = new Vector3();
            Quaternion rot = new Quaternion(0, 0, 0, 1);

            foreach (CameraView item in _camera_views)
            {
                fov += item.fieldOfView * item.value;
                _arm_length += item.cameraDistance * item.value;
                pos += item.transform.position * item.value;
                rot = Quaternion.Lerp(rot, item.rotation, item.value);
            }
                
            _player_camera.fieldOfView = fov;
            _player_camera.transform.rotation = rot;
            _player_camera.transform.position = pos + _player_camera.transform.rotation * new Vector3(0, 0, -_arm_length);
        }

        private void Start()
        {
            _menus = FindObjectOfType<InterfaceMenu>();

            _camera_views = FindObjectsOfType<CameraView>();
            _dereference = new Dictionary<CameraView, int>();
            for (int i = 0; i < _camera_views.Length; i++)
            {
                _dereference[_camera_views[i]] = i;
            }

            _target_index = _dereference[PlayerRear];

            foreach( CameraView v in _camera_views )
            {
                if (v == PlayerRear)
                {
                    v.active = true;
                    v.value = 1.0f;
                }
                else
                {
                    v.active = false;
                    v.value = 0.0f;
                }
            }

            _player_camera = GetComponentInChildren<Camera>();

            _last_mouse_pos = Input.mousePosition;

            _actual_additional_fov = 0.0f;
            _target_additional_fov = 0.0f;
        }

        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.Regular &&
                _menus.CurrentMenu != InterfaceMenu.MenuType.Docking)
                return;

            Vector2 mouseMove = mouseDelta;

            _actual_additional_fov = _target_additional_fov * 0.1f + _actual_additional_fov * 0.9f;

            UpdateValues();
        }
        private void LateUpdate()
        {
            ScannerIlluminator.intensity = PlayerScanner.value;
            _last_mouse_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}