using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexMaker : MonoBehaviour {

    public ComplexAssetDatabase Source;

    public int MaxItems;
    public int DistanceBetweenCores;
    public int MaxDepth;

    public float MinScale;
    public float MaxScale;

    private int _current_items;

    public void Generate()
    {
        _current_items = 0;

        GameObject root = Source.CreateCore();
        root.transform.SetParent(transform);
        root.transform.localPosition = new Vector3(0, 0, 0);
        root.transform.localRotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
        root.transform.localScale = new Vector3(1, 1, 1);

        ComplexMesh mesh = root.GetComponent<ComplexMesh>();

        int cap = mesh.VertexCount();
        print("Intial vertices = " + cap);
        int result = 1;
        for (int i = 0; i < cap; i++)
        {
            ComplexVertex next_vert = mesh.GetNextVertex();
            if (next_vert == null)
                return;

            _current_items++;
            result = System.Math.Max(result, Generate(next_vert, root.transform.position, 0, DistanceBetweenCores));
        }

        mesh.MaxChildDepth = result;
    }
    private int Generate(ComplexVertex vert, Vector3 root, int depth, int last_core)
    {
        if (_current_items >= MaxItems)
            return 1;

        ComplexAssetDatabase.Type type = vert.VertexType;

        if ( last_core > 0 )
            type = (type & ~ComplexAssetDatabase.Type.Core);

        if (depth > MaxDepth)
            type = ComplexAssetDatabase.Type.Endpoint;

        GameObject next = Source.Create(type);
        ComplexMesh mesh = next.GetComponent<ComplexMesh>();
        mesh.MaxChildDepth = 1;
        next.transform.SetParent(vert.transform);

        if ((mesh.MeshType & ComplexAssetDatabase.Type.Core) != 0)
            last_core = DistanceBetweenCores;
        else
            last_core--;

        ComplexVertex src_vert = mesh.GetNextVertex();
        if (src_vert == null)
            return mesh.MaxChildDepth;

        next.transform.localPosition = -src_vert.transform.localPosition;
        next.transform.localRotation = Quaternion.Euler(new Vector3(0, Random.value * 360.0f, 0.0f));
        if (mesh.MeshType == ComplexAssetDatabase.Type.Endpoint)
        {
            float rscale = Random.Range(MinScale, MaxScale);
            next.transform.localScale = new Vector3(rscale, rscale, rscale);
        }
        else
            next.transform.localScale = new Vector3(1, 1, 1);

        if (mesh.MeshType == ComplexAssetDatabase.Type.Arm)
        {
            for (int i = 0; i < mesh.VertexCount(); i++)
            {
                ComplexVertex next_vert = mesh.GetNextVertex();
                if (next_vert == null)
                    return mesh.MaxChildDepth;

                mesh.MaxChildDepth = System.Math.Max(mesh.MaxChildDepth, Generate(next_vert, root, depth + 1, last_core));
            }
        }
        else
        {
            int cap = Mathf.RoundToInt(Random.value * mesh.VertexCount());

            for (int i = 0; i < cap; i++)
            {
                ComplexVertex next_vert = mesh.GetNextVertex();
                if (next_vert == null)
                    return mesh.MaxChildDepth;

                _current_items++;
                mesh.MaxChildDepth = System.Math.Max(mesh.MaxChildDepth, Generate(next_vert, root, depth + 1, last_core));
            }
        }
        return mesh.MaxChildDepth;
    }

    private void Start()
    {
        Generate();
    }
}
