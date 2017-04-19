using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ModelImage : MonoBehaviour
    {
        public ModelViewer Source;
        private UnityEngine.UI.RawImage _img;

        public void SetSource(ModelViewer mod)
        {
            if (mod == null)
                _img.texture = null;
            else
                _img.texture = mod.Result;
        }

        void Start()
        {
            _img = GetComponent<UnityEngine.UI.RawImage>();
            SetSource(Source);
        }
    }
}