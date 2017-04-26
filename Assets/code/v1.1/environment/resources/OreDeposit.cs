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
        private Player _player;
        private Camera _camera;

        public CaptionText caption { get { return _cap; } }

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

        private void Start()
        {
            Extracting = false;

            _total = InitialAmount;
            _last = _total;

            _player = FindObjectOfType<Player>();
            _camera = _player.GetComponentInChildren<Camera>();

            _cap = GetComponentInChildren<CaptionText>(true);
            _cap.Radius = transform.localScale.x * 2.5f;
        }
        private void Update()
        {
            float dist = Vector3.Distance(transform.position, _camera.transform.position);
            if (dist < _player.selector.MaxDistance)
                _cap.gameObject.SetActive(true);
            else if (_cap.gameObject.activeInHierarchy)
                _cap.gameObject.SetActive(false);

            if (_last != _total)
            {
                _cap.Text = "Ore Deposit of " + Mathf.RoundToInt(_total).ToString();
                _last = _total;
            }
        }
    }
}