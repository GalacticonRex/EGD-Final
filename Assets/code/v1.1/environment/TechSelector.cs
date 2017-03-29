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

        void Start()
        {
            _player = FindObjectOfType<Player>();
            _log = FindObjectOfType<NotificationLog>();
            _renderer = GetComponent<Renderer>();
            _renderer.enabled = false;
        }

        void Update()
        {
            Selectable s = _player.selector.selectedItem;
            if (s == null)
            {
                _renderer.enabled = false;
                return;
            }

            TechComponent tech = s.GetComponent<TechComponent>();
            if (tech == null)
            {
                _renderer.enabled = false;
            }
            else
            {
                transform.position = tech.transform.position;
                transform.localScale = tech.transform.localScale;
                _renderer.enabled = true;
                if (Input.GetMouseButton(0))
                {
                    _renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);

                    float tw = tech.Tech.GetTotalWeight();
                    if (_player.resourceManager.RequestStorage(tech.Tech))
                    {
                        _log.push(new Notification("Collected Tech",
                                                    tech.Tech.Name(),
                                                    "Tech collection was successful! Total weight was " + Mathf.RoundToInt(tw).ToString()));
                        Destroy(tech.gameObject);
                    }
                    else
                    {
                        _log.push(new Notification("Not Enough Storage",
                                                    Mathf.RoundToInt(tw).ToString() + " / " + _player.resourceManager.Storage.remaining,
                                                    "There is not enough space to store " + tech.Tech.Name()));
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