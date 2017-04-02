using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public abstract class CameraView : MonoBehaviour {
        public float AdjustRate = 0.5f;

        protected bool _active = true;

        protected CameraSystem _camera_system;

        protected float _target_camera_distance;
        protected float _target_rot_x;
        protected float _target_rot_y;

        protected float _actual_camera_distance;
        protected float _actual_fov;
        protected float _actual_rot_x;
        protected float _actual_rot_y;

        protected Quaternion _actual_rotation = new Quaternion(0,0,0,1);

        public bool active
        {
            get { return _active; }
            set { _active = value; }
        }
        public float fieldOfView
        {
            get { return _actual_fov; }
        }
        public float cameraDistance
        {
            get { return _actual_camera_distance; }
            set { _target_camera_distance = value; }
        }
        public float rotationX
        {
            get { return _actual_rot_x; }
            set { _target_rot_x = value; }
        }
        public float rotationY
        {
            get { return _actual_rot_y; }
            set { _target_rot_y = value; }
        }
        public Quaternion rotation
        {
            get { return _actual_rotation; }
        }

        protected Quaternion CalculateRotaionFromAngles()
        {
            return Quaternion.AngleAxis(_actual_rot_x, Vector3.up) *
                    Quaternion.AngleAxis(_actual_rot_y, Vector3.right);
        }
        protected float CalculateFOVFromDistance()
        {
            float t_hgt = CameraSystem.FrustumHeightAtDistance(Camera.main, _actual_camera_distance);
            return CameraSystem.FOVForHeightAndDistance(t_hgt, _actual_camera_distance);
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate();

        protected void Start()
        {
            _camera_system = GetComponentInParent<CameraSystem>();
            OnStart();
            _actual_fov = CalculateFOVFromDistance();
        }
        protected void Update()
        {
            if ( _active )
                OnUpdate();

            _actual_camera_distance = (1-AdjustRate) * _actual_camera_distance + AdjustRate * _target_camera_distance;

            _actual_rot_x = (1-AdjustRate) * _actual_rot_x + AdjustRate * _target_rot_x;
            _actual_rot_y = (1-AdjustRate) * _actual_rot_y + AdjustRate * _target_rot_y;

            _actual_fov = CalculateFOVFromDistance();
        }

    }
}