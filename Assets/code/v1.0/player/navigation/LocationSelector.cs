using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSelector : MonoBehaviour {

    public PlayerMove Source;
    public Transform Line;

    private Plane _base;
    private PlayerMove _controller;
    private List<Renderer> _renderers;

    public void SetVisibility(bool x)
    {
        foreach(Renderer r in _renderers)
        {
            r.enabled = x;
        }
    }
    public void Show()
    {
        SetVisibility(true);
    }
    public void Hide()
    {
        SetVisibility(false);
    }
    public void FindLocation()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        _base.Raycast(r, out dist);
        transform.position = r.GetPoint(dist);
    }
    public void SetLocation(Vector3 pos)
    {
        transform.position = pos;
    }
    public Vector3 GetLocation()
    {
        return transform.position;
    }

	// Use this for initialization
	private void Start () {
        _base = new Plane(Vector3.up, 0.0f);
        _controller = FindObjectOfType<PlayerMove>();

        Renderer[] r = GetComponentsInChildren<Renderer>();

        _renderers = new List<Renderer>();
        _renderers.AddRange(r);

        Hide();
    }
    private void Update()
    {
        Vector3 source = Source.transform.position;
        float ratio = _controller.GetArmRatio();

        Vector3 dif = transform.position - source;
        if (dif.magnitude == 0)
        {
            Line.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            Line.position = (transform.position + source) / 2.0f;
            Line.rotation = Quaternion.LookRotation(dif);
            Line.localScale = new Vector3(ratio * 30.0f, ratio * 30.0f, Mathf.Max(10.0f, dif.magnitude));
        }
    }
}
