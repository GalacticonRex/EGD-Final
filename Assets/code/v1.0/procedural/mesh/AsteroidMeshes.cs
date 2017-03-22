using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMeshes : MonoBehaviour {
    public GameObject Hover;
    public Asteroid Hovering;

    public Mesh Source;
    public Material MaterialNormal;
    public Material MaterialSpecial;
    public int Count = 0;
    public float MinPerturb = 0.8f;
    public float MaxPerturb = 1.3f;

    public GameObject OreDesposits;
    public float OreProbablilty;
    public float OreMinimum = 50.0f;
    public float OreMaximum = 1000.0f;

    private ParticleSystem _particles;
    private TextInterface _display;
    private PlayerMove _player;
    private Mesh[] _elements;
    private MeshRenderer _regular;
    private MeshRenderer _special;

    public Material GetNormalMaterial()
    {
        if ( _regular == null )
            generate();
        return _regular.material;
    }
    public Material GetSpecialMaterial()
    {
        if ( _special == null )
            generate();
        return _special.material;
    }

    public Mesh GetMesh()
    {
        if (_elements == null )
            generate();

        return _elements[Random.Range(0,_elements.Length)];
    }
    public Mesh GetMesh(int i)
    {
        if (_elements == null)
            generate();

        if (i >= _elements.Length)
            return _elements[_elements.Length - 1];
        return _elements[i];
    }

    private void generate()
    {
        if (Source == null || Count == 0)
            return;

        GameObject A = new GameObject();
        A.name = "Regular Material Object";
        {
            _regular = A.AddComponent<MeshRenderer>();
            _regular.material = MaterialNormal;
            ScannerControl s = A.AddComponent<ScannerControl>();
            s.ScannerColor = Color.blue;
        }
        A.transform.parent = transform;

        GameObject B = new GameObject();
        B.name = "Special Material Object";
        {
            _special = B.AddComponent<MeshRenderer>();
            _special.material = MaterialSpecial;
            ScannerControl s = B.AddComponent<ScannerControl>();
            s.ScannerColor = Color.white;
        }
        B.transform.parent = transform;

        _elements = new Mesh[Count];

        for (int i = 0; i < Count; i++)
        {
            _elements[i] = new Mesh();
            _elements[i].name = "Asteroid " + i.ToString();

            Vector3[] verts = Source.vertices;

            for (int j = 0; j < verts.Length; j++)
            {
                bool found = false;
                for (int k = 0; k < j; k++)
                {
                    if (verts[j].normalized == verts[k].normalized)
                    {
                        verts[j] = verts[k];
                        found = true;
                    }
                }
                if (!found) {
                    Vector3 vert = verts[j];
                    Vector3 norm = vert.normalized;
                    verts[j] = norm * Random.Range(MinPerturb, MaxPerturb);
                }
            }

            _elements[i].vertices = verts;
            _elements[i].triangles = Source.triangles;
            _elements[i].uv = Source.uv;
            _elements[i].normals = Source.normals;
            _elements[i].colors = Source.colors;
            _elements[i].tangents = Source.tangents;
        }
    }

    private void Start()
    {
        _display = FindObjectOfType<TextInterface>();
        _player = FindObjectOfType<PlayerMove>();
        _particles = Hover.GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        Hovering = null;

        if (_player.GetMode() > 0.9f)
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit))
            {
                Asteroid a = hit.collider.GetComponent<Asteroid>();
                if (a != null && Vector3.Distance(a.transform.position, _player.transform.position) < _player.HarvestDistance)
                {
                    OreDeposit o = a.GetComponentInChildren<OreDeposit>();
                    Caption c = a.GetComponentInChildren<Caption>();
                    if (o != null)
                    {
                        _player.Select(a);

                        Hovering = a;

                        Hover.transform.position = Hovering.transform.position;
                        Hover.transform.localScale = Hovering.transform.localScale * 3.0f;

                        c.Text = "Ore Deposit of " + Mathf.RoundToInt(o.Remaining()).ToString();
                    }
                }
            }
        }

        if (Hovering == null)
        {
            Hover.GetComponent<Renderer>().enabled = false;
            if (_particles.isPlaying)
                _particles.Stop();
        }
        else
        {
            Hover.GetComponent<Renderer>().enabled = true;
            if (Input.GetMouseButton(0))
            {
                Hover.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);

                OreDeposit ore = Hovering.GetComponent<OreDeposit>();
                float got = ore.Extract(Mathf.Min(_player.Storage.remaining, 100.0f * Time.deltaTime));
                float amount = _player.AddOre(got);

                if (got > 0)
                {
                    if (!_particles.isPlaying)
                        _particles.Play();
                }
                else if (_particles.isPlaying)
                    _particles.Stop();
            }
            else
            {
                Hover.GetComponent<Renderer>().material.color = new Color(1.0f, 0.6796875f, 0.89453125f, 0.4f);
                if (_particles.isPlaying)
                    _particles.Stop();
            }
                
        }
    }
}
