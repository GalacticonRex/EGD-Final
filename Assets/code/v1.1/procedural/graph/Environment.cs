using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class Environment
    {

        public enum Trait
        {
            Hostile,
            Peaceful,
            Wild,
            Urban
        }

        public static Trait randomTrait()
        {
            System.Random r = new System.Random();
            System.Array a = System.Enum.GetValues(typeof(Trait));
            return (Trait)a.GetValue(r.Next(1, a.Length));
        }

        private int _era;
        private Trait _nature;
        private List<Actor> _actors;
        private Environment _past_self;

        public Environment(Generator g, Trait nat)
        {
            _era = g.currentEra();
            _nature = nat;
            _actors = new List<Actor>();
            _past_self = g.currentEnvironment();
            Debug.Log(describe());
        }

        public int actorCount()
        {
            return _actors.Count;
        }
        public void addActor(Actor a)
        {
            _actors.Add(a);
        }
        public Actor getActor(int i)
        {
            return _actors[i];
        }
        public Trait getNature()
        {
            return _nature;
        }
        public void setNature(Trait n)
        {
            _nature = n;
        }
        public string describe()
        {
            return "environment is " + _nature.ToString().ToLower();
        }
    }
}