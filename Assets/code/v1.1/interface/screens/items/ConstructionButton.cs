using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ConstructionButton : MonoBehaviour {

        public string Description;
        public float OreCost;
        public UnityEngine.Events.UnityEvent OnClick;
        public ModelViewer Source;

        private ConstructionMenu _menu;

        public void OnEnter()
        {
            _menu.ButtonHover(this);
        }
        public void OnLeave()
        {
            _menu.ButtonHover(null);
        }
        public void OnPress()
        {
            _menu.ButtonPress(this);
        }

        private void Start()
        {
            _menu = FindObjectOfType<ConstructionMenu>();
        }

    }
}