using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ConstructionButton : MonoBehaviour {

        public string Description;
        public float OreCost;

        private ConstructionMenu _menu;

        public void OnEnter()
        {
            _menu.ButtonHover(this);
        }
        public void OnLeave()
        {
            _menu.ButtonHover(null);
        }

        private void Start()
        {
            _menu = FindObjectOfType<ConstructionMenu>();
        }

    }
}