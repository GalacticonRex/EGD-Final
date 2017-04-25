using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class AsteroidObject : MonoBehaviour
    {
        public System.Random RandomGenerator;
        public void Start()
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf == null)
            {
                mf = gameObject.AddComponent<MeshFilter>();
                mf.mesh = Asteroids.GetMesh(RandomGenerator);
                if (mf.mesh == null)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            Bounds bound = mf.mesh.bounds;
            SphereCollider collid = GetComponent<SphereCollider>();
            collid.radius = Mathf.Max(bound.extents.x, bound.extents.y, bound.extents.z);
            collid.center = bound.center;

            Rigidbody rb = GetComponent<Rigidbody>();
            float r = collid.radius * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            rb.mass = 4.0f/3.0f * Mathf.PI * (r*r*r);
        }
    }
}