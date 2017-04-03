using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class OreDeposit : MonoBehaviour
    {
        public float InitialAmount;
        public bool Extracting;

        private float _last;
        private float _total;
        private CaptionText _cap;

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

                Destroy(_cap);

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

        public void DroneExtraction(DroneAI drone)
        {
            drone.AddOre(Extract(drone.OreExtractionSpeed * Time.deltaTime));
        }

        private void Start()
        {
            Extracting = false;
            _total = InitialAmount;
            _last = _total;
            _cap = GetComponentInChildren<CaptionText>();
        }
        private void Update()
        {
            if (_last != _total)
            {
                _cap.Text = "Ore Deposit of " + Mathf.RoundToInt(_total).ToString();
                _last = _total;
            }
        }
    }
}