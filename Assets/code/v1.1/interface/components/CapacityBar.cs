using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityBar : MonoBehaviour {
    public UnityEngine.UI.Image[] Bars;
    public UnityEngine.UI.Text CurrentCapacity;
    public UnityEngine.UI.Text MaxCapacity;

    private float _max_capacity;
    private float[] _current_capacity;

    public float remaining
    {
        get
        {
            return _max_capacity - currentCapacity;
        }
    }
    public float currentCapacity
    {
        get
        {
            float total = 0;
            for (int i=0;i<_current_capacity.Length;i++ )
            {
                total += _current_capacity[i];
            }
            return total;
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
            update_capacity_bars();
        }
    }

    private void update_capacity_bars()
    {
        CurrentCapacity.text = Mathf.RoundToInt(currentCapacity).ToString();
        float bar_size = 0;
        int i = 0;
        foreach(UnityEngine.UI.Image img in Bars)
        {
            float shift_amount = 300.0f * _current_capacity[i] / _max_capacity;
            RectTransform r = img.GetComponent<RectTransform>();
            r.localPosition = new Vector3(-bar_size, 0);
            r.sizeDelta = new Vector2(shift_amount, r.sizeDelta.y);
            bar_size += shift_amount;
            i++;
        }
    }
    public bool CheckIfSpace(float amount, int type = 0)
    {
        float current_amount = currentCapacity;
        return (current_amount + amount <= _max_capacity);
    }
    public float Add(float amount, int type = 0)
    {
        float current_amount = currentCapacity;
        float old_amount = _current_capacity[type];
        float new_amount = Mathf.Max(0.0f, Mathf.Min(_max_capacity, current_amount + amount));

        _current_capacity[type] += new_amount - current_amount;

        update_capacity_bars();

        return _current_capacity[type] - old_amount;
    }
    public float Remove(float amount, int type = 0)
    {
        return Add(-amount, type);
    }
    private void Start()
    {
        if (Bars == null)
        {
            Destroy(this);
        }
        else
        {
            _current_capacity = new float[Bars.Length];
        }
    }
}
