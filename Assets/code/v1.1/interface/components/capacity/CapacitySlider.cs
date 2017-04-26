using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacitySlider : MonoBehaviour {

    public Image[] Elements;
    private float[] _amounts;
    private float[] _maximums;

    public void SetAmount(int type, float value)
    {
        _amounts[type] = value;
    }
    public void SetMaximum(int type, float value)
    {
        _maximums[type] = value;
    }

    private void Start()
    {
        _amounts = new float[Elements.Length];
        _maximums = new float[Elements.Length];
        foreach (Image img in Elements)
            img.fillAmount = 0;
    }
    private void Update()
    {
        float running = 0.0f;
        for (int i=0;i<Elements.Length;i++)
        {
            float increase = _amounts[i] / _maximums[i];
            running += increase;
            Elements[i].fillAmount = running;
        }
    }
}
