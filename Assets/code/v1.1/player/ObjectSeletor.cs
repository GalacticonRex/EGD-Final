using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSeletor : MonoBehaviour {

    public float MaxDistance = 100.0f;
    public GameObject Scanner;

    private CameraSystem _camera;
    private Selectable _currently_hovered;
    private Selectable _currently_selected;
    private Selectable _last_selected;

    public bool hovering
    {
        get { return (_currently_hovered != null); }
    }
    public bool selected
    {
        get { return (_currently_selected != null); }
    }
    public Selectable hoveringItem
    {
        get { return _currently_hovered; }
    }
    public Selectable selectedItem
    {
        get { return _currently_selected; }
    }
    public Selectable lastSelectedItem
    {
        get { return _last_selected; }
    }

    void Start () {
        _camera = GetComponent<CameraSystem>();
    }
	
	void Update () {
        Scanner.transform.localScale = new Vector3(2 * MaxDistance, 0.1f, 2 * MaxDistance);
        Color color = Scanner.GetComponent<Renderer>().material.color;
        color.a = 0.1f * (1.0f - _camera.interpolation);
        Scanner.GetComponent<Renderer>().enabled = (_camera.interpolation != 1);
        Scanner.GetComponent<Renderer>().material.color = color;

        _currently_hovered = null;
        if (_camera.interpolation > 0.9f)
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit, MaxDistance))
            {
                _currently_hovered = hit.collider.GetComponent<Selectable>();
                if ( Input.GetMouseButtonDown(0) )
                {
                    _currently_selected = _currently_hovered;
                }
            }
        }
        if (!Input.GetMouseButton(0) && _currently_selected != null)
        {
            _last_selected = _currently_selected;
            _currently_selected = null;
        }
    }
}
