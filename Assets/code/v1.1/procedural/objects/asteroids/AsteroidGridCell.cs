using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class AsteroidGridCell : MonoBehaviour
    {

        public AsteroidGrid ParentGrid;
        public float Size;
        public int[] Location;
        public UnityEngine.Events.UnityEvent OnInit;

        public void Init()
        {
            OnInit.Invoke();
        }

    }
}