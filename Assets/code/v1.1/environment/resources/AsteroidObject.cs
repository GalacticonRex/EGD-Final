using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidObject : MonoBehaviour {
	void Start () {
        MeshFilter mf = GetComponent<MeshFilter>();
        mf.mesh = Asteroids.GetMesh();
        if (mf.mesh == null)
        {
            Destroy(gameObject);
            return;
        }
    }
}
