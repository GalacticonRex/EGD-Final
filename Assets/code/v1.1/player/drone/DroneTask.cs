using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    [System.Serializable]
    public class DroneTaskEvent : UnityEngine.Events.UnityEvent<DroneAI>
    {
    }

    public class DroneTask : MonoBehaviour {

        public DroneTaskEvent OnTaskComplete;
        public DroneTaskEvent DuringTask;

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

        private void OnDestroy()
        {
            _manager.CancelTask(this);
        }

    }
}