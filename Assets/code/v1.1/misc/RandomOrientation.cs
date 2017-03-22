using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOrientation : MonoBehaviour {
	void Start () {
        transform.rotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
    }
}
