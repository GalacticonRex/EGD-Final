using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ArtifactUIItem : MonoBehaviour {

        public Artifact Item;

        private ArtifactScreen _parent;
        private UnityEngine.UI.Text _name;

        public void Select()
        {
            _parent.Push(this);
        }

        void Start() {
            _parent = GetComponentInParent<ArtifactScreen>();
            _name = GetComponentInChildren<UnityEngine.UI.Text>();
        }

        private void Update()
        {
            if ( Item != null )
                _name.text = Item.Name();
        }

    }
}