using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class RotationOverTime : MonoBehaviour
    {
        public Vector3 Axis = Vector3.up;
        public float Speed;

        void Update()
        {
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(Speed * Time.deltaTime, Axis);
        }
    }
}