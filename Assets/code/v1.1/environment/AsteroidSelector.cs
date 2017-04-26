using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class AsteroidSelector : MonoBehaviour
    {
        private Player _player;
        private Renderer _renderer;

        void CreateTask(OreDeposit ore, float work)
        {
            DroneTask a = ore.gameObject.AddComponent<DroneTask>();

            a.DuringTask = new DroneTaskEvent();
            a.DuringTask.AddListener(ore.DroneExtraction);

            a.WorkRemaining = work;
            a.WorkRadius = 0.5f;
            a.WorkPosition = Random.onUnitSphere * transform.localScale.x;
            a.WorkAxis = a.WorkPosition.normalized;

            _player.droneManager.QueueDroneTask(a);
        }

        void Start()
        {
            _player = FindObjectOfType<Player>();
            _renderer = GetComponent<Renderer>();

            _renderer.enabled = false;
        }

        void Update()
        {
            Selectable s = null;
            if (!_player.inputs.ui && _player.inputs.hit != null)
                s = _player.inputs.hit.GetComponent<Selectable>();

            if (s == null)
            {
                _renderer.enabled = false;
                return;
            }

            OreDeposit ore = s.GetComponent<OreDeposit>();
            if (ore == null || ore.Extracting == true)
            {
                _renderer.enabled = false;
            }
            else
            {
                MeshFilter mesh = ore.GetComponent<MeshFilter>();
                transform.position = ore.transform.position - mesh.mesh.bounds.center;
                transform.localScale = ore.transform.localScale * 5.0f;
                _renderer.enabled = true;
                if (Input.GetMouseButton(0))
                {
                    _renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
                    if ( Input.GetMouseButtonDown(0) )
                    {
                        CreateTask(ore, 10.0f);
                    }
                }
                else
                {
                    _renderer.material.color = new Color(1.0f, 0.6796875f, 0.89453125f, 0.4f);
                }
            }
        }
    }
}