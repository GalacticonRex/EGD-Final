using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechPiece {
    private string _name;
    private float _weight;
    private int[] _attachments;
    private TechPiece[] _adjacent;

    private int get_other(int x)
    {
        return x ^ 0x1;
    }

    public TechPiece(string n, float w, int top, int bot, int left, int right)
    {
        _name = n;
        _weight = w;
        _attachments = new int[4] { top, bot, left, right };
        _adjacent = new TechPiece[4] { null, null, null, null };
    }

    public string Name()
    {
        return _name;
    }

    public int GetConnectivity(int x)
    {
        return _attachments[x];
    }
    public bool WillConnect(TechPiece t, int location)
    {
        int location_other = get_other(location);
        return (t != null && t != this && t._attachments[location_other] != -1 && t._attachments[location_other] == _attachments[location]);
    }
    public bool AttemptConnection(TechPiece t, int location, out TechPiece displaced)
    {
        if ( !WillConnect(t,location) )
        {
            displaced = null;
            return false;
        }

        int location_other = get_other(location);

        displaced = t._adjacent[location_other];
        displaced._adjacent[location] = null;

        t._adjacent[location_other] = this;
        _adjacent[location] = t;

        return true;
    }

    public float GetTotalWeight()
    {
        float total = 0;
        HashSet<TechPiece> _visited = new HashSet<TechPiece>();
        Queue<TechPiece> _next = new Queue<TechPiece>();
        _next.Enqueue(this);
        while (_next.Count != 0)
        {
            TechPiece t = _next.Dequeue();
            if ( !_visited.Contains(t) )
            {
                _visited.Add(t);
                total += t._weight;
                for ( int i=0;i<4;i++ )
                {
                    if (t._adjacent[i] != null)
                        _next.Enqueue(t._adjacent[i]);
                }
            }
        }
        return total;
    }
}


public class TechComponent : MonoBehaviour {
    public string Name = "New Tech Component";
    public float Weight = 100;
    public int Top = -1;
    public int Bottom = -1;
    public int Left = -1;
    public int Right = -1;

    private PlayerMove _player;
    private TechPiece _root;
    private Renderer _renderer;
    private Collider _collider;

    public TechPiece Tech
    {
        get
        {
            return _root;
        }
    }

    private void Start () {
        _player = FindObjectOfType<PlayerMove>();
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();

        if (_renderer == null || _collider == null)
            Destroy(this);

        _root = new TechPiece(Name, Weight, Top, Bottom, Left, Right);
    }
}
