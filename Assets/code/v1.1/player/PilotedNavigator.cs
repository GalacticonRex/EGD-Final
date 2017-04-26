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

        public float[] MaxAcceleration;
        public float[] MaxVelocity;
        public float[] EnergyCost;

        public float YawPitchAnglePerSecond = 16.0f;
        public float RollAnglePerSecond = 16.0f;

        private InterfaceMenu _menus;
        private CameraSystem _camera;
        private ResourceManager _resources;
        private Vector2 _rooted_position;
        private float _acceleration;
        private float _current_velocity;
        private float _target_velocity;

        private int _gear;
        private float _time_between;
        private Coroutine _waiting_on_press;
        private Rigidbody _self;
        private HashSet<object> _blockers = new HashSet<object>();

        public void Block(object obj)
        {
            _blockers.Add(obj);
        }
        public void Unblock(object obj)
        {
            _blockers.Remove(obj);
        }

        private IEnumerator GetTimesPressed()
        {
            yield return new WaitForSeconds(_time_between);
            _waiting_on_press = null;
        }

        private void Start()
        {
            _menus = FindObjectOfType<InterfaceMenu>();
            _camera = FindObjectOfType<CameraSystem>();
            _resources = FindObjectOfType<ResourceManager>();

            _self = GetComponent<Rigidbody>();

            _gear = 0;
            _time_between = 0.2f;

            _acceleration = 0.0f;
            _current_velocity = 0.0f;
            _target_velocity = 0.0f;
        }

        private void Update()
        {
            if (_blockers.Count > 0)
                return;

            if (Input.GetKey(RollLeft))
            {
                Quaternion roll = Quaternion.AngleAxis(RollAnglePerSecond * Time.deltaTime, Rotated.forward);
                _self.MoveRotation(roll * Rotated.rotation);
            }
            if (Input.GetKey(RollRight))
            {
                Quaternion roll = Quaternion.AngleAxis(-RollAnglePerSecond * Time.deltaTime, Rotated.forward);
                _self.MoveRotation(roll * Rotated.rotation);
            }
            if (Input.GetMouseButtonDown(1))
                _rooted_position = Input.mousePosition;

            if (Input.GetMouseButton(1))
            {
                Vector2 delta = new Vector2(Input.mousePosition.x - _rooted_position.x, _rooted_position.y - Input.mousePosition.y);

                Vector3 offset = Rotated.up * delta.x + Rotated.right * delta.y;
                Vector3 cross = Vector3.Cross(Rotated.up, offset.normalized);

                _self.MoveRotation(Quaternion.AngleAxis(offset.magnitude / YawPitchAnglePerSecond * Time.deltaTime, offset.normalized) * Rotated.rotation);
            }

            if (Input.GetKeyDown(Accelerate))
            {
                if (_waiting_on_press == null)
                    _waiting_on_press = StartCoroutine(GetTimesPressed());
                else
                {
                    _gear++;
                    StopCoroutine(_waiting_on_press);
                    _waiting_on_press = null;
                }
            }
            else if ( !Input.GetKey(Accelerate) && _waiting_on_press == null )
            {
                _gear = 0;
            }

            if (Input.GetKey(Accelerate) && _resources.RequestEnergy(EnergyCost[_gear] * Time.deltaTime))
            {
                _acceleration += MaxAcceleration[_gear] * Time.deltaTime;
                _target_velocity = MaxVelocity[_gear];
            }
            else
            {
                _target_velocity = 0.0f;
                if (Input.GetKey(Stop))
                {
                    _acceleration = 0.0f;
                    _current_velocity = 0.0f;
                }
                else
                    _acceleration = MaxVelocity[_gear] * MaxVelocity[_gear] * Time.deltaTime;
            }

            
            if (_current_velocity > _target_velocity)
            {
                _current_velocity -= _acceleration;
                if (_current_velocity <= _target_velocity)
                {
                    _current_velocity = _target_velocity;
                }
            }
            else
            {
                _current_velocity += _acceleration;
                if (_current_velocity >= _target_velocity)
                {
                    _current_velocity = _target_velocity;
                }
            }

            _self.MovePosition(Moved.position + Rotated.forward * _current_velocity * Time.deltaTime);
            _camera.additionalFOV = _current_velocity / 60.0f;
        }
    }
}