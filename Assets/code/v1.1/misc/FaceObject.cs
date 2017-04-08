using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour {

    public GameObject Target;
    public Vector3 ScaleRatio;

    private Vector3 _init_scale;

    private void Start()
    {
        _init_scale = transform.localScale;
    }
	void Update () {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        transform.localScale = new Vector3( ScaleRatio.x * _init_scale.x * dir.magnitude,
                                            ScaleRatio.y * _init_scale.y * dir.magnitude,
                                            ScaleRatio.z * _init_scale.z * dir.magnitude);
    }
}
