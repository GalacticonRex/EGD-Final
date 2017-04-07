using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class DroneTaskManager : MonoBehaviour {

        private Player _player;

        private void Start() {
            _player = FindObjectOfType<Player>();
        }
        private void Update() {
            if ( Vector3.Distance(_player.transform.position, transform.position) > _player.selector.MaxDistance ) {
                DroneTask[] tasks = GetComponents<DroneTask>();
                foreach( DroneTask task in tasks )
                {
                    if (task.assignedDrone != null)
                        task.assignedDrone.CancelTask();
                    Destroy(task);
                }
                Destroy(this);
            }
        }
    }
}