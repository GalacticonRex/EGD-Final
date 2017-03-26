using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOverTime : MonoBehaviour {
    public float Speed;
	void Update () {
        transform.rotation = transform.rotation * Quaternion.AngleAxis(Speed * Time.deltaTime, transform.up);
	}
}
