using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComplexMesh : MonoBehaviour {

    public ComplexAssetDatabase.Type MeshType = ComplexAssetDatabase.Type.Any;
    public int MaxChildDepth = 0;
    public float MaxWiggle = 0.0f;
    public bool Activate = false;

    public int VertexCount()
    {
        return _vertices.Length;
    }

    private ComplexVertex[] _vertices;
    private Quaternion[] _root_rotation;
    private Quaternion[] _source_rotation;
    private Quaternion[] _target_rotation;
    private int _in_use_count;
    private float _wiggle_lerp;

    public ComplexVertex GetNextVertex()
    {
        print(gameObject.name);
        if (_in_use_count >= _vertices.Length)
            return null;
        ComplexVertex vert = _vertices[_in_use_count];
        _in_use_count++;
        return vert;
    }

	void Awake () {
        _vertices = GetComponentsInChildren<ComplexVertex>(true);
        print(_vertices.Length);
        List<Quaternion> rot = new List<Quaternion>();
        List<Quaternion> gen = new List<Quaternion>();
        foreach (ComplexVertex vert in _vertices)
        {
            rot.Add(vert.transform.localRotation);
            gen.Add(Quaternion.AngleAxis((Random.value - 0.5f) * MaxWiggle, vert.transform.right) *
                    Quaternion.AngleAxis((Random.value - 0.5f) * MaxWiggle, vert.transform.up) * vert.transform.localRotation);
        }
        _root_rotation = rot.ToArray();
        _source_rotation = rot.ToArray();
        _target_rotation = gen.ToArray();

        _in_use_count = 0;
        _wiggle_lerp = 0.0f;
    }
    private void Update()
    {
        if (!Activate || MaxWiggle <= 0.0f)
            return;

        if ( _wiggle_lerp >= 1.0f )
        {
            for (int i = 0; i < _vertices.Length; i++)
            {
                ComplexVertex vert = _vertices[i];
                Quaternion root = _root_rotation[i];
                float wiggle = MaxWiggle / MaxChildDepth;
                _source_rotation[i] = _target_rotation[i];
                _target_rotation[i] = Quaternion.AngleAxis((Random.value - 0.5f) * wiggle, vert.transform.right) * 
                                        Quaternion.AngleAxis((Random.value - 0.5f) * wiggle, vert.transform.up) * root;
            }
            _wiggle_lerp = 0.0f;
        }

        for(int i=0;i<_vertices.Length;i++)
        {
            ComplexVertex vert = _vertices[i];
            Quaternion root = _root_rotation[i];
            Quaternion src = _source_rotation[i];
            Quaternion dest = _target_rotation[i];
            vert.transform.localRotation = Quaternion.Slerp(src, dest, _wiggle_lerp);
        }

        _wiggle_lerp += 0.01f * Time.deltaTime / (MaxChildDepth * MaxChildDepth);
    }

}
