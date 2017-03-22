using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
    private int _actors_needed;
    private List<Environment> _eras;
    public delegate void SearchActors(Actor actor);

    public void BuildEras()
    {
        if (_eras == null)
        {
            _actors_needed = 0;
            _eras = new List<Environment>();
        }
    }

    private void Start()
    {
        BuildEras();
    }

    public int currentEra()
    {
        return (_eras==null)?0:_eras.Count;
    }
    public Environment currentEnvironment()
    {
        return (_eras==null||_eras.Count==0)?null:_eras[_eras.Count-1];
    }
    public Environment createNewEra()
    {
        Environment e = new Environment(this, Environment.randomTrait());
        _eras.Add(e);
        return e;
    }
    public void createNewActor(string n)
    {
        Actor a = new Actor(this, n, Actor.randomTrait(), Actor.randomTrait());
        bfsActors((Actor actor) => {
            a.relate(actor, Relationship.randomQuality());
            actor.relate(a, Relationship.randomQuality());
        });
    }

    public void bfsActors(SearchActors func)
    {
        bfsActors(_eras.Count - 1, func);
    }
    public void bfsActors(int era, SearchActors func)
    {
        Queue<Actor> q = new Queue<Actor>();
        HashSet<Actor> traversed = new HashSet<Actor>();

        Environment current = this.currentEnvironment();
        for (int i=0;i<current.actorCount();i++)
            q.Enqueue(current.getActor(i));

        while (q.Count != 0)
        {
            Actor a = q.Dequeue();
            func(a);
            a.enqueue(ref q, ref traversed);
        }
    }
}
