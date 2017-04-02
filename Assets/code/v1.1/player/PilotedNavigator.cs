using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class PilotedNavigator : MonoBehaviour
    {
        public Transform Moved;
        public Transform Rotated;
        public KeyCode Accelerate = KeyCode.W;
        public KeyCode Stop = KeyCode.S;
        public KeyCode RollLeft = KeyCode.A;
        public KeyCode RollRight = KeyCode.D;
        public float MaxAcceleration = 0.05f;
        public float MaxVelocity = 8.0f;

        public float YawPitchAnglePerSecond = 16.0f;
        public float RollAnglePerSecond = 16.0f;

        private CameraSystem _camera;
        private Vector2 _rooted_position;
        private float _acceleration;
        private float _velocity;

        private void Start()
        {
            _camera = FindObjectOfType<CameraSystem>();
        }

        private void Update()
        {
            if (Input.GetKey(Accelerate))
                _acceleration = MaxAcceleration * Time.deltaTime;
            else if (Input.GetKey(Stop))
                _acceleration = -MaxVelocity;
            else
                _acceleration = -MaxVelocity * Time.deltaTime * 2.0f;

            if (Input.GetKey(RollLeft))
            {
                Quaternion roll = Quaternion.AngleAxis(RollAnglePerSecond * Time.deltaTime, Rotated.forward);
                Rotated.rotation = roll * Rotated.rotation;
            }
            if (Input.GetKey(RollRight))
            {
                Quaternion roll = Quaternion.AngleAxis(-RollAnglePerSecond * Time.deltaTime, Rotated.forward);
                Rotated.rotation = roll * Rotated.rotation;
            }
            if (Input.GetMouseButtonDown(1))
                _rooted_position = Input.mousePosition;

            if ( Input.GetMouseButton(1) )
            {
                Vector2 delta = new Vector2(Input.mousePosition.x - _rooted_position.x, _rooted_position.y - Input.mousePosition.y);

                Vector3 offset = Rotated.up * delta.x + Rotated.right * delta.y;
                Vector3 cross = Vector3.Cross(Rotated.up, offset.normalized);

                Rotated.rotation = Quaternion.AngleAxis(offset.magnitude / YawPitchAnglePerSecond * Time.deltaTime, offset.normalized) * Rotated.rotation;
            }

            _velocity = Mathf.Min(MaxVelocity, Mathf.Max(0.0f, _velocity + _acceleration * Time.deltaTime));

            Moved.position += Rotated.forward * _velocity;

        }
    }
}