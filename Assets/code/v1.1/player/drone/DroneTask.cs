using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class DroneTask : MonoBehaviour {
        
        public Vector3 WorkPosition;
        public Vector3 WorkAxis;

        public float WorkRemaining;
        public float WorkRadius;

        private DroneManager _manager;
        private DroneAI _drone;

        public DroneAI assignedDrone
        {
            get { return _drone; }
        }

        public void AssignToDrone(DroneAI d)
        {
            _drone = d;
        }

        private void Start()
        {
            _manager = FindObjectOfType<DroneManager>();
        }
        private void Update()
        {
            if (WorkRemaining <= 0)
            {
                Destroy(this);

                _manager.FinishDroneTask(this);

                _drone.Task = null;
                _drone.ReturnToShip = true;
            }
        }

    }
}