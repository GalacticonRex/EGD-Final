using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class TechModelViewers : MonoBehaviour {

        public ModelViewer[] Sources;
        private Dictionary<string, ModelViewer> _reference;

        public ModelViewer GetModelView(string name)
        {
            ModelViewer output;
            if(!_reference.TryGetValue(name, out output))
            {
                return null;
            }
            return output;
        }

        private void Awake() {
            _reference = new Dictionary<string, ModelViewer>();
            foreach(ModelViewer mod in Sources)
            {
                _reference.Add(mod.ReferenceName, mod);
            }
        }
    }
}