using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class BaseManager : MonoBehaviour {

        private BaseResources _resources;

        public BaseResources Resources()
        {
            return _resources;
        }

        void Start() {
            _resources = GetComponent<BaseResources>();
        }

        void Update() {

        }
    }
}