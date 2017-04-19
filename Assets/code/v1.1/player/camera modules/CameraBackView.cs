using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CameraBackView : CameraView
    {
        public float MinDistance = 10.0f;
        public float MaxDistance = 100.0f;
        public Vector2 InitialRotation = new Vector2(0, 0);

        private Player _player;
        private Quaternion _local_rotation;
        private Quaternion _target_rotation;

        public float ratio
        {
            get
            {
                return (_actual_camera_distance + MinDistance) / (MaxDistance + MinDistance);
            }
        }

        protected override void OnStart()
        {
            _player = FindObjectOfType<Player>();
            _actual_camera_distance = MinDistance;
            _target_camera_distance = MinDistance;
            _actual_rot_x = InitialRotation.x;
            _target_rot_x = InitialRotation.x;
            _actual_rot_y = InitialRotation.y;
            _target_rot_y = InitialRotation.y;
            _local_rotation = CalculateRotationFromAngles();
        }
        protected override void OnUpdate()
        {
            Vector2 mouseMove = _camera_system.mouseDelta;
            float axis = Input.GetAxis("Mouse ScrollWheel");
            float adjusted = Mathf.Pow(axis, 1.3f);
            if (axis > 0)
            {
                _target_camera_distance = Mathf.Max(MinDistance, _target_camera_distance * 0.8f);
            }
            else if (axis < 0)
            {
                _target_camera_distance = Mathf.Min(MaxDistance, _target_camera_distance * 1.3f);
            }
            _target_rotation = transform.rotation * _local_rotation;
            _actual_rotation = Quaternion.Lerp(_actual_rotation, _target_rotation, AdjustRate);
        }
    }
}