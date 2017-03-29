using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class RotationOverTime : MonoBehaviour
    {
        public float Speed;
        void Update()
        {
            transform.rotation = transform.rotation * Quaternion.AngleAxis(Speed * Time.deltaTime, Vector3.up);
        }
    }
}