using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class BaseResources : MonoBehaviour {
        static private float _last_year_checked = 0.0f;

        static private List<TechPiece> _tech_stored = new List<TechPiece>();
        static private float _ore_stored = 1000;
        static private float _energy_stored = 1000;

        static private float _energy_generation = 0.4f;
        static private float _ore_generation = 0.0f;

        private ResourceManager _resources;

        public int TechStored()
        {
            return _tech_stored.Count;
        }
        public float OreStored()
        {
            return _ore_stored;
        }
        public float EnergyStored()
        {
            return _energy_stored;
        }

        public void AddTech(TechPiece t)
        {
            _tech_stored.Add(t);
        }
        public void AddEnergy(float en)
        {
            _energy_stored += en;
        }
        public void AddOre(float ore)
        {
            _ore_stored += ore;
        }

        void Start() {
            _resources = FindObjectOfType<ResourceManager>();

            float current_year = _resources.CurrentYear();
            float delta = current_year - _last_year_checked;

            _energy_stored += _energy_generation * delta;
            _ore_stored += _ore_generation * delta;

            _last_year_checked = current_year;
        }
        void Update() {
            _energy_stored += _energy_generation * Time.deltaTime;
            _ore_stored += _ore_generation * Time.deltaTime;
            _last_year_checked = _resources.CurrentYear();
        }
    }
}