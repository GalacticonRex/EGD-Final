using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour {
    static private Mesh[] _elements;
    static private Material _regular;
    static private Material _special;

    public Mesh[] Source;
    public Material MaterialNormal;
    public Material MaterialSpecial;

    private void Awake()
    {
        generate(this);
    }

    static public Material GetNormalMaterial()
    {
        return _regular;
    }
    static public Material GetSpecialMaterial()
    {
        return _special;
    }

    static public Mesh GetMesh()
    {
        return _elements[Random.Range(0, _elements.Length)];
    }
    static public Mesh GetMesh(int i)
    {
        if (i >= _elements.Length)
            return _elements[_elements.Length - 1];
        return _elements[i];
    }

    static private void generate(Asteroids a)
    {
        if (a.Source == null || _elements != null)
            return;

        _regular = a.MaterialNormal;
        _special = a.MaterialSpecial;

        _elements = new Mesh[a.Source.Length];

        for (int i = 0; i < a.Source.Length; i++)
        {
            _elements[i] = a.Source[i];
            _elements[i].name = "Asteroid " + i.ToString();
        }
    }
}
