using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRotationOf : MonoBehaviour {
    public Transform Object;
	void Update () {
        Vector3 dif = Object.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dif.normalized);
	}
}
