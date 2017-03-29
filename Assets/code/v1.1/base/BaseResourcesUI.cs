using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class BaseResourcesUI : MonoBehaviour {

        public UnityEngine.UI.Text Energy;
        public UnityEngine.UI.Text Ore;
        public UnityEngine.UI.Text Tech;

        private BaseResources _base_resources;
        private ResourceManager _player_resources;

        public void Reenergize()
        {
            _player_resources.Refuel(_base_resources);
        }
        public void OffloadOre()
        {
            _player_resources.OffloadOre(_base_resources);
        }
        public void OffloadTech()
        {
            List<TechPiece> techs = _player_resources.OffloadTech(_base_resources);
            foreach( TechPiece p in techs )
                _base_resources.AddTech(p);
        }

        // Use this for initialization
        void Start() {
            _player_resources = FindObjectOfType<ResourceManager>();
            _base_resources = FindObjectOfType<BaseResources>();
            if (_base_resources == null)
                Destroy(this);
        }

        // Update is called once per frame
        void Update() {
            Energy.text = Mathf.RoundToInt(_base_resources.EnergyStored()).ToString();
            Ore.text = Mathf.RoundToInt(_base_resources.OreStored()).ToString();
            Tech.text = _base_resources.TechStored().ToString();
        }
    }
}