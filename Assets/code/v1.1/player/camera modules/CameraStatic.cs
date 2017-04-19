using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CameraStatic : CameraView
    {
        public float Distance = 400.0f;
        public Vector2 Rotation = new Vector2(0, 0);
        public Vector2 RotationOverTime = new Vector2(0, 0);

        protected override void OnStart()
        {
            _actual_camera_distance = Distance;
            _target_camera_distance = Distance;
            _actual_rot_x = Rotation.x;
            _target_rot_x = Rotation.x;
            _actual_rot_y = Rotation.y;
            _target_rot_y = Rotation.y;
        }

        protected override void OnUpdate()
        {
            Rotation += RotationOverTime * Time.unscaledDeltaTime;
            _target_camera_distance = Distance;
            _target_rot_x = Rotation.x;
            _target_rot_y = Rotation.y;
            _actual_rotation = CalculateRotationFromAngles();
        }
    }
}