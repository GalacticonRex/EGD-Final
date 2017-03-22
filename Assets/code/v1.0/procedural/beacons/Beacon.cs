using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour {

    public string Text = "";
    public Color BandColor = Color.green;
    public float BandRadius = 1.0f;
    public float BandSpeed = 1.0f;
    public float Minimum = 0.0f;
    public float Maximum = 0.0f;

    private PlayerMove _player;
    private TextInterface _display;
    private Renderer _renderer;
    private Collider _collider;
    private float _time_count;
    private bool _selected;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _player = FindObjectOfType<PlayerMove>();
        _display = FindObjectOfType<TextInterface>();
        if (_renderer == null || _collider == null)
            Destroy(this);
        _renderer.material.SetColor("_BandColor", BandColor);
        _renderer.material.SetFloat("_BandRadius", 0);

        _selected = false;
    }
	
	// Update is called once per frame
	void Update () {
        bool selected = _selected;

        if (Vector3.Distance(transform.position, _player.transform.position) < _player.ScanDistance)
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            selected = (Physics.Raycast(r, out hit, _player.ScanDistance) && hit.collider == _collider);
            if (selected)
            {
                _player.Select(this);
                if (Input.GetMouseButton(0))
                    _display.Show(Text);
            }

            if (selected != _selected)
            {
                _selected = selected;
                _renderer.material.SetFloat("_BandRadius", (_selected) ? BandRadius : 0);
            }
        }

        Vector3 v = transform.position;

        _renderer.material.SetVector("_OriginPoint", new Vector4(v.x, v.y, v.z));
        _renderer.material.SetFloat("_BandOffset", ((Mathf.Sin(_time_count)+1)/2) * (Maximum - Minimum) + Minimum);

        _time_count += Time.deltaTime * BandSpeed;
        while (_time_count > Mathf.PI * 2.0f)
            _time_count -= Mathf.PI * 2.0f;

    }
}
