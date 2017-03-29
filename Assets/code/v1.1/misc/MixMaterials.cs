using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class MixMaterials : MonoBehaviour
    {
        public Material ScannerMaterial;

        private Player _player;
        private Renderer _renderer;
        private Material _source;

        // Use this for initialization
        void Start()
        {
            _player = FindObjectOfType<Player>();
            _renderer = GetComponent<Renderer>();
            _source = _renderer.sharedMaterial;
        }

        // Update is called once per frame
        void Update()
        {
            _renderer.material.Lerp(
                    ScannerMaterial,
                    _source,
                    _player.cameraSystem.interpolation
                    );

        }
    }
}