using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor {
    public static readonly string[] defaultConstants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z", "th", "ch", "sh", "wh", "tt", "nn" };
    public static readonly string[] defaultVowels = { "a", "e", "i", "o", "u", "ea", "oo", "ui" };

    public enum Trait
    {
        Greedy,
        Tough,
        Compassionate,
        Manipulative,
        Logical
    }

    public static string NewName(int min, int max, string[] consonants, string[] vowels)
    {
        System.Random r = new System.Random();
        int len = r.Next(min, max);
        string output = "";
        bool vowel = (r.Next(0, 10) < 3);
        for (int i = 0; i < len; i++)
        {
            if (vowel)
            {
                output += vowels[r.Next(vowels.Length)];
                vowel = !vowel;
            }
            else
            {
                output += consonants[r.Next(consonants.Length)];
                if (r.Next(0, 10) < 5)
                    vowel = !vowel;
            }
        }
        return output;
    }
    public static Trait randomTrait()
    {
        System.Random r = new System.Random();
        System.Array a = System.Enum.GetValues(typeof(Trait));
        return (Trait)a.GetValue(r.Next(a.Length));
    }

    private int _era;
    private Environment _env;
    private string _name;
    private Trait _nature;
    private Trait _demeanour;
    private List<Relationship> _relationships;
    private Dictionary<Actor, Relationship> _associations;
    private Actor _past_self;

    public Actor(Generator g, Trait nat, Trait dem)
    {
        _era = g.currentEra();
        _env = g.currentEnvironment();
        _env.addActor(this);
        _name = NewName(4, 12, defaultConstants, defaultVowels);
        _nature = nat;
        _demeanour = dem;
        _relationships = new List<Relationship>();
        _associations = new Dictionary<Actor, Relationship>();
        Debug.Log(describe());
    }
    public Actor(Generator g, string n, Trait nat, Trait dem)
    {
        _era = g.currentEra();
        _env = g.currentEnvironment();
        _env.addActor(this);
        _name = n;
        _nature = nat;
        _demeanour = dem;
        _relationships = new List<Relationship>();
        _associations = new Dictionary<Actor, Relationship>();
        Debug.Log(describe());
    }
    public void enqueue(ref Queue<Actor> q, ref HashSet<Actor> check)
    {
        foreach (Relationship a in _relationships)
        {
            Actor b = a.traverse(this);
            if (!check.Contains(b))
            {
                q.Enqueue(b);
                check.Add(b);
            }
        }
    }
    public string name()
    {
        return _name;
    }
    public Trait nature()
    {
        return _nature;
    }
    public Trait demeanour()
    {
        return _demeanour;
    }
    public void setNature(Trait t)
    {
        _nature = t;
    }
    public void setDemeanour(Trait t)
    {
        _demeanour = t;
    }
    public int relationshipCount()
    {
        return _relationships.Count;
    }
    public Relationship relationship(int i)
    {
        return _relationships[i];
    }

    public Actor successor(Generator g)
    {
        _past_self = new Actor(g, _name, _nature, _demeanour);
        _past_self.relate(this, Relationship.Quality.Neutral);
        return _past_self;
    }

    public Relationship relate(Actor other, Relationship.Quality q)
    {
        if (other == this)
            return null;

        Relationship r;
        if (!_associations.TryGetValue(other, out r))
        {
            r = new Relationship(this, other, q);
            _associations.Add(other, r);
            _relationships.Add(r);
        }
        else
        {
            r.setQuality(q);
        }
        return r;
    }

    public string describe()
    {
        return _name + " is " + _nature.ToString().ToLower() + " by nature and has a " + _demeanour.ToString().ToLower() + " demeanour";
    }
}
