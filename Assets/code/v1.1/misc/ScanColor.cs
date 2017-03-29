using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ScanColor : MonoBehaviour
    {
        public Color ScannerColor = Color.blue;
        public Material ScannerMaterial;

        private float _last_known;
        private Player _mode_ctrl;

        // Use this for initialization
        void Start()
        {
            _mode_ctrl = FindObjectOfType<Player>();

            if (_mode_ctrl == null)
                Destroy(this);
        }

        // Update is called once per frame
        void Update()
        {
            float value = _mode_ctrl.cameraSystem.interpolation;
            if (_last_known != value)
            {
                ScannerMaterial.SetFloat("_Mode", value);
                _last_known = value;
            }
        }
    }
}