using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar { 
    public class ArtifactSelector : MonoBehaviour {
        private Player _player;
        private Renderer _renderer;
        private NotificationLog _log;
        private bool _hover;

        void Start()
        {
            _player = FindObjectOfType<Player>();
            _log = FindObjectOfType<NotificationLog>();
            _renderer = GetComponent<Renderer>();
            _renderer.enabled = false;
        }

        void CreateTask(ArtifactObject artf)
        {
            DroneTask a = artf.gameObject.AddComponent<DroneTask>();
            a.OnTaskComplete = new DroneTaskEvent();
            a.OnTaskComplete.AddListener(artf.PickUp);

            a.WorkRemaining = 1.0f;
            a.WorkRadius = 0.5f;
            a.WorkPosition = Random.onUnitSphere;
            a.WorkAxis = a.WorkPosition.normalized;

            _player.droneManager.QueueDroneTask(a);
        }

        void Update()
        {
            Selectable s = null;
            if (!_player.inputs.ui && _player.inputs.hit != null)
                s = _player.inputs.hit.GetComponent<Selectable>();

            if (s == null)
            {
                _renderer.enabled = false;
                _hover = false;
                return;
            }

            ArtifactObject artf = s.GetComponent<ArtifactObject>();
            if (artf == null)
            {
                _renderer.enabled = false;
                _hover = false;
            }
            else
            {
                transform.position = artf.transform.position;
                transform.localScale = artf.transform.localScale * 5.0f;
                _renderer.enabled = true;

                if (!artf.Extracting && _hover && Input.GetMouseButtonUp(0))
                {
                    CreateTask(artf);
                    artf.Extracting = true;
                }

                if (Input.GetMouseButton(0))
                {
                    _renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
                    _hover = true;
                }
                else
                {
                    _renderer.material.color = new Color(1.0f, 0.6796875f, 0.89453125f, 0.4f);
                    _hover = false;
                }
            }
        }
    }
}