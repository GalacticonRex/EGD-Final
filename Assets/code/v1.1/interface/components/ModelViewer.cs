using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelViewer : MonoBehaviour {

    public GameObject Object;
    public int Width;
    public int Height;

    public Texture Result
    {
        get
        {
            return _render_texture;
        }
    }
    public Transform CameraTransform
    {
        get
        {
            return _camera.transform;
        }
    }

    private GameObject _instance;
    private RenderTexture _render_texture;
    private Camera _camera;

	void Awake () {
        _render_texture = new RenderTexture(Width, Height, 16);

        _camera = gameObject.GetComponent<Camera>();
        _camera.targetTexture = _render_texture;

        _instance = Instantiate(Object);
        _instance.layer = _camera.cullingMask;
        _instance.transform.SetParent(transform);
        _instance.transform.localPosition = new Vector3(0, 0, 10);

        transform.rotation = Quaternion.LookRotation(_instance.transform.localPosition, Vector3.up);
    }

}
