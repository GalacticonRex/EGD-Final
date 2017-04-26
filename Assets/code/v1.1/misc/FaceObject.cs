using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour {

    public GameObject Target;
    public Vector3 ScaleRatio;
    public bool ScaleByDistance = true;

    private Vector3 _init_scale;

    private void Start()
    {
        _init_scale = transform.localScale;
    }
	private void LateUpdate () {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        if (ScaleByDistance)
        {
            transform.localScale = new Vector3(ScaleRatio.x * _init_scale.x * dir.magnitude,
                                                ScaleRatio.y * _init_scale.y * dir.magnitude,
                                                ScaleRatio.z * _init_scale.z * dir.magnitude);
        }
        else
        {
            transform.localScale = ScaleRatio;
        }
    }
}
