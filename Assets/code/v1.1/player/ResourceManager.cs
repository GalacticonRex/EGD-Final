using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ResourceManager : MonoBehaviour
    {
        static private List<TechPiece> _tech_stored = new List<TechPiece>();
        static private float _energy_capacity = 100.0f;
        static private float _energy_stored = 60.0f;
        static private float _storage_capacity = 10000.0f;
        static private float _storage_ore = 0;
        static private ulong _current_year = 16182468175968148519;
        static private ulong _final_year = 18446744073709551615;

        static readonly int StoreOre = 0;
        static readonly int StoreTech = 1;

        public CapacityBar Energy;
        public CapacityBar Storage;
        public CapacityTextULong Year;

        public float HarvestDistance = 100.0f;
        public float ScanDistance = 100.0f;

        private float _current_subyear = 0.0f;

        public bool RequestTime(ulong years)
        {
            if (_final_year - _current_year > years)
            {
                return false;
            }
            else
            {
                _current_year += years;
                return true;
            }
        }

        public bool RequestEnergy(float amount)
        {
            bool good = Energy.CheckIfQuantity(amount);
            if (good)
            {
                Energy.Remove(amount);
                _energy_stored -= amount;
            }
            return good;
        }
        public bool RequestStorage(float amount)
        {
            bool good = Storage.CheckIfSpace(amount, StoreOre);
            if (good)
            {
                Storage.Add(amount, StoreOre);
                _storage_ore += amount;
            }
            return good;
        }
        public bool RequestStorage(TechPiece t)
        {
            float amount = t.GetTotalWeight();
            bool good = Storage.CheckIfSpace(amount, StoreTech);
            if (good)
            {
                Storage.Add(amount, StoreTech);
                _tech_stored.Add(t);
            }
            return good;
        }

        void Start()
        {
            Energy.maxCapacity = _energy_capacity;
            Storage.maxCapacity = _storage_capacity;
            Year.currentCapacity = _current_year;
            Year.maxCapacity = _final_year;

            Year.Add(_current_year);
            Energy.Add(_energy_stored);
            Storage.Add(_storage_ore);

            foreach (TechPiece t in _tech_stored)
            {
                Storage.Add(t.GetTotalWeight(), 1);
            }
        }
        private void Update()
        {
            _current_subyear += Time.deltaTime;
            while (_current_subyear > 1.0f)
            {
                _current_subyear -= 1.0f;
                _current_year++;
                Year.Add(1);
            }
        }
    }
}