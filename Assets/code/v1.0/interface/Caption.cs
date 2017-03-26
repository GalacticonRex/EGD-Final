using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caption : MonoBehaviour {
    public GameObject Source;
    public string TextData;
    public float Radius;

    private PlayerMove _player;
    private GameObject _caption;
    private UnityEngine.UI.Image _panel;
    private UnityEngine.UI.Text _text;

    public string Text {
        get
        {
            return TextData;
        }
        set
        {
            TextData = value;
            _text.text = TextData;
        }
    }


    private void Start () {
        _player = FindObjectOfType<PlayerMove>();
        _caption = Instantiate(Source);
        _caption.transform.SetParent(transform);

        _panel = _caption.GetComponentInChildren<UnityEngine.UI.Image>();
        _text = _caption.GetComponentInChildren<UnityEngine.UI.Text>();

        _text.text = TextData;
    }
    private void Update()
    {
        if (_caption == null)
            Destroy(this);

        float distance = Vector3.Distance(_player.transform.position, transform.position);
        if ( distance < _player.ScanDistance )
        {
            float alpha = (_player.ScanDistance - distance) / _player.ScanDistance;
            _caption.SetActive(true);

            Vector3 dir = transform.position - Camera.main.transform.position;
            _caption.transform.position = transform.position - dir.normalized * Radius;
            _caption.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            _caption.transform.localScale = new Vector3(dir.magnitude / 10000.0f, dir.magnitude / 10000.0f, 1.0f);

            Color temp;

            temp = _panel.color;
            temp.a = 0.5f * _player.GetMode() * alpha;
            _panel.color = temp;

            temp = _text.color;
            temp.a = _player.GetMode() * alpha;
            _text.color = temp;
        }
        else
        {
            _caption.SetActive(false);
        }
    }
}
