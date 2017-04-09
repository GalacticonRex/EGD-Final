using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ConstructionMenu : MonoBehaviour {

        public UnityEngine.UI.Text Description;

        public UnityEngine.UI.Text OreCost;
        public GameObject InsufficientOreBar;

        private BaseManager _manager;

        public void ButtonHover(ConstructionButton button)
        {
            if (button == null)
            {
                Description.text = "Select an item to construct.";
                OreCost.text = "---";
                InsufficientOreBar.SetActive(false);
            }
            else
            {
                Description.text = button.Description;
                OreCost.text = button.OreCost.ToString();
                InsufficientOreBar.SetActive((button.OreCost <= _manager.Resources().OreStored()));
            }

        }
        public void ButtonPress(ConstructionButton button)
        {
            
        }

        private void Start()
        {
            _manager = FindObjectOfType<BaseManager>();
            ButtonHover(null);
        }
    }
}