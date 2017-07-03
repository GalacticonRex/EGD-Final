using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class DroneManager : MonoBehaviour
    {
        public int DockedDrones;
        public GameObject DronePrefab;
        public AudioSource Launch;
        public AudioSource Return;

        private int _max_drones;

        private List<DroneTask> _tasks;
        private Dictionary<DroneAI, DroneTask> _drone_to_task;
        private Dictionary<DroneTask, DroneAI> _task_to_drone;

        private ArtifactScreen _artf_screen;
        private ResourceManager _resources;

        private DroneUIBinding[] _bindings;

        public void BuildDrone()
        {
            DockedDrones++;
            _max_drones++;
        }
        public void QueueDroneTask(DroneTask task)
        {
            DroneTaskManager mngr = task.GetComponent<DroneTaskManager>();
            if ( mngr == null )
            {
                task.gameObject.AddComponent<DroneTaskManager>();
            }
            _tasks.Add(task);
        }
        public void FinishDroneTask(DroneTask task)
        {
            _task_to_drone.Remove(task);
            _drone_to_task.Remove(task.assignedDrone);
        }
        public void CancelTask(DroneTask task)
        {
            _tasks.Remove(task);
        }
        public void ReturnToShip(DroneAI drone)
        {
            Return.Play();
            float o = drone.GetOre();
            TechPiece t = drone.GetTech();
            Artifact a = drone.GetArtifact();

            if (a != null)
            {
                _resources.RequestStorage(a);
            }
            if (t != null)
            {
                _resources.RequestStorage(t);
            }
            if (o > 0)
            {
                _resources.RequestStorage(o);
            }

            Destroy(drone.gameObject);
            DockedDrones++;
        }
        private void Start()
        {
            InterfaceMenu im = FindObjectOfType<InterfaceMenu>();
            _artf_screen = im.element(InterfaceMenu.MenuType.ArtifactViewing).GetComponent<ArtifactScreen>();
            _resources = FindObjectOfType<ResourceManager>();

            _tasks = new List<DroneTask>();
            _drone_to_task = new Dictionary<DroneAI, DroneTask>();
            _task_to_drone = new Dictionary<DroneTask, DroneAI>();

            _max_drones = DockedDrones;

            _bindings = FindObjectsOfType<DroneUIBinding>();
        }
        private void Update()
        {
            if ( _tasks.Count > 0 && DockedDrones > 0 )
            {
                GameObject go = Instantiate(DronePrefab);
                Launch.Play();
                go.transform.position = transform.position;

                DroneAI ai = go.GetComponent<DroneAI>();

                ai.Task = _tasks[0];
                ai.Task.AssignToDrone(ai);
                _tasks.RemoveAt(0);

                _drone_to_task[ai] = ai.Task;
                _task_to_drone[ai.Task] = ai;

                DockedDrones--;
            }

            foreach(DroneUIBinding bind in _bindings)
            {
                bind.availableDrones = DockedDrones;
                bind.maxDrones = _max_drones;
            }
        }

    }
}