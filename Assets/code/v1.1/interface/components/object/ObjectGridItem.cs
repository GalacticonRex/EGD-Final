using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ObjectGridItem : MonoBehaviour {

        public int Index;
        public object Mounted;
        public UnityEngine.Events.UnityEvent OnMountChange;
        public ObjectGrid ParentGrid;

        public void SetHoverIndex()
        {
            ParentGrid.SetHoverIndex(Index);
        }
        private void Start()
        {
            ParentGrid = GetComponentInParent<ObjectGrid>();
        }

    }
}