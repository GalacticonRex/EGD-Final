using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LastStar
{
    public class AssetDatabase : MonoBehaviour
    {
        #region Constants
        public const string CreatedCiv = "created";
        public const string KilledCiv = "killed";
        public const string WasCiv = "was";
        public const string TraitDead = "dead";
        #endregion
        #region Tuple Pair
        public class Pair<T1, T2>
        {
            public T1 First { get; private set; }
            public T2 Second { get; private set; }
            internal Pair(T1 first, T2 second)
            {
                First = first;
                Second = second;
            }
            public override int GetHashCode()
            {
                int hash = 13;
                hash = (hash * 7) + First.GetHashCode();
                hash = (hash * 7) + Second.GetHashCode();
                return hash;
            }
            public override bool Equals(object obj)
            {
                return Equals(obj as Pair<T1,T2>);
            }
            public bool Equals(Pair<T1,T2> p)
            {
                return (First.Equals(p.First) && Second.Equals(p.Second));
            }
        }
        public static class Tuple
        {
            public static Pair<T1, T2> New<T1, T2>(T1 first, T2 second)
            {
                Pair<T1, T2> tuple = new Pair<T1, T2>(first, second);
                return tuple;
            }
        }
        #endregion
        #region Generate Random Words
        public static readonly string[] defaultConstants = {
            "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "w", "x", "z",
            "th", "ch", "sh", "wh", "tt", "nn"
        };
        public static readonly string[] defaultVowels = {
            "a", "e", "i", "o", "u",
            "ea", "oo", "ui", "ai"
        };

        private static string __generateName(System.Random r, int min, int max, string[] consonants, string[] vowels)
        {
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
                    if (r.Next(0, 10) <= 7)
                        vowel = !vowel;
                }
            }
            return output;
        }
        #endregion
        #region Helper Classes
        public class UniverseSegment
        {
            private AssetDatabase _parent;
            private List<ActorWeb> _historical;
            private ActorWeb _current;
            private Translator _translator;
            private CivEvent _last_event;
            private int _target_number_of_actors;
            private int _target_universe_age;
            private ulong _years_ago;

            public UniverseSegment(AssetDatabase par, int initial_actors, int num_actors, int age_seg)
            {
                _parent = par;
                _target_number_of_actors = num_actors;
                _target_universe_age = age_seg;

                _years_ago = 0;

                _historical = new List<ActorWeb>();
                _current = new ActorWeb(0);

                for ( int i=0;i<initial_actors;i++ )
                {
                    _current.CreateBlankActor(_parent.GetRandomName(2, 8), _parent.GetTrait());
                }
            }
            public bool NextEra()
            {
                if (_target_universe_age == 0)
                    return false;
                _target_universe_age--;

                _years_ago += (ulong)_parent._random.Next(2000000, 4000000);

                _current.PrintGraph();
                ActorWeb new_web = _current.CreateNewEra();
                _parent.RandomizeEvents();

                _last_event = null;
                Actor[][] possible = null;

                while (possible == null)
                {
                    _last_event = _parent.GetRandomEvent();
                    if (_last_event == null )
                        break;
                    possible = _last_event.FitToWeb(new_web);
                }

                if ( possible != null )
                {
                    print(_last_event.Name + ", " + _last_event.Description);
                    int index = _parent._random.Next(possible.Length);
                    Actor[] result = possible[index];
                    _last_event.ApplyChanges(new_web, result);

                    _translator = new Translator(_parent);
                    for( int i=0;i<result.Length;i++ )
                    {
                        _translator.pnoun(i, result[i].Name);
                    }

                    _historical.Add(_current);
                    _current = new_web;

                    return true;
                }
                else
                {
                    _target_universe_age = 0;
                    return false;
                }
            }
            public ArtifactOutput CreateArtifact()
            {
                return _parent.CreateArtifact(_years_ago, _last_event, _translator);
            }
        }
        public class Actor
        {
            private static ulong _source_index = 0;
            private ulong _instance_index;
            public string Name;
            public string Trait;
            public int Era;
            public List<Actor> Adjacent;
            public ulong Instance
            {
                get
                {
                    return _instance_index;
                }
            }
            public Actor(string name, string trait, int era)
            {
                _instance_index = _source_index ++ ;
                //print("Create actor " + name + "(" + trait + ") from era " + era.ToString() + " = id " + _instance_index.ToString());
                Name = name;
                Trait = trait;
                Era = era;
                Adjacent = new List<Actor>();
            }
            public override bool Equals(object obj)
            {
                return Equals(obj as Actor);
            }
            public bool Equals(Actor a)
            {
                return a != null && a._instance_index == _instance_index;
            }
            public override int GetHashCode()
            {
                return (int)(_instance_index % int.MaxValue);
            }
        }
        public class ActorWeb
        {
            // special relationships include:
            // * created -- for a race that created another
            // * killed -- for a race that killed another
            // * was -- for historic relations

            private int _local_era;
            private HashSet<Actor> _actors;
            private Dictionary<Pair<Actor, Actor>, string> _relations;

            public ActorWeb(int era)
            {
                _actors = new HashSet<Actor>();
                _relations = new Dictionary<Pair<Actor, Actor>, string>();
                _local_era = era;
            }
            public int ActorCount()
            {
                return _actors.Count;
            }
            public Actor[] GetActorArray()
            {
                List<Actor> result = new List<Actor>(_actors.Count);
                foreach(Actor a in _actors)
                {
                    result.Add(a);
                }
                return result.ToArray();
            }
            public ActorWeb CreateNewEra()
            {
                ActorWeb new_web = new ActorWeb(_local_era + 1);
                Dictionary<Actor, Actor> new_actors = new Dictionary<Actor, Actor>();
                foreach( Actor a in _actors )
                {
                    if ( a.Trait == TraitDead )
                        continue;
                    
                    if (!new_actors.ContainsKey(a))
                    {
                        Actor new_a = new Actor(a.Name, a.Trait, a.Era + 1);
                        new_actors.Add(a, new_a);
                        new_web._actors.Add(new_a);
                        //print(a.Instance + " ==> " + new_a.Instance);
                    }

                    foreach ( Actor b in a.Adjacent )
                    {
                        if (b.Trait == TraitDead)
                            continue;

                        if (!new_actors.ContainsKey(b))
                        {
                            Actor new_b = new Actor(b.Name, b.Trait, b.Era + 1);
                            new_actors.Add(b, new_b);
                            new_web._actors.Add(new_b);
                            //print(b.Instance + " ==> " + new_b.Instance);
                        }

                        //print("get " + a.Instance);
                        //print("get " + b.Instance);
                        string relate = GetRelationship(a, b);
                        new_web.SetRelationship(relate, new_actors[a], new_actors[b]);
                    }
                    //new_web.SetRelationship(WasCiv, new_actors[a], a);
                }
                return new_web;
            }
            public Actor CreateBlankActor(string name, string trait)
            {
                Actor n = new Actor(name, trait, _local_era);
                _actors.Add(n);
                return n;
            }
            public void SetDualRelationsnip(string type, Actor a, Actor b)
            {
                SetRelationship(type, a, b);
                SetRelationship(type, b, a);
            }
            public void SetRelationship(string type, Actor a, Actor b)
            {
                if (!_actors.Contains(a) && a.Era == _local_era)
                    _actors.Add(a);

                if (!_actors.Contains(b) && b.Era == _local_era)
                    _actors.Add(b);

                string current;
                var temp = new Pair<Actor, Actor>(a, b);
                if (_relations.TryGetValue(temp, out current) )
                {
                    _relations[temp] = type;
                }
                else
                {
                    _relations.Add(temp, type);
                    a.Adjacent.Add(b);
                }
            }
            public string GetRelationship(Actor a, Actor b)
            {
                string current;
                var temp = new Pair<Actor, Actor>(a, b);
                if (_relations.TryGetValue(temp, out current))
                {
                    return current;
                }
                else
                {
                    return "none";
                }
            }
            public void PrintGraph()
            {
                print("--- Graph For Era " + _local_era.ToString() + " -----------------------------------------------------------");
                /*foreach (Actor a in _actors)
                {
                    print("Have " + a.Name + "(" + a.Trait + ") = id " + a.Instance);
                }*/
                foreach(KeyValuePair<Pair<Actor,Actor>, string> kvp in _relations)
                {
                    Actor a = kvp.Key.First;
                    Actor b = kvp.Key.Second;
                    print(a.Name + "(" + a.Instance.ToString() + ") and " + b.Name + "(" + a.Instance.ToString() + ") are " + kvp.Value);
                }
            }
        }
        public class Translator
        {
            #region Members
            private AssetDatabase _parent;
            private System.Random _random = new System.Random();
            private Dictionary<int, string> _pnoun = new Dictionary<int, string>();
            private Dictionary<string, Dictionary<int, string>> _data;
            #endregion
            #region Methods
            public Translator(AssetDatabase p)
            {
                _parent = p;
                _data = new Dictionary<string, Dictionary<int, string>>();
            }
            public void pnoun(int index, string value)
            {
                if (_pnoun.ContainsKey(index))
                {
                    _pnoun[index] = value[0].ToString().ToUpper() + value.Substring(1);
                }
                else
                {
                    _pnoun.Add(index, value[0].ToString().ToUpper() + value.Substring(1));
                }
            }
            public string pnoun(int index = -1)
            {
                if (index < 0)
                {
                    string output = __generateName(_random, 2, 8, defaultConstants, defaultVowels);
                    output = output[0].ToString().ToUpper() + output.Substring(1);
                    return output;
                }
                else
                {
                    string output;
                    if (_pnoun.TryGetValue(index, out output))
                    {
                        return output;
                    }
                    else
                    {
                        output = __generateName(_random, 2, 8, defaultConstants, defaultVowels);
                        output = output[0].ToString().ToUpper() + output.Substring(1);
                        _pnoun.Add(index, output);
                        return output;
                    }
                }
            }
            public string getElement(string _class, int _value)
            {
                Dictionary<int, string> output;
                if (!_data.TryGetValue(_class, out output))
                {
                    output = new Dictionary<int, string>();
                    _data.Add(_class, output);
                }

                string element;
                if (!output.TryGetValue(_value, out element))
                {
                    element = _parent.Get(_class);
                    output.Add(_value, element);
                }
                return element;
            }
            public string getElement(string _class, int _value, string _filter)
            {
                Dictionary<int, string> output;
                if (!_data.TryGetValue(_class, out output))
                {
                    output = new Dictionary<int, string>();
                    _data.Add(_class, output);
                }

                string element;
                if (!output.TryGetValue(_value, out element))
                {
                    element = _parent.Get(_class, _filter);
                    output.Add(_value, element);
                }
                return element;
            }
            #endregion
        }
        public class Node
        {
            public string type;

            public string[] name_const_data;
            public string[] name_variable_data;

            public string[] const_data;
            public string[] variable_data;
        }
        public class CivEvent
        {
            #region Static Methods
            public static void ParseFillResult(string input, out GenericStatment result)
            {
                result = new GenericStatment();
                result.Trait = "any";

                bool in_paran = false;
                bool start_arrow = false;
                bool arrow_indicated = false;
                bool trait_indicated = false;

                string buffer = "";
                foreach( char c in input )
                {
                    if ( c == ')' )
                    {
                        if (!in_paran || trait_indicated)
                            throw new System.Exception("Invalid statement \"" + input + "\" (invalid close paran)");
                        result.Trait = buffer;
                        buffer = "";
                        trait_indicated = true;
                        in_paran = false;
                    }
                    else if ( c == '(' )
                    {
                        if (in_paran || trait_indicated)
                            throw new System.Exception("Invalid statement \"" + input + "\" (invalid open paran)");
                        if (arrow_indicated)
                            result.Second = buffer;
                        else
                            result.First = buffer;
                        buffer = "";
                        in_paran = true;
                    }
                    else if ( c == '-' )
                    {
                        if (start_arrow || arrow_indicated)
                            throw new System.Exception("Invalid statement \"" + input + "\" (invalid '->')");
                        result.First = buffer;
                        buffer = "";
                        start_arrow = true;
                    }
                    else if ( c == '>' )
                    {
                        if (!start_arrow || arrow_indicated)
                            throw new System.Exception("Invalid statement \"" + input + "\" (invalid '->')");
                        start_arrow = false;
                        arrow_indicated = true;
                    }
                    else if ( c >= 'A' || c <= 'Z' || c >= 'a' || c <= 'z' || c >= '0' || c <= '9' )
                    {
                        buffer += c;
                    }
                    else
                    {
                        throw new System.Exception("Invalid statement \"" + input + "\" (invalid character '" + c + "')");
                    }
                }
                if (buffer.Length != 0)
                {
                    if (arrow_indicated && !trait_indicated)
                    {
                        result.Second = buffer;
                    }
                    else
                    {
                        throw new System.Exception("Invalid statement \"" + input + "\"");
                    }
                }
            }
            #endregion
            #region Member Classes
            public class GenericStatment
            {
                public string First;
                public string Second;
                public string Trait;
                public bool Inverse;
            }
            public class Requirement
            {
                #region Members
                public bool Inverse;
                public int First;
                public int Second;
                public string Trait;
                #endregion
                #region Methods
                public Requirement(CivEvent ev, GenericStatment stmt)
                {
                    Inverse = stmt.Inverse;
                    Trait = stmt.Trait;
                    if (!ev.ActorNames.TryGetValue(stmt.First, out First))
                        throw new System.Exception("Invalid name \"" + stmt.First + "\" (try using \"civ " + stmt.First + "\" first)");
                    if (stmt.Second == null || !ev.ActorNames.TryGetValue(stmt.Second, out Second))
                        Second = -1;
                }
                #endregion
            }
            public class Result
            {
                #region Enums
                public enum Type
                {
                    ChangeTrait,
                    ChangeRelate
                }
                #endregion
                #region Members
                public Type Functionality;
                public int First;
                public int Second;
                public string Trait;
                #endregion
                #region Methods
                public Result(CivEvent ev, GenericStatment stmt)
                {
                    Functionality = Type.ChangeRelate;
                    Trait = stmt.Trait;
                    if (stmt.First == null || !ev.ActorNames.TryGetValue(stmt.First, out First))
                        throw new System.Exception("Invalid name \"" + stmt.First + "\" (try using \"civ " + stmt.First + "\" first)");
                    if (stmt.Second == null || !ev.ActorNames.TryGetValue(stmt.Second, out Second))
                    {
                        Functionality = Type.ChangeTrait;
                        Second = -1;
                    }
                }
                #endregion
            }
            public class ResultList
            {
                #region Members
                public float Probability;
                public Result[] Data;
                #endregion
                #region Methods
                public ResultList(float prob, Result[] res)
                {
                    //print("Result probability = " + prob.ToString());
                    Probability = prob;
                    Data = res;
                }
                #endregion
            }
            #endregion
            #region Members
            public string Name;
            public string Description;
            public int Interest;

            public int ActorsNeeded = 0;
            public Dictionary<string, int> ActorNames = new Dictionary<string, int>();
            public Requirement[] Requirements;
            public ResultList[] Results;

            private System.Random _random = new System.Random();
            #endregion
            #region Methods
            public void AddActor(string name)
            {
                if (ActorNames.ContainsKey(name))
                    throw new System.Exception("Repeated civilization name \"" + name + "\"");
                ActorNames[name] = ActorsNeeded;
                ActorsNeeded++;
            }
            // Gets a list of actors that correspond to template indices, returns null if there is no fit
            public Actor[][] FitToWeb(ActorWeb web)
            {
                List<Actor[]> possible = new List<Actor[]>();

                Actor[] current = web.GetActorArray();

                Actor[][] data = new Actor[ActorsNeeded][];
                for (int j=0;j<ActorsNeeded;j++ )
                    data[j] = current.OrderBy(x => _random.Next()).ToArray();

                int[] i = new int[ActorsNeeded];
                while (i[0] < web.ActorCount())
                {
                    bool fits = true;
                    foreach(Requirement r in Requirements)
                    {
                        if (r.Second == -1)
                        {
                            string civ_trait = data[r.First][i[r.First]].Trait;

                            if (r.Inverse)
                            {
                                if (r.Trait == "any" || civ_trait == r.Trait)
                                {
                                    fits = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (r.Trait != "any" && civ_trait != r.Trait)
                                {
                                    fits = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Actor a = data[r.First][i[r.First]];
                            Actor b = data[r.Second][i[r.Second]];
                            if (a.Equals(b))
                            {
                                fits = false;
                                break;
                            }

                            string rel_trait = web.GetRelationship(a, b);

                            if (r.Inverse)
                            {
                                if (rel_trait != "none" && (r.Trait == "any" || rel_trait == r.Trait))
                                {
                                    fits = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (rel_trait == "none" || (r.Trait != "any" && rel_trait != r.Trait))
                                {
                                    fits = false;
                                    break;
                                }
                            }
                        }
                    }
                    if ( fits )
                    {
                        Actor[] good = new Actor[ActorsNeeded];
                        for (int j = 0; j < ActorsNeeded; j++)
                            good[j] = data[j][i[j]];
                        possible.Add(good);
                    }

                    int index = ActorsNeeded - 1;
                    i[index]++;
                    while (index > 0 && i[index]==web.ActorCount())
                    {
                        i[index] = 0;
                        index--;
                        i[index]++;
                    }
                }
                
                return (possible.Count==0)?null:possible.ToArray();
            }
            // Applies changes to a web using the actor list for the template indices
            public void ApplyChanges(ActorWeb src, Actor[] actors)
            {
                ResultList list = GetRandomResultList();
                foreach(Result r in list.Data)
                {
                    if ( r.Functionality == Result.Type.ChangeRelate )
                    {
                        string current = src.GetRelationship(actors[r.First], actors[r.Second]);
                        src.SetRelationship(r.Trait, actors[r.First], actors[r.Second]);
                    }
                    else
                    {
                        actors[r.First].Trait = r.Trait;
                    }
                }
            }
            public ResultList GetRandomResultList()
            {
                float _calced = (float)_random.NextDouble();
                float _sofar = 0.0f;
                foreach( ResultList r in Results )
                {
                    if ( _calced >= _sofar && _calced < _sofar + r.Probability  )
                    {
                        return r;
                    }
                    _calced += r.Probability;
                }
                return null;
            }
            #endregion
        }
        public class ElementTypes
        {
            #region Members
            private System.Random _random;
            private Dictionary<string, string[]> _sorted;
            private string[] _unsorted;
            #endregion
            #region Methods
            public ElementTypes(string[] a, Dictionary<string, string[]> b)
            {
                _random = new System.Random();
                _sorted = b;
                _unsorted = a;
            }
            public string Get()
            {
                string result = _unsorted[_random.Next(0, _unsorted.Length)];
                return result;
            }
            public string Get(string filter)
            {
                string[] output;
                if (!_sorted.TryGetValue(filter, out output))
                    throw new System.Exception("Cannot get filtered value \"" + filter + "\"");
                string result = output[_random.Next(0, output.Length)];
                return result;
            }
            #endregion
        }
        public class ArtifactOutput {
            public string name;
            public string data;
            public ulong date;
        }
        #endregion
        #region Members
        public string Directory = "procedural";
        public string TraitsFile = "traits";
        public string SourceDirectory = "source";
        public string DataDirectory = "data";
        public string EventDirectory = "event";

        private System.Random _random = new System.Random();
        private List<string> _file_names;
        private List<string> _traits;
        private Dictionary<string, CivEvent> _name_to_event;
        private Dictionary<string, ElementTypes> _elements;
        private Dictionary<string, Node> _data_referenced;
        private Dictionary<string, Node[]> _data_sorted;
        private Node[] _data_unsorted;
        private CivEvent[] _events_unsorted;
        private int _events_sorted_index;
        private CivEvent[] _events_sorted;
        #endregion
        #region Get Values
        public int GetFileCount()
        {
            return _file_names.Count;
        }
        public string GetFileName(int index)
        {
            return _file_names[index];
        }
        public string Get(string _class)
        {
            ElementTypes elem;
            if (!_elements.TryGetValue(_class, out elem))
                throw new System.Exception("Cannot find element of class \"" + _class + "\"");
            string result = elem.Get();
            return result;
        }
        public string Get(string _class, string _filter)
        {
            ElementTypes elem;
            if (!_elements.TryGetValue(_class, out elem))
                throw new System.Exception("Cannot find element of class \"" + _class + "\"");
            string result = elem.Get(_filter);
            return result;
        }
        public string GetTrait()
        {
            return _traits[_random.Next(_traits.Count)];
        }
        public string GetRandomName()
        {
            return __generateName(_random, 2, 8, defaultConstants, defaultVowels);
        }
        public string GetRandomName(int max)
        {
            return __generateName(_random, 2, System.Math.Max(2, max), defaultConstants, defaultVowels);
        }
        public string GetRandomName(int min, int max)
        {
            return __generateName(_random, System.Math.Min(min, max), System.Math.Max(min, max), defaultConstants, defaultVowels);
        }
        public void RandomizeEvents()
        {
            CivEvent[] temp_rand = _events_unsorted.OrderBy(x => _random.Next()).ToArray();
            _events_sorted = temp_rand.OrderBy(x => _random.Next(x.Interest)).Reverse().ToArray();
            _events_sorted_index = 0;
        }
        public CivEvent GetRandomEvent()
        {
            int index = _events_sorted_index;
            _events_sorted_index++;
            return
                (_events_sorted_index>_events_unsorted.Length) ?
                    null : _events_sorted[index];
        }
        #endregion
        #region Parse Files
        private string FillTextBrackets(Translator enc, string[] const_data, string[] variable_data)
        {
            string output = "";
            for (int i = 0; i < variable_data.Length; i++)
            {
                output += const_data[i];
                string[] parsed = variable_data[i].Split(new char[1] { ' ' });
                if (parsed[0] == "pnoun")
                {
                    string added = "";
                    if (parsed.Length == 1)
                    {
                        added = enc.pnoun();
                    }
                    else
                    {
                        int index;
                        if (int.TryParse(parsed[1], out index))
                            added = enc.pnoun(index);
                        else
                            throw new System.Exception("Could not find a replacement for \"" + variable_data[i] + "\"");
                    }
                    output += added;
                }
                else
                {
                    string added = "";
                    if (parsed.Length == 1)
                    {
                        added = Get(parsed[0]);
                    }
                    else if (parsed.Length == 2)
                    {
                        int index;
                        if (int.TryParse(parsed[1], out index))
                        {
                            added = enc.getElement(parsed[0], index);
                        }
                        else
                        {
                            added = Get(parsed[0], parsed[1]);
                        }
                    }
                    else if (parsed.Length == 3)
                    {
                        int index;
                        if (int.TryParse(parsed[1], out index))
                        {
                            added = enc.getElement(parsed[0], index, parsed[2]);
                        }
                    }
                    else
                    {
                        throw new System.Exception("Could not find a replacement for \"" + variable_data[i] + "\"");
                    }
                    print(variable_data[i] + " ==> " + added);
                    output += added;
                }
            }
            output += const_data[const_data.Length - 1];
            return output;
        }
        private void ParseTextBrackets(string fname, string string_data, ref List<string> cons, ref List<string> vars)
        {
            cons.Clear();
            vars.Clear();

            bool is_variable = false;
            string buffer = "";
            foreach (char c in string_data)
            {
                if (c == '[' && !is_variable)
                {
                    cons.Add(buffer);
                    buffer = "";
                    is_variable = true;
                }
                else if (c == ']' && is_variable)
                {
                    vars.Add(buffer);
                    buffer = "";
                    is_variable = false;
                }
                else if (c == '[' || c == ']')
                {
                    throw new System.Exception("From " + fname + " : invalid token " + c);
                }
                else
                {
                    buffer += c;
                }
            }
            cons.Add(buffer);
        }
        public Node ParseTextAsset(string fname, string text)
        {
            Node result = new Node();
            result.name_const_data = new string[1] { Path.GetFileNameWithoutExtension(fname) };
            result.name_variable_data = null;

            int index = 0;
            string[] raw_lines = text.Split(new char[] { '\n', '\r' });
            List<string> temp = new List<string>();
            foreach (string l in raw_lines)
            {
                if (l.Length != 0)
                {
                    temp.Add(l);
                }
            }
            string[] lines = temp.ToArray();

            List<string> vars = new List<string>();
            List<string> cons = new List<string>();

            bool searching_meta = true;
            while (searching_meta)
            {
                string[] meta = lines[index].Split(new char[] { ' ', '\t' });
                switch ( meta[0].Trim() )
                {
                    case "event":
                        string event_type = meta[1].Trim();
                        if ( _name_to_event.ContainsKey(event_type) )
                        {
                            result.type = event_type;
                        }
                        else if ( event_type == "any" )
                        {
                            result.type = event_type;
                        }
                        else
                        {
                            throw new System.Exception("From " + fname + " : invalid event type \"" + meta[1] + "\"");
                        }
                        break;
                    case "name":
                        string combine = meta[1];
                        for (int i = 2; i < meta.Length; i++)
                            combine += " " + meta[i];
                        ParseTextBrackets(fname, combine, ref cons, ref vars);

                        result.name_const_data = cons.ToArray();
                        result.name_variable_data = vars.ToArray();

                        break;
                    default:
                        searching_meta = false;
                        break;
                }
                if (searching_meta)
                    index++;
            }

            string string_data = lines[index];
            for (index ++; index < lines.Length; index++)
            {
                string_data += "\n" + lines[index];
            }

            ParseTextBrackets(fname, string_data, ref cons, ref vars);
            result.const_data = cons.ToArray();
            result.variable_data = vars.ToArray();

            return result;
        }
        public CivEvent ParseEventAsset(string fname, string text)
        {
            CivEvent ev = new CivEvent();
            ev.Name = Path.GetFileNameWithoutExtension(fname);

            int def_type = 0;

            List<CivEvent.Requirement> temp_req = new List<CivEvent.Requirement>();
            List<int> temp_result_prob = new List<int>();
            List<CivEvent.Result[]> temp_results = new List<CivEvent.Result[]>();
            List<CivEvent.Result> temp_current_results = new List<CivEvent.Result>();

            string[] lines = text.Split(new char[] { '\n' });
            foreach( string line in lines )
            {
                string trim_line = line.Trim();

                string[] temp_split = trim_line.Split(new char[] { ' ' });
                List<string> temp2_split = new List<string>();
                foreach( string ts in temp_split )
                {
                    string trimmed = ts.Trim();
                    if (trimmed.Length == 0)
                        continue;
                    temp2_split.Add(trimmed);
                }
                string[] split = temp2_split.ToArray();

                if (split.Length == 0)
                {
                    continue;
                }
                else
                {
                    string data = split[0];
                    if (data == "require")
                    {
                        if (split.Length > 1)
                            throw new System.Exception("Invalid line \"" + trim_line + "\"");
                        def_type = 1;
                    }
                    else if (data == "result")
                    {
                        if (split.Length == 2)
                        {
                            int output;
                            if (int.TryParse(split[1], out output))
                                temp_result_prob.Add(output);
                            else
                                throw new System.Exception("Cannot use \"" + split[1] + "\" for result probability");
                        }
                        else if (split.Length > 2)
                            throw new System.Exception("Invalid line \"" + trim_line + "\"");

                        def_type = 2;

                        if (temp_current_results.Count > 0)
                        {
                            temp_results.Add(temp_current_results.ToArray());
                            temp_current_results.Clear();
                        }
                    }
                    else if (def_type == 0)
                    {
                        if (data == "description")
                        {
                            ev.Description = split[1];
                            for (int i = 2; i < split.Length; i++)
                                ev.Description += " " + split[i];
                        }
                        else if (data == "priority")
                        {
                            int output;
                            if (!int.TryParse(split[1], out output))
                                throw new System.Exception("From " + fname + " : Invalid line \"" + trim_line + "\"");
                            ev.Interest = output;
                        }
                        else if (data == "civ")
                        {
                            if (split.Length > 2 )
                                throw new System.Exception("From " + fname + " : Invalid line \"" + trim_line + "\"");
                            ev.AddActor(split[1]);
                        }
                        else
                            throw new System.Exception("From " + fname + " : Invalid line \"" + trim_line + "\"");
                    }
                    else if (def_type != 0)
                    {
                        string operation;
                        bool inverse;
                        CivEvent.GenericStatment stmt;
                        if ( split[0] == "not" )
                        {
                            if (def_type == 2)
                                throw new System.Exception("Cannot have \"not\" in result statement");
                            operation = split[1];
                            for (int i=2;i<split.Length;i++ )
                                operation += split[i];
                            inverse = true;
                        }
                        else
                        {
                            operation = split[0];
                            for (int i = 1; i < split.Length; i++)
                                operation += split[i];
                            inverse = false;
                        }
                        CivEvent.ParseFillResult(operation, out stmt);
                        stmt.Inverse = inverse;

                        //print(operation + " == " + stmt.Inverse.ToString() + " " + stmt.First + ", " + stmt.Second + ", " + stmt.Trait);

                        if ( def_type == 1 )
                        {
                            temp_req.Add(new CivEvent.Requirement(ev, stmt));
                        }
                        else if ( def_type == 2 )
                        {
                            temp_current_results.Add(new CivEvent.Result(ev, stmt));
                        }
                    }
                    else
                        throw new System.Exception("From " + fname + " : Invalid line \"" + trim_line + "\"");
                }
            }
            if (temp_current_results.Count > 0)
            {
                temp_results.Add(temp_current_results.ToArray());
                temp_current_results.Clear();
            }

            List<CivEvent.ResultList> temp_result_lists = new List<CivEvent.ResultList>();
            int prob_sum = 0;
            foreach(int i in temp_result_prob)
            {
                prob_sum += i;
            }
            //print("Got " + temp_results.Count + " result lists from " + fname);
            for ( int i=0;i<temp_results.Count;i++ )
            {
                temp_result_lists.Add(new CivEvent.ResultList((float)temp_result_prob[i] / prob_sum, temp_results[i]));
            }

            ev.Requirements = temp_req.ToArray();
            ev.Results = temp_result_lists.ToArray();

            return ev;
        }
        public void ParseData(FileInfo f)
        {
            List<string> temp = new List<string>();
            Dictionary<string, List<string>> sorted_data = new Dictionary<string, List<string>>();

            string text = File.ReadAllText(f.FullName);
            string[] lines = text.Split(new char[] { '\n' });
            foreach (string line in lines)
            {
                if (line.Length == 0)
                    continue;

                string[] split = line.Split(new char[] { '|' });
                if (split.Length == 0 || split[0].Trim().Length == 0)
                {
                    continue;
                }
                else if (split.Length == 1)
                {
                    temp.Add(split[0].Trim());
                }
                else if (split.Length == 2)
                {
                    string[] attribs = split[1].Split(new char[] { ',' });
                    foreach (string attr in attribs)
                    {
                        string s = split[0].Trim();
                        string t = attr.Trim();
                        temp.Add(s);
                        List<string> moo;
                        if (!sorted_data.TryGetValue(t, out moo))
                        {
                            moo = new List<string>();
                            sorted_data.Add(t, moo);
                        }
                        moo.Add(s);
                    }
                }
                else
                    throw new System.Exception("Invalid expression \"" + line + "\"");
            }

            string[] unsorted = new string[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                unsorted[i] = temp[i];
            }

            Dictionary<string,string[]> sorted = new Dictionary<string, string[]>();
            foreach (KeyValuePair<string, List<string>> pair in sorted_data)
            {
                string[] collected = pair.Value.ToArray();
                sorted.Add(pair.Key, collected);
            }

            ElementTypes store = new ElementTypes(unsorted, sorted);
            _elements.Add(Path.GetFileNameWithoutExtension(f.Name), store);
        }
        public void GetTraitFile()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + Directory + "\\" + TraitsFile + ".txt";
            string[] lines = File.ReadAllLines(path);
            _traits = new List<string>(lines);
        }
        public void GetSourceAllFiles()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + Directory + "\\" + SourceDirectory;
            if (!System.IO.Directory.Exists(path))
            {
                throw new System.Exception("Cannot find " + path + " directory!");
            }

            _elements = new Dictionary<string, ElementTypes>();
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.Name[0] == '_')
                    continue;
                ParseData(f);
            }
        }
        public void GetDataAllFiles()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + Directory + "\\" + DataDirectory;
            if (!System.IO.Directory.Exists(path))
            {
                throw new System.Exception("Cannot find " + path + " directory!");
            }

            _data_referenced = new Dictionary<string, Node>();
            _file_names = new List<string>();

            List<string> data = new List<string>();
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.Name[0] == '_')
                    continue;

                _file_names.Add(f.Name);
                data.Add(File.ReadAllText(f.FullName));
            }

            List<Node> nodes = new List<Node>();
            Dictionary<string, List<Node>> temp = new Dictionary<string, List<Node>>();
            for ( int i=0;i<data.Count;i++ )
            {
                Node n = ParseTextAsset(_file_names[i], data[i]);
                _data_referenced.Add(_file_names[i], n);

                nodes.Add(n);

                if (n.type != null && n.type != "" && n.type != "any")
                {
                    List<Node> output;
                    if (!temp.TryGetValue(n.type, out output))
                    {
                        output = new List<Node>();
                        temp.Add(n.type, output);
                    }
                    output.Add(n);
                }
            }
            _data_unsorted = nodes.ToArray();

            _data_sorted = new Dictionary<string, Node[]>();
            foreach ( KeyValuePair<string, List<Node>> kvp in temp )
            {
                _data_sorted.Add(kvp.Key, kvp.Value.ToArray());
            }
        }
        public void GetEventAllFiles()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + Directory + "\\" + EventDirectory;
            if (!System.IO.Directory.Exists(path))
            {
                throw new System.Exception("Cannot find " + path + " directory!");
            }

            List<string> fname = new List<string>();
            List<string> data = new List<string>();
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo f in files)
            {
                if (f.Name[0] == '_')
                    continue;

                fname.Add(f.Name);
                data.Add(File.ReadAllText(f.FullName));
            }

            List<CivEvent> temp_events_unsorted = new List<CivEvent>();
            _name_to_event = new Dictionary<string, CivEvent>();
            for (int i = 0; i < data.Count; i++)
            {
                CivEvent n = ParseEventAsset(fname[i], data[i]);
                _name_to_event.Add(Path.GetFileNameWithoutExtension(fname[i]), n);
                temp_events_unsorted.Add(n);
            }
            _events_unsorted = temp_events_unsorted.ToArray();
        }
        #endregion
        #region Create Events
        public Translator CreateEvent()
        {
            return new Translator(this);
        }
        public ArtifactOutput CreateArtifact(ulong date, Node template, Translator enc)
        {
            ArtifactOutput ao = new ArtifactOutput();
            ao.name = FillTextBrackets(enc, template.name_const_data, template.name_variable_data);
            ao.data = FillTextBrackets(enc, template.const_data, template.variable_data);
            ao.date = date;

            return ao;
        }
        public ArtifactOutput CreateArtifact(ulong date, CivEvent ev, Translator enc)
        {
            Node[] available;
            if (!_data_sorted.TryGetValue(ev.Name, out available))
            {
                return null;
            }
            Node template = available[_random.Next(available.Length)];
            return CreateArtifact(date, template, enc);
        }
        public ArtifactOutput CreateArtifact(ulong date, string fname, Translator enc)
        {
            Node template = _data_referenced[fname];
            return CreateArtifact(date, template, enc);
        }
        public ArtifactOutput CreateArtifact(ulong date, Translator enc)
        {
            Node template = _data_unsorted[_random.Next(0, _data_unsorted.Length)];
            return CreateArtifact(date, template, enc);
        }
        #endregion

        public AssetDatabase()
        {
            GetTraitFile();
            GetSourceAllFiles();
            GetEventAllFiles();
            GetDataAllFiles();
        }
    }
}