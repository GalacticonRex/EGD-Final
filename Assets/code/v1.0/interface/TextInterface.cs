using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInterface : MonoBehaviour {

    public UnityEngine.UI.MaskableGraphic Display;
    public UnityEngine.UI.Text TextElement;
    private bool _showing;

    public void Show(string txt)
    {
        _showing = true;
        Display.enabled = true;
        TextElement.enabled = true;
        TextElement.text = txt;
    }
    public void Hide()
    {
        _showing = false;
        Display.enabled = false;
        TextElement.enabled = false;
    }
    private void Start()
    {
        Hide();
    }
}
