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

        public void SetAmount(float amt)
        {
            _total = amt;
        }
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

                DroneTask[] tasks = GetComponents<DroneTask>();
                foreach (DroneTask task in tasks)
                {
                    if (task.assignedDrone != null)
                        task.assignedDrone.CancelTask();
                    Destroy(task);
                }

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
            float amount = Extract(drone.OreExtractionSpeed * Time.deltaTime);
            drone.AddOre(amount);
        }

        public void ManageCaptionText(GameObject caption_text)
        {
            _cap = GetComponent<CaptionText>();
            if (_cap == null && caption_text != null)
            {
                GameObject ore_go = Instantiate(caption_text);
                ore_go.transform.SetParent(transform);
                ore_go.transform.localPosition = new Vector3();

                _cap = gameObject.AddComponent<CaptionText>();
                _cap.Source = ore_go;

                print("Created CAPTION TEXT");
            }
            if (_cap != null)
            {
                _cap.TextData = "Ore Deposit of " + Mathf.RoundToInt(InitialAmount).ToString();
                _cap.Radius = 4.0f * transform.localScale.x;
            }
        }

        private void Start()
        {
            Extracting = false;

            _total = InitialAmount;
            _last = _total;

            //ManageCaptionText(null);
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