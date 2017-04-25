using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ConstructionMenu : MonoBehaviour {

        public UnityEngine.UI.Text Description;
        public UnityEngine.UI.Text OreCost;
        public GameObject InsufficientOreBar;
        public ModelImage Target;

        private BaseManager _manager;

        public void ButtonHover(ConstructionButton button)
        {
            if (button == null)
            {
                Description.text = "Select an item to construct.";
                OreCost.text = "---";
                InsufficientOreBar.SetActive(false);
                Target.SetSource(null);
            }
            else
            {
                Description.text = button.Description;
                OreCost.text = button.OreCost.ToString();
                Target.SetSource(button.Source);
                bool enough = button.OreCost <= _manager.Resources().OreStored();
                print(button.OreCost.ToString() + " <= " + _manager.Resources().OreStored() + " ==> " + enough.ToString());
                InsufficientOreBar.SetActive(!enough);
            }

        }
        public void ButtonPress(ConstructionButton button)
        {
            bool enough = button.OreCost <= _manager.Resources().OreStored();

            bool valid = enough;
            if ( valid )
            {
                _manager.Resources().AddOre(-button.OreCost);
                button.OnClick.Invoke();
                ButtonHover(button);
            }
        }

        private void Start()
        {
            _manager = FindObjectOfType<BaseManager>();
            ButtonHover(null);
        }
    }
}