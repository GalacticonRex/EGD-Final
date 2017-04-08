using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class LightLerp : MonoBehaviour
    {
        public float TargetValue;
        private Light[] _affected;
        private float[] _original_value;
        private CameraSystem _camera;
        private float _last_recorded;

        // Use this for initialization
        void Start()
        {
            _camera = FindObjectOfType<CameraSystem>();
            List<Light> lights = new List<Light>();
            Light li = GetComponent<Light>();
            if (li != null)
                lights.Add(li);
            lights.AddRange(GetComponentsInChildren<Light>());
            _affected = lights.ToArray();
            _original_value = new float[_affected.Length];
            for (int i = 0; i < _original_value.Length; i++)
            {
                _original_value[i] = _affected[i].intensity;
            }
            _last_recorded = TargetValue;
        }

        // Update is called once per frame
        void Update()
        {
            if (_last_recorded != _camera.PlayerScanner.value)
            {
                _last_recorded = _camera.PlayerScanner.value;
                float distance = 1.0f - Mathf.Abs(TargetValue - _last_recorded);
                for (int i = 0; i < _affected.Length; i++)
                {
                    _affected[i].intensity = _original_value[i] * distance;
                }
            }
        }
    }
}