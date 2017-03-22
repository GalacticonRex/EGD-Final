using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    // Public Members
    public SmoothNavigation Navigator;
    public float ScanDistance = 100.0f;
    public float HarvestDistance = 60.0f;
    public GameObject Scanner;
    public LocationSelector Selector;
    public LocationSelector Destination;

    public CapacityBar Energy;
    public CapacityBar Storage;

    public void Select(MonoBehaviour mb)
    {
        _currently_selected = mb;
    }

    public float GetArmRatio()
    {
        float norm = (_camera_arm + _camera_dist_min_norm) / (_camera_dist_min_norm + _camera_dist_max_norm);
        float iso = (_camera_arm + _camera_dist_min_iso) / (_camera_dist_min_iso + _camera_dist_max_iso);
        return _camera_lerp * norm + (1.0f - _camera_lerp) * iso;
    }
    public float GetMode()
    {
        return _camera_lerp;
    }
    public void Arrived()
    {
        Destination.Hide();
        _has_destination = false;
    }
    public void SetDestination(Vector3 dst)
    {
        Destination.SetLocation(dst);
        _has_destination = true;
    }
    public Vector3 GetDestination()
    {
        return (_has_destination)?Destination.GetLocation():transform.position;
    }
    public bool HasDestination()
    {
        return _has_destination;
    }
    public float AddOre(float x)
    {
        _storage_ore += x;
        return Storage.Add(x);
    }
    public bool AttemptStoreTech(TechPiece t)
    {
        float w = t.GetTotalWeight();
        if ( Storage.CheckIfSpace(w, 1) )
        {
            _tech_stored.Add(t);
            Storage.Add(w, 1);
            return true;
        }
        return false;
    }

    static private List<TechPiece> _tech_stored = new List<TechPiece>();
    static private float _energy_capacity = 1000.0f;
    static private float _energy_stored = 1000.0f;
    static private float _storage_capacity = 1000.0f;
    static private float _storage_ore = 0;

    // Private Members
    private float _camera_dist_min_iso = 200.0f;
    private float _camera_dist_max_iso = 10000.0f;
    private float _camera_dist_min_norm = 10.0f;
    private float _camera_dist_max_norm = 80.0f;

    private bool _has_destination;

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

    private MonoBehaviour _currently_selected = null;
    private bool _navigation_initiated_in_normal_view = false;

    private float FrustumHeightAtDistance(float distance)
    {
        return 2.0f * distance * Mathf.Tan(_player_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    private float FOVForHeightAndDistance(float height, float distance)
    {
        return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
    }

    // Use this for initialization
    private void Start() {
        Energy.maxCapacity = _energy_capacity;
        Storage.maxCapacity = _storage_capacity;

        Energy.Add(_energy_stored);
        Storage.Add(_storage_ore);
        foreach( TechPiece t in _tech_stored )
        {
            Storage.Add(t.GetTotalWeight(), 1);
        }

        _has_destination = false;

        _camera_rot_iso = new Quaternion(0, 0, 0, 1);
        _camera_rot_norm = new Quaternion(0, 0, 0, 1);

        _player_camera = GetComponentInChildren<Camera>();

        _last_mouse_pos = Input.mousePosition;

        _isometric_mode = false;

        _camera_lerp = 1.0f;

        _camera_arm = 5.0f;

        _camera_arm_norm = _camera_dist_min_norm;
        _camera_arm_iso = _camera_dist_min_iso;

        _camera_hgt = 10.0f;

        _camera_hgt_norm = FrustumHeightAtDistance(_camera_arm_norm);
        _camera_hgt_iso = FrustumHeightAtDistance(_camera_arm_iso);

        _camera_fov_iso = FOVForHeightAndDistance(_camera_hgt_iso, _camera_arm_iso);
        _camera_fov_norm = FOVForHeightAndDistance(_camera_hgt_norm, _camera_arm_norm);
    }

    // Update is called once per frame
    private void Update() {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mouseMove = mousePos - _last_mouse_pos;

        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            _isometric_mode = !_isometric_mode;
            Selector.SetVisibility(_isometric_mode);
            Destination.SetVisibility(_isometric_mode && _has_destination);
        }

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
                    _camera_arm_iso = Mathf.Max(_camera_dist_min_iso, _camera_arm_iso * 0.9f);
                    _camera_hgt_iso = FrustumHeightAtDistance(_camera_arm_iso);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    _camera_arm_iso = Mathf.Min(_camera_dist_max_iso, _camera_arm_iso * 1.1f);
                    _camera_hgt_iso = FrustumHeightAtDistance(_camera_arm_iso);
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
                    _camera_arm_norm = Mathf.Max(_camera_dist_min_norm, _camera_arm_norm * 0.9f);
                    _camera_hgt_norm = FrustumHeightAtDistance(_camera_arm_norm);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    _camera_arm_norm = Mathf.Min(_camera_dist_max_norm, _camera_arm_norm * 1.1f);
                    _camera_hgt_norm = FrustumHeightAtDistance(_camera_arm_norm);
                }
                if (Input.GetMouseButton(1))
                {
                    _camera_rot_norm_x += mouseMove.x;
                    _camera_rot_norm_y -= mouseMove.y;
                }
            }
        }

        Scanner.transform.localScale = new Vector3(2*ScanDistance, 0.1f, 2 * ScanDistance);
        Color color = Scanner.GetComponent<Renderer>().material.color;
        color.a = 0.1f * (1.0f - _camera_lerp);
        Scanner.GetComponent<Renderer>().enabled = (_camera_lerp != 1);
        Scanner.GetComponent<Renderer>().material.color = color;

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

        if (Navigator.IsMoving())
        {
            _energy_stored += Energy.Remove(1.0f * Time.deltaTime);
        }
        else
        {
            _energy_stored += Energy.Remove(0.1f * Time.deltaTime);
        }

        Selector.FindLocation();

        if (_currently_selected == null)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 nloc = Selector.GetLocation();
                if (Vector3.Distance(transform.position, nloc) > 3.0f)
                {
                    if (_isometric_mode)
                        Destination.Show();
                    else
                        _navigation_initiated_in_normal_view = true;
                    SetDestination(nloc);
                }
            }
            else if (_navigation_initiated_in_normal_view)
            {
                _navigation_initiated_in_normal_view = false;
                Arrived();
            }
        }
        if ( Input.GetMouseButtonUp(0) )    
            _currently_selected = null;
    }
}
