using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class AsteroidObject : MonoBehaviour
    {
        void Start()
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh = Asteroids.GetMesh();
            if (mf.mesh == null)
            {
                Destroy(gameObject);
                return;
            }
            Bounds bound = mf.mesh.bounds;
            SphereCollider collid = GetComponent<SphereCollider>();
            collid.radius = bound.extents.magnitude;
            collid.center = bound.center;
        }
    }
}