using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastStar
{
    public class WormholeParticles : MonoBehaviour
    {
        public float TimeToWait = 2.0f;
        public int MaxParticles = 1000;
        public string NextScene;

        private ParticleSystem _sys;
        private float _particle_count;
        private bool _hit;

        private void Start()
        {
            _sys = GetComponentInChildren<ParticleSystem>();
            _particle_count = 0.0f;
            _hit = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                _hit = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                _hit = false;
            }
        }
        private void Update()
        {
            if (_hit && _particle_count < MaxParticles)
            {
                _particle_count += MaxParticles / TimeToWait * Time.deltaTime;

                ParticleSystem.EmissionModule m = _sys.emission;
                m.rateOverTime = new ParticleSystem.MinMaxCurve(_particle_count);

                if (_particle_count >= MaxParticles)
                {
                    SceneManager.LoadScene(NextScene);
                }
            }
            else
            {
                _particle_count -= MaxParticles / TimeToWait * Time.deltaTime;

                if (_particle_count < 0)
                {
                    _particle_count = 0;
                    _hit = false;
                }

                ParticleSystem.EmissionModule m = _sys.emission;
                m.rateOverTime = new ParticleSystem.MinMaxCurve(_particle_count);
            }

        }
    }
}