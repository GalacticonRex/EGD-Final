using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLog : MonoBehaviour {
    public GameObject Instance;
    public UnityEngine.UI.Scrollbar Scroll;
    public float ItemSize = 80.0f;
    public float ScrollScale = 0.1f;

    private List<GameObject> _objects = new List<GameObject>();
    private RectTransform _self;
    private float _height_needed = 0.0f;

    public GameObject Push()
    {
        GameObject n = Instantiate(Instance);
        RectTransform r = n.GetComponent<RectTransform>();
        r.SetParent(transform);

        // >>>>>>>>>>>> GROSS >>>>>>>>>>>>>>>>>>>>>>>>>>>>
        r.anchoredPosition = new Vector2(0, 0);
        r.offsetMin = new Vector2(0, -ItemSize);
        r.offsetMax = new Vector2(0, 0);
        r.localScale = new Vector3(1, 1, 1);
        // <<<<<<<<<<<< GROSS <<<<<<<<<<<<<<<<<<<<<<<<<<<<

        foreach (GameObject hnd in _objects)
        {
            hnd.transform.localPosition = hnd.transform.localPosition - new Vector3(0, r.rect.height, 0);
        }

        _objects.Add(n);
        _height_needed += r.rect.height;

        return n;
    }
    public void Remove(GameObject go)
    {
        int index = _objects.IndexOf(go);
        if ( index >= 0 && index < _objects.Count )
        {
            RectTransform r = go.GetComponent<RectTransform>();
            for ( int i=index+1;i<_objects.Count;i++ )
            {
                _objects[i].transform.localPosition = _objects[i].transform.localPosition + new Vector3(0, r.rect.height, 0);
            }
            _objects.RemoveAt(index);
        }
    }
    public void ModifyValue()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 temp = rect.localPosition;
        temp.y = Scroll.value * (_height_needed - rect.rect.height);
        rect.localPosition = temp;
    }
    public void ScrollValue()
    {
        Scroll.value -= Input.mouseScrollDelta.y * ScrollScale;
    }

    private void Start()
    {
        _self = GetComponent<RectTransform>();
    }
    private void Update()
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (Scroll != null)
        {
            if (_height_needed <= rect.rect.height)
            {
                Scroll.gameObject.SetActive(false);
            }
            else
            {
                Scroll.gameObject.SetActive(true);
                Scroll.size = rect.rect.height / _height_needed;
            }
        }
    }
}
