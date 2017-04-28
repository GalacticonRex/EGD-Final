using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ComplexMaker : MonoBehaviour
    {
        public int RandSeed;
        public ComplexAssetDatabase Source;

        public int MaxItems;
        public int DistanceBetweenCores;
        public int MaxDepth;

        public float MinEndpointScale;
        public float MaxEndpointScale;

        public float MinCoreScale;
        public float MaxCoreScale;

        private int _current_items;
        private System.Random _random;

        private Vector3 onUnitSphere()
        {
            float x, y, z;
            while ((x = (2.0f * (float)_random.NextDouble() - 1.0f)) == 0.0f) ;
            while ((y = (2.0f * (float)_random.NextDouble() - 1.0f)) == 0.0f) ;
            while ((z = (2.0f * (float)_random.NextDouble() - 1.0f)) == 0.0f) ;
            float len = Mathf.Sqrt(x * x + y * y + z * z);
            return new Vector3(x / len, y / len, z / len);
        }

        private class ComplexIteration
        {
            public ComplexIteration previous;
            public ComplexVertex vertex;
            public ComplexMesh mesh;
            public int depth;
            public int last_core;
        }

        public void AssignPosition()
        {
            AsteroidGridCell Parent = GetComponentInParent<AsteroidGridCell>();
            int x = Parent.Location[0];
            int y = Parent.Location[1];
            int z = Parent.Location[2];
            RandSeed = (x << 17) ^ (x >> 15) ^ (y << 12) ^ (y << 20) ^ (z << 24) ^ (z >> 8);
        }

        public void GenerateAsync()
        {
            _current_items = 0;

            Queue<ComplexIteration> queue = new Queue<ComplexIteration>();

            GameObject root = Source.CreateCore(_random);
            root.transform.SetParent(transform);

            root.transform.localPosition = new Vector3(0, 0, 0);
            root.transform.localRotation = Quaternion.LookRotation(onUnitSphere(), onUnitSphere());
            root.transform.localScale = new Vector3(1, 1, 1);
            float rscale = (float)_random.NextDouble() * (MaxCoreScale - MinCoreScale) + MinCoreScale;
            root.transform.GetChild(0).localScale = new Vector3(rscale, rscale, rscale);

            ComplexMesh mesh = root.GetComponent<ComplexMesh>();

            ComplexIteration initial = new ComplexIteration();
            initial.previous = null;
            initial.vertex = null;
            initial.mesh = mesh;
            initial.depth = 0;
            initial.last_core = DistanceBetweenCores;

            int cap = mesh.VertexCount();
            for (int i = 0; i < cap; i++)
            {
                ComplexVertex next_vert = mesh.GetNextVertex();
                if (next_vert == null)
                    return;

                ComplexIteration iter = new ComplexIteration();
                iter.previous = initial;
                iter.vertex = next_vert;
                iter.mesh = null;
                iter.depth = 1;
                iter.last_core = initial.last_core - 1;

                _current_items++;

                queue.Enqueue(iter);
            }

            StartCoroutine(Generate(queue));
        }
        private void DiscoverDepth(ComplexIteration iter, int depth)
        {
            if (iter.mesh != null && iter.mesh.MaxChildDepth < depth)
            {
                iter.mesh.MaxChildDepth = depth;
                if (iter.previous != null)
                    DiscoverDepth(iter.previous, depth + 1);
            }
        }
        private IEnumerator Generate(Queue<ComplexIteration> queue)
        {
            while (queue.Count > 0 && _current_items < MaxItems)
            {
                yield return null;

                ComplexIteration iter = queue.Dequeue();

                ComplexAssetDatabase.Type type = iter.vertex.VertexType;

                if (iter.last_core > 0)
                    type = (type & ~ComplexAssetDatabase.Type.Core);

                if (iter.depth > MaxDepth)
                    type = ComplexAssetDatabase.Type.Endpoint;

                GameObject next = Source.Create(_random, type);
                iter.mesh = next.GetComponent<ComplexMesh>();
                iter.mesh.MaxChildDepth = 1;
                next.transform.SetParent(iter.vertex.transform);

                if ((iter.mesh.MeshType & ComplexAssetDatabase.Type.Core) != 0)
                    iter.last_core = DistanceBetweenCores;

                ComplexVertex src_vert = iter.mesh.GetNextVertex();
                if (src_vert == null)
                    continue;

                next.transform.localPosition = -src_vert.transform.localPosition;
                next.transform.localRotation = Quaternion.Euler(new Vector3(0, (float)_random.NextDouble() * 360.0f, 0.0f));
                if (iter.mesh.MeshType == ComplexAssetDatabase.Type.Core)
                {
                    float rscale = (float)_random.NextDouble() * (MaxCoreScale - MinCoreScale) + MinCoreScale;
                    next.transform.GetChild(0).localScale = new Vector3(rscale, rscale, rscale);
                }
                if (iter.mesh.MeshType == ComplexAssetDatabase.Type.Endpoint)
                {
                    float rscale = (float)_random.NextDouble() * (MaxEndpointScale - MinEndpointScale) + MinEndpointScale;
                    next.transform.localScale = new Vector3(rscale, rscale, rscale);
                }
                else
                    next.transform.localScale = new Vector3(1, 1, 1);

                if (iter.mesh.MeshType == ComplexAssetDatabase.Type.Arm)
                {
                    for (int i = 0; i < iter.mesh.VertexCount(); i++)
                    {
                        ComplexVertex next_vert = iter.mesh.GetNextVertex();
                        if (next_vert == null)
                            break;

                        ComplexIteration new_iter = new ComplexIteration();
                        new_iter.previous = iter;
                        new_iter.vertex = next_vert;
                        new_iter.depth = iter.depth;
                        new_iter.last_core = iter.last_core - 1;
                        queue.Enqueue(new_iter);
                    }
                }
                else
                {
                    int cap = Mathf.RoundToInt((float)_random.NextDouble() * iter.mesh.VertexCount());

                    for (int i = 0; i < cap; i++)
                    {
                        ComplexVertex next_vert = iter.mesh.GetNextVertex();
                        if (_current_items >= MaxItems || next_vert == null)
                            break;

                        float dif = Vector3.Dot((next_vert.transform.position - transform.position).normalized, next_vert.transform.up);
                        if (dif < 0)
                            continue;

                        _current_items++;

                        ComplexIteration new_iter = new ComplexIteration();
                        new_iter.previous = iter;
                        new_iter.vertex = next_vert;
                        new_iter.depth = iter.depth + 1;
                        new_iter.last_core = iter.last_core - 1;
                        queue.Enqueue(new_iter);

                        DiscoverDepth(new_iter, 1);
                    }
                }
            }
        }

        private void Start()
        {
            _random = new System.Random(UniverseMap.GetSeed() ^ RandSeed);
            GenerateAsync();
        }
    }
}