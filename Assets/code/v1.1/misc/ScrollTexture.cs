using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {
    public Vector2 Rate;
    private Renderer _source;
    private Vector2 _total;
    private void Start()
    {
        _source = GetComponent<Renderer>();
    }
	private void Update () {
        Debug.Log("HELLO");
        _source.material.SetTextureOffset("_Normal1", _total);
        _source.material.SetTextureOffset("_Normal2", _total);
        _total += Rate * Time.deltaTime;
    }
}
