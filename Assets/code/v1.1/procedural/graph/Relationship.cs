using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relationship {
    public enum Quality
    {
        Friendly,
        Cordial,
        Curious,
        Neutral,
        Aggressive,
        Fearful
    }
    public static Quality randomQuality()
    {
        System.Random r = new System.Random();
        System.Array a = System.Enum.GetValues(typeof(Quality));
        return (Quality)a.GetValue(r.Next(a.Length));
    }

    private Actor _Source;
    private Actor _Destination;
    private Quality _Quality;

    public Relationship(Actor A, Actor B, Quality Q)
    {
        _Source = A;
        _Destination = B;
        _Quality = Q;
        Debug.Log(describe());
    }
    public Quality quality()
    {
        return _Quality;
    }
    public Actor traverse(Actor a)
    {
        if (a == _Source)
            return _Destination;
        else if (a == _Destination)
            return _Source;
        else
            return null;
    }
    public void setQuality(Quality q)
    {
        _Quality = q;
    }
    public string describe()
    {
        return _Source.name().ToLower() + " " + _Quality.ToString().ToLower() + " " + _Destination.name().ToLower();
    }
}
