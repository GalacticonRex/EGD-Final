using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CapacityTextULong : MonoBehaviour
    {
        public UnityEngine.UI.Text CurrentCapacity;
        public UnityEngine.UI.Text MaxCapacity;

        private ulong _max_capacity;
        private ulong _current_capacity;

        public ulong remaining
        {
            get
            {
                return _max_capacity - _current_capacity;
            }
        }
        public ulong currentCapacity
        {
            get
            {
                return _current_capacity;
            }
            set
            {
                _current_capacity = value;
                CurrentCapacity.text = _current_capacity.ToString();
            }
        }
        public ulong maxCapacity
        {
            get
            {
                return _max_capacity;
            }
            set
            {
                _max_capacity = value;
                MaxCapacity.text = _max_capacity.ToString();
            }
        }

        public bool CheckIfSpace(ulong amount)
        {
            return (_current_capacity + amount <= _max_capacity);
        }
        public bool CheckIfQuantity(ulong amount)
        {
            return (_current_capacity >= amount);
        }
        public ulong Add(ulong amount)
        {
            ulong old_amount = _current_capacity;
            ulong new_amount = System.Math.Max(0, System.Math.Min(_max_capacity, old_amount + amount));

            _current_capacity += new_amount - old_amount;
            CurrentCapacity.text = _current_capacity.ToString();

            return new_amount;
        }
        public ulong Remove(ulong amount)
        {
            ulong old_amount = _current_capacity;
            ulong new_amount = System.Math.Max(0, System.Math.Min(_max_capacity, old_amount - amount));

            _current_capacity -= new_amount - old_amount;
            CurrentCapacity.text = _current_capacity.ToString();

            return new_amount;
        }
    }

}