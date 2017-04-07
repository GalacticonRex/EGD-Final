using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CapacityText : MonoBehaviour
    {
        public UnityEngine.UI.Text CurrentCapacity;
        public UnityEngine.UI.Text MaxCapacity;

        private float _max_capacity;
        private float _current_capacity;

        public float remaining
        {
            get
            {
                return _max_capacity - _current_capacity;
            }
        }
        public float currentCapacity
        {
            get
            {
                return _current_capacity;
            }
            set
            {
                _current_capacity = value;
                CurrentCapacity.text = Mathf.RoundToInt(_current_capacity).ToString();
                Debug.Log(gameObject.name + " Current: " + CurrentCapacity.text);
            }
        }
        public float maxCapacity
        {
            get
            {
                return _max_capacity;
            }
            set
            {
                _max_capacity = value;
                MaxCapacity.text = Mathf.RoundToInt(_max_capacity).ToString();
                Debug.Log(gameObject.name + " Max: " + MaxCapacity.text);
            }
        }

        public bool CheckIfSpace(float amount)
        {
            return (_current_capacity + amount <= _max_capacity);
        }
        public bool CheckIfQuantity(float amount)
        {
            return (_current_capacity >= amount);
        }
        public float Add(float amount)
        {
            float old_amount = _current_capacity;
            float new_amount = Mathf.Max(0.0f, Mathf.Min(_max_capacity, old_amount + amount));

            _current_capacity += new_amount - old_amount;

            return new_amount;
        }
        public float Remove(float amount)
        {
            return Add(-amount);
        }
    }
}