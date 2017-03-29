using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class RandomLocation : MonoBehaviour
    {
        public float MinRadius = 0.0f;
        public float MaxRadius = 1000.0f;

        public float MinPositiveY = 50.0f;
        public float MaxPositiveY = 150.0f;

        public float MinNegativeY = 50.0f;
        public float MaxNegativeY = 150.0f;

        private void Start()
        {
            float radius = Random.Range(MinRadius, MaxRadius);
            float angle = Random.value * Mathf.PI * 2.0f;
            float z;
            if (Random.value < 0.5f)
            {
                z = Random.Range(MinPositiveY, MaxPositiveY);
            }
            else
            {
                z = -Random.Range(MinNegativeY, MaxNegativeY);
            }
            Vector3 pos = new Vector3(radius * Mathf.Cos(angle), z, radius * Mathf.Sin(angle));
            transform.position = pos;
        }
    }
}