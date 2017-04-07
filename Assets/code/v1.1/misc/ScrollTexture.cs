using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ScrollTexture : MonoBehaviour
    {
        public Vector2[] Rate;
        public string[] TextureNames;

        private Renderer _source;
        private Vector2[] _total;

        private void Start()
        {
            _source = GetComponent<Renderer>();
            _total = new Vector2[Rate.Length];
        }
        private void Update()
        {
            for (int i = 0; i < TextureNames.Length; i++)
            {
                _source.material.SetTextureOffset(TextureNames[i], _total[i]);
                _total[i] += Rate[i] * Time.deltaTime;
            }
        }
    }
}