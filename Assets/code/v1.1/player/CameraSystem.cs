using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {
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
            float norm = (_camera_arm + DistanceNormalMin) / (DistanceNormalMin + DistanceNormalMax);
            float iso = (_camera_arm + DistanceScanMin) / (DistanceScanMin + DistanceScanMax);
            return _camera_lerp * norm + (1.0f - _camera_lerp) * iso;
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
        set { _isometric_mode = value; }
    }
    #endregion

    #region Public Attributes
    public float DistanceScanMin = 200.0f;
    public float DistanceScanMax = 10000.0f;
    public float DistanceNormalMin = 10.0f;
    public float DistanceNormalMax = 80.0f;
    #endregion

    #region Private Attributes
    private Camera _player_camera;

    private float _camera_lerp;

    private float _camera_fov_iso;
    private float _camera_fov_norm;

    private float _camera_hgt;
    private float _camera_hgt_iso;
    private float _camera_hgt_norm;

    private float _camera_arm;
    private float _camera_arm_norm;
    private float _camera_arm_iso;

    private Quaternion _camera_rot;
    private Quaternion _camera_rot_iso;
    private Quaternion _camera_rot_norm;

    private float _camera_rot_iso_x;
    private float _camera_rot_iso_y;

    private float _camera_rot_norm_x;
    private float _camera_rot_norm_y;

    private Vector2 _last_mouse_pos;

    private bool _isometric_mode;
    #endregion

    void Start () {
        _camera_rot_iso = new Quaternion(0, 0, 0, 1);
        _camera_rot_norm = new Quaternion(0, 0, 0, 1);

        _player_camera = GetComponentInChildren<Camera>();

        _last_mouse_pos = Input.mousePosition;

        _isometric_mode = false;

        _camera_lerp = 1.0f;

        _camera_arm = 5.0f;

        _camera_arm_norm = DistanceNormalMax;
        _camera_arm_iso = DistanceScanMin;

        _camera_hgt = 10.0f;

        _camera_hgt_norm = FrustumHeightAtDistance(_player_camera, _camera_arm_norm);
        _camera_hgt_iso = FrustumHeightAtDistance(_player_camera, _camera_arm_iso);

        _camera_fov_iso = FOVForHeightAndDistance(_camera_hgt_iso, _camera_arm_iso);
        _camera_fov_norm = FOVForHeightAndDistance(_camera_hgt_norm, _camera_arm_norm);
    }
	
	void Update () {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mouseMove = mousePos - _last_mouse_pos;

        if (_isometric_mode)
        {
            if (_camera_lerp > 0.0f)
            {
                _camera_lerp -= 0.05f;
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    _camera_arm_iso = Mathf.Max(DistanceScanMin, _camera_arm_iso * 0.9f);
                    _camera_hgt_iso = FrustumHeightAtDistance(_player_camera, _camera_arm_iso);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    _camera_arm_iso = Mathf.Min(DistanceScanMax, _camera_arm_iso * 1.1f);
                    _camera_hgt_iso = FrustumHeightAtDistance(_player_camera, _camera_arm_iso);
                }
                if (Input.GetMouseButton(1))
                {
                    _camera_rot_iso_x += mouseMove.x;
                }
            }
        }
        else
        {
            if (_camera_lerp < 1.0f)
            {
                _camera_lerp += 0.05f;
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    _camera_arm_norm = Mathf.Max(DistanceNormalMin, _camera_arm_norm * 0.9f);
                    _camera_hgt_norm = FrustumHeightAtDistance(_player_camera, _camera_arm_norm);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    _camera_arm_norm = Mathf.Min(DistanceNormalMax, _camera_arm_norm * 1.1f);
                    _camera_hgt_norm = FrustumHeightAtDistance(_player_camera, _camera_arm_norm);
                }
                if (Input.GetMouseButton(1))
                {
                    _camera_rot_norm_x += mouseMove.x;
                    _camera_rot_norm_y -= mouseMove.y;
                }
            }
        }

        _camera_rot_iso = Quaternion.AngleAxis(_camera_rot_iso_x, Vector3.up) *
                          Quaternion.AngleAxis(45.0f, Vector3.right);
        _camera_rot_norm = Quaternion.AngleAxis(_camera_rot_norm_x, Vector3.up) *
                           Quaternion.AngleAxis(_camera_rot_norm_y, Vector3.right);

        _camera_arm = (_camera_arm_norm * _camera_lerp) + (_camera_arm_iso * (1.0f - _camera_lerp));
        _camera_hgt = (_camera_hgt_norm * _camera_lerp) + (_camera_hgt_iso * (1.0f - _camera_lerp));

        _player_camera.fieldOfView = FOVForHeightAndDistance(_camera_hgt, _camera_arm);

        _camera_rot = Quaternion.Lerp(_camera_rot_iso, _camera_rot_norm, _camera_lerp);
        _player_camera.transform.rotation = _camera_rot;
        _player_camera.transform.position = transform.position + _camera_rot * new Vector3(0, 0, -_camera_arm);

        _last_mouse_pos = mousePos;
    }
}
