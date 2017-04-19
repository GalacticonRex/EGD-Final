using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxScreen : MonoBehaviour {

    public UnityEngine.UI.Image Top;
    public UnityEngine.UI.Image Bottom;
    private float _amount = 0.0f;
    private float _direction = 0.0f;

    public void Show()
    {
        _direction = 1.0f;
    }
    public void Hide()
    {
        _direction = -1.0f;
    }
    public void HideNow()
    {
        _direction = 0.0f;
        _amount = 0.0f;
        Update();
    }
    private void Update () {
        _amount = Mathf.Min(1.0f, Mathf.Max(0.0f, _amount + _direction * Time.unscaledDeltaTime));
        {
            RectTransform r = Bottom.GetComponent<RectTransform>();
            Vector3 scale = r.localScale;
            scale.y = _amount;
            r.localScale = scale;
        }
        {
            RectTransform r = Top.GetComponent<RectTransform>();
            Vector3 scale = r.localScale;
            scale.y = _amount;
            r.localScale = scale;
        }
    }
}
