using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ModelImage : MonoBehaviour
    {
        public ModelViewer Source;
        private UnityEngine.UI.RawImage _img;
        private CameraSystem _camera_sys;

        public void ManipulateViewer()
        {
            if ( Input.GetMouseButton(1) )
            {
                Vector2 d = _camera_sys.mouseDelta;
                Source.Rotation = Quaternion.AngleAxis(-d.x, Vector3.up) * Quaternion.AngleAxis(d.y, Vector3.right) * Source.Rotation;
            }
        }

        public void SetSource(ModelViewer mod)
        {
            if (mod == null)
            {
                _img.enabled = false;
                _img.texture = null;
            }
            else
            {
                _img.enabled = true;
                _img.texture = mod.Result;
            }
        }

        void Awake()
        {
            _camera_sys = FindObjectOfType<CameraSystem>();
            _img = GetComponent<UnityEngine.UI.RawImage>();
            SetSource(Source);
        }
    }
}