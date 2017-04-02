using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class OreDeposit : MonoBehaviour
    {
        public float InitialAmount;
        public bool Extracting;

        private float _total;

        public float Remaining()
        {
            return _total;
        }
        public float Extract(float amount)
        {
            if (_total <= amount)
            {
                AsteroidObject parent = GetComponentInParent<AsteroidObject>();
                MeshRenderer mr = parent.GetComponent<MeshRenderer>();
                mr.sharedMaterial = Asteroids.GetNormalMaterial();

                CaptionText cap = GetComponentInParent<CaptionText>();
                Destroy(cap);

                foreach (Transform t in transform)
                    Destroy(t.gameObject);
                Destroy(this);

                return _total;
            }
            else
            {
                _total -= amount;
                return amount;
            }
        }

        private void Start()
        {
            Extracting = false;
            _total = InitialAmount;
        }
    }
}