using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TechSelector : MonoBehaviour
    {
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

        void PushSuccess(TechComponent tech, float tw)
        {
            _log.Push(new Notification(
                "Collected Tech",
                tech.Tech.Name(),
                "Tech collection was successful! Total weight was " + Mathf.RoundToInt(tw).ToString())
            );
            Destroy(tech.gameObject);
        }
        void PushFailure(TechComponent tech, float tw)
        {
            _log.Push(new Notification(
                "Not Enough Storage",
                Mathf.RoundToInt(tw).ToString() + " / " + _player.resourceManager.Storage.remaining,
                "There is not enough space to store " + tech.Tech.Name())
            );
        }
        void CreateTask(TechComponent tech)
        {
            DroneTask a = tech.gameObject.AddComponent<DroneTask>();
            a.OnTaskComplete = new DroneTaskEvent();
            a.OnTaskComplete.AddListener(tech.PickUp);

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

            TechComponent tech = s.GetComponent<TechComponent>();
            if (tech == null)
            {
                _renderer.enabled = false;
                _hover = false;
            }
            else
            {
                Debug.Log("HIT!");
                transform.position = tech.transform.position;
                transform.localScale = tech.transform.localScale * 5.0f;
                _renderer.enabled = true;

                if (!tech.Extracting && _hover && Input.GetMouseButtonUp(0))
                {
                    CreateTask(tech);
                    tech.Extracting = true;
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