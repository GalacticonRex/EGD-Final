using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class DroneManager : MonoBehaviour
    {
        public int DockedDrones;
        public GameObject DronePrefab;

        private List<DroneTask> _tasks;
        private Dictionary<DroneAI, DroneTask> _drone_to_task;
        private Dictionary<DroneTask, DroneAI> _task_to_drone;

        public void QueueDroneTask(DroneTask task)
        {
            _tasks.Add(task);
        }
        public void FinishDroneTask(DroneTask task)
        {
            _task_to_drone.Remove(task);
            _drone_to_task.Remove(task.assignedDrone);
        }
        public void ReturnToShip(DroneAI drone)
        {
            if (drone.OnReturnToShip != null)
                drone.OnReturnToShip.Invoke(drone);
            Destroy(drone.gameObject);
            DockedDrones++;
        }
        private void Start()
        {
            _tasks = new List<DroneTask>();
            _drone_to_task = new Dictionary<DroneAI, DroneTask>();
            _task_to_drone = new Dictionary<DroneTask, DroneAI>();
        }
        private void Update()
        {
            if ( _tasks.Count > 0 && DockedDrones > 0 )
            {
                GameObject go = Instantiate(DronePrefab);
                go.transform.position = transform.position;

                DroneAI ai = go.GetComponent<DroneAI>();

                ai.Task = _tasks[0];
                ai.Task.AssignToDrone(ai);
                _tasks.RemoveAt(0);

                _drone_to_task[ai] = ai.Task;
                _task_to_drone[ai.Task] = ai;

                DockedDrones--;
            }
        }

    }
}