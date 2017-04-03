using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class DroneAI : MonoBehaviour {

        public DroneTask Task;
        public bool ReturnToShip;
        public float MaxWeight = 1000.0f;
        public float MaxVelocity = 1.0f;
        public float MaxAcceleration = 2.0f;

        private bool _arrived;
        private float _acceleration;
        private float _velocity;

        private float _ore_stored;
        private TechPiece _tech_stored;
        private DroneManager _manager;
        
        private Quaternion _local_init_rotation;
        private float _local_rotation_lerp;

        public float GetOre()
        {
            return _ore_stored;
        }
        public TechPiece GetTech()
        {
            return _tech_stored;
        }

        private void Start()
        {
            _manager = FindObjectOfType<DroneManager>();
            _ore_stored = 0;
            _tech_stored = null;
        }

        private void Update() {
            if ( ReturnToShip )
            {
                _acceleration = MaxAcceleration;
                _velocity = Mathf.Max(0.0f, Mathf.Min(MaxVelocity, _velocity + _acceleration * Time.deltaTime));

                Vector3 target = _manager.transform.position;
                Vector3 distance = target - transform.position;

                if (distance.magnitude <= _velocity * Time.deltaTime)
                {
                    _manager.ReturnToShip(this);
                }
                else
                {
                    Vector3 direction = distance.normalized;
                    transform.rotation = Quaternion.LookRotation(direction);
                    transform.position += direction * _velocity * Time.deltaTime;
                }
            }
            else if (_arrived)
            {
                float ang;
                Vector3 axis;
                transform.rotation.ToAngleAxis(out ang, out axis);

                Quaternion target = Quaternion.AngleAxis(ang, Task.WorkAxis);

                if (_local_rotation_lerp < 1.0f)
                {
                    transform.rotation = Quaternion.Lerp(_local_init_rotation, target, _local_rotation_lerp);
                    _local_rotation_lerp += 2.0f * Time.deltaTime;
                }
                else
                {
                    transform.rotation = target;
                    Task.WorkRemaining -= Time.deltaTime;

                    if (Task.DuringTask != null)
                        Task.DuringTask.Invoke(this);

                    if (Task.WorkRemaining <= 0)
                    {
                        if (Task.OnTaskComplete != null)
                            Task.OnTaskComplete.Invoke(this);
                        ReturnToShip = true;
                        Task = null;
                    }
                }
            }
            else
            {
                _acceleration = MaxAcceleration;
                _velocity = Mathf.Max(0.0f, Mathf.Min(MaxVelocity, _velocity + _acceleration * Time.deltaTime));

                Vector3 target = Task.transform.position + Task.WorkPosition;
                Vector3 distance = target - transform.position;

                if (distance.magnitude <= _velocity * Time.deltaTime)
                {
                    transform.position = Task.transform.position + Task.WorkPosition;
                    _local_init_rotation = transform.rotation;
                    _arrived = true;
                    _local_rotation_lerp = 0.0f;
                }
                else
                {
                    Vector3 direction = distance.normalized;
                    transform.rotation = Quaternion.LookRotation(direction);
                    transform.position += direction * _velocity * Time.deltaTime;
                }
            }
        }
    }
}