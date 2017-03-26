using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerControl : MonoBehaviour {

    public Color ScannerColor = Color.blue;

    private float _last_known;
    private PlayerMove _mode_ctrl;
    private Material _controlled;

	// Use this for initialization
	void Start () {
        _mode_ctrl = FindObjectOfType<PlayerMove>();
        Renderer r = GetComponent<Renderer>();
        if (r == null || r.material == null || _mode_ctrl == null)
            Destroy(this);
        _controlled = r.material;
        _controlled.SetColor("_ModeColor", ScannerColor);
        _last_known = _mode_ctrl.GetMode();
    }
	
	// Update is called once per frame
	void Update () {
        float value = _mode_ctrl.GetMode();
        if (_last_known != value)
        {
            GetComponent<Renderer>().enabled = (value != 0);
            _controlled.SetFloat("_Mode", value);
            _last_known = value;
        }
    }
}
