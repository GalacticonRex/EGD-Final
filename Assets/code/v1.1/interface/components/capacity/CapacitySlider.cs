using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacitySlider : MonoBehaviour {

    public Image[] Elements;
    private float[] _amounts;
    private float _maximum;

    public float allocated
    {
        get
        {
            float total = 0.0f;
            foreach (float f in _amounts)
                total += f;
            return total;
        }
    }
    public float remaining
    {
        get { return _maximum - allocated; }
    }

    public void SetAmount(int type, float value)
    {
        _amounts[type] = value;
    }
    public void SetMaximum(float value)
    {
        _maximum = value;
    }
    public float Amount(int type)
    {
        return _amounts[type];
    }
    public void Add(int type, float value)
    {
        float current = allocated;
        float result = Mathf.Min(current + value, _maximum);
        _amounts[type] += result - current;
    }
    public void Remove(int type, float value)
    {
        _amounts[type] = Mathf.Max(_amounts[type] - value, 0);
    }

    private void Awake()
    {
        _amounts = new float[Elements.Length];
        _maximum = 1.0f;
        foreach (Image img in Elements)
            img.fillAmount = 0;
    }
    private void Update()
    {
        float running = 0.0f;
        for (int i=0;i<Elements.Length;i++)
        {
            float increase = _amounts[i] / _maximum;
            running += increase;
            Elements[i].fillAmount = running;
        }
    }
}
