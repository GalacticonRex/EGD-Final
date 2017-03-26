using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSelector : MonoBehaviour {
    private Player _player;
    private Renderer _renderer;

    void Start()
    {
        _player = FindObjectOfType<Player>();
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

                Debug.Log(tech.Tech.GetTotalWeight());
                if (_player.resourceManager.RequestStorage(tech.Tech))
                {
                    Destroy(tech.gameObject);
                }
            }
            else
            {
                _renderer.material.color = new Color(1.0f, 0.6796875f, 0.89453125f, 0.4f);
            }
        }
    }
}
