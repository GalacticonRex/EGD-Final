using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public Transform OriginPoint;
    public GameObject Instance;
    private Queue<GameObject> _available = new Queue<GameObject>();

    public void Push(GameObject obj)
    {
        _available.Enqueue(obj);
    }
    public GameObject Pop(float thresh)
    {
        GameObject go;
        if (_available.Count == 0)
        {
            go = Instantiate(Instance);
            go.GetComponent<ObjectHider>().Parent = this;
        }
        else
        {
            go = _available.Dequeue();
            go.SetActive(true);
        }
        ObjectHider oh = go.GetComponent<ObjectHider>();
        oh.DistanceThreshold = thresh;
        oh.Init();
        return go;
    }
    public GameObject Pop(float thresh, out bool instantiated)
    {
        GameObject go;
        if (_available.Count == 0)
        {
            go = Instantiate(Instance);
            go.GetComponent<ObjectHider>().Parent = this;
            instantiated = true;
        }
        else
        {
            go = _available.Dequeue();
            go.SetActive(true);
            instantiated = false;
        }
        ObjectHider oh = go.GetComponent<ObjectHider>();
        oh.DistanceThreshold = thresh;
        oh.Init();
        return go;
    }
}
