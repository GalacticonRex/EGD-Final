using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class Asteroid : MonoBehaviour
    {
        public AsteroidMeshes Source;
        public Material Matl;

        private void Start()
        {
            MeshFilter mf = gameObject.AddComponent<MeshFilter>();
            MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

            mf.mesh = Source.GetMesh();
            if (mf.mesh == null)
            {
                Destroy(gameObject);
                return;
            }
            mr.sharedMaterial = Matl;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;
        }
    }
}