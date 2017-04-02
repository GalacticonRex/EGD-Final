using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CameraScanner : CameraView
    {
        public float MinDistance = 400.0f;
        public float MaxDistance = 4000.0f;
        public Vector2 InitialRotation = new Vector2(0, 0);

        public float ratio
        {
            get
            {
                return (_actual_camera_distance + MinDistance) / (MaxDistance + MinDistance);
            }
        }

        protected override void OnStart()
        {
            _actual_camera_distance = MinDistance;
            _target_camera_distance = MinDistance;
            _actual_rot_x = InitialRotation.x;
            _target_rot_x = InitialRotation.x;
            _actual_rot_y = InitialRotation.y;
            _target_rot_y = InitialRotation.y;
        }

        protected override void OnUpdate()
        {
            Vector2 mouseMove = _camera_system.mouseDelta;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                _target_camera_distance = Mathf.Max(MinDistance, _target_camera_distance * 0.9f);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                _target_camera_distance = Mathf.Min(MaxDistance, _target_camera_distance * 1.1f);
            }
            if (Input.GetMouseButton(1))
            {
                _target_rot_x += mouseMove.x;
                _target_rot_y -= mouseMove.y;
            }
            _actual_rotation = CalculateRotaionFromAngles();
        }
    }
}