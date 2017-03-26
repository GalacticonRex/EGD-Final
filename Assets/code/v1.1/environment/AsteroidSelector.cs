using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSelector : MonoBehaviour {
    private Player _player;
    private ParticleSystem _particles;
    private Renderer _renderer;

	void Start () {
        _player = FindObjectOfType<Player>();
        _renderer = GetComponent<Renderer>();
        _particles = GetComponentInChildren<ParticleSystem>();

        _renderer.enabled = false;
        _particles.Stop();
    }
	
	void Update () {
        Selectable s = _player.selector.selectedItem;
        if (s == null)
        {
            _renderer.enabled = false;
            if (_particles.isPlaying)
                _particles.Stop();
            return;
        }

        OreDeposit ore = s.GetComponent<OreDeposit>();
        if (ore == null)
        {
            _renderer.enabled = false;
            if (_particles.isPlaying)
                _particles.Stop();
        }
        else
        {
            transform.position = ore.transform.position;
            transform.localScale = ore.transform.localScale;
            _renderer.enabled = true;
            if (Input.GetMouseButton(0))
            {
                _renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.4f);

                float got = ore.Extract(Mathf.Min(_player.resourceManager.Storage.remaining, 100.0f * Time.deltaTime));

                CaptionText t = ore.GetComponentInChildren<CaptionText>();
                t.Text = "Ore Deposit of " + Mathf.RoundToInt(ore.Remaining()).ToString();

                _player.resourceManager.RequestStorage(got);

                if (got > 0)
                {
                    if (!_particles.isPlaying)
                        _particles.Play();
                }
                else if (_particles.isPlaying)
                    _particles.Stop();
            }
            else
            {
                _renderer.material.color = new Color(1.0f, 0.6796875f, 0.89453125f, 0.4f);
                if (_particles.isPlaying)
                    _particles.Stop();
            }
        }
	}
}
