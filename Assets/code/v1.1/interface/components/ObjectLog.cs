using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLog : MonoBehaviour {
    public GameObject Instance;

    private List<GameObject> _objects = new List<GameObject>();
    private RectTransform _self;

    public GameObject Push()
    {
        GameObject n = Instantiate(Instance);
        RectTransform r = n.GetComponent<RectTransform>();
        r.SetParent(transform);

        // >>>>>>>>>>>> GROSS >>>>>>>>>>>>>>>>>>>>>>>>>>>>
        r.anchoredPosition = new Vector2(0, 0);
        r.offsetMin = new Vector2(0, -80);
        r.offsetMax = new Vector2(0, 0);
        r.localScale = new Vector3(1, 1, 1);
        // <<<<<<<<<<<< GROSS <<<<<<<<<<<<<<<<<<<<<<<<<<<<

        foreach (GameObject hnd in _objects)
        {
            hnd.transform.localPosition = hnd.transform.localPosition - new Vector3(0, r.rect.height, 0);
        }

        _objects.Add(n);

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

    private void Start()
    {
        _self = GetComponent<RectTransform>();
    }
}
