using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class RealtimeAsteroidCluster : MonoBehaviour
    {
        public Transform OriginPoint;
        public ObjectManager Manager;
        public GameObject OreDepositSrc;

        public float UpdateTime = 1.0f;
        public float TargetFog = 0.02f;

        public float OreProbability = 1.0f;
        public float OreMinimum = 50.0f;
        public float OreMaximum = 1000.0f;

        public float MinRadius = 100.0f;
        public float MaxRadius = 1000.0f;
        public float Increment = 15.0f;

        public float MinAsteroidSize = 4.0f;
        public float MaxAsteroidSize = 12.0f;

        private float _actual_fog;
        private Player _player;
        private float _current_radius;
        private Vector3 _last_position;
        private Vector3 _directional;

        private Vector3 GeneratePosition()
        {
            return OriginPoint.position + (Random.onUnitSphere + _directional).normalized * _current_radius;
        }
        private void GenerateTransform(Transform t)
        {
            float v = (Random.value * Random.value) * (MaxAsteroidSize - MinAsteroidSize) + MinAsteroidSize;

            t.position = GeneratePosition();
            t.rotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
            t.localScale = new Vector3(v, v, v);
        }
        private GameObject MakeAsteroid()
        {
            GameObject go = Manager.Pop(2.0f * MaxRadius);

            go.transform.parent = transform;

            if (Random.value < OreProbability / 100.0f)
            {
                go.GetComponent<MeshRenderer>().sharedMaterial = Asteroids.GetSpecialMaterial();

                OreDeposit ore = go.GetComponent<OreDeposit>();
                if (ore == null)
                {
                    ore = go.AddComponent<OreDeposit>();
                    ore.InitialAmount = (Random.value * Random.value) * (OreMaximum - OreMinimum) + OreMinimum;
                }

                GenerateTransform(go.transform);

                CaptionText c = go.GetComponentInChildren<CaptionText>();
                if (c == null)
                {
                    GameObject ore_go = Instantiate(OreDepositSrc);
                    ore_go.transform.parent = go.transform;
                    ore_go.transform.localPosition = new Vector3();
                    c = ore_go.GetComponent<CaptionText>();
                }
                c.TextData = "Ore Deposit of " + Mathf.RoundToInt(ore.InitialAmount).ToString();
                c.Radius = 4.0f * go.transform.localScale.x;
            }
            else
            {
                go.GetComponent<MeshRenderer>().sharedMaterial = Asteroids.GetNormalMaterial();
                GenerateTransform(go.transform);
                CaptionText c = GetComponentInChildren<CaptionText>();
                if (c != null)
                {
                    Destroy(c.gameObject);
                }
            }

            SphereCollider sp = go.GetComponent<SphereCollider>();
            sp.radius = 1.3f;

            return go;
        }
        private IEnumerator InitializeAsteroids()
        {
            while (_current_radius < MaxRadius)
            {
                yield return new WaitForSeconds(1.0f);

                GameObject go = MakeAsteroid();
                go.name = "Asteroid " + go.transform.position.ToString();

                _current_radius = Mathf.Min(MaxRadius, _current_radius + Increment);
            }
        }
        private IEnumerator GenerateAsteroids()
        {
            while (true)
            {
                if (_last_position != OriginPoint.position)
                {
                    _current_radius = Mathf.Max(0.0f, _current_radius - Vector3.Distance(_last_position, OriginPoint.position));
                    StartCoroutine(InitializeAsteroids());
                }
                _directional = (OriginPoint.position - _last_position).normalized;
                _last_position = OriginPoint.position;
                yield return new WaitForSeconds(UpdateTime);
            }
        }
        private IEnumerator ReduceFog()
        {
            while (_actual_fog > TargetFog)
            {
                yield return null;
                _actual_fog = Mathf.Max(TargetFog, _actual_fog * 0.9f);
            }
        }
        private void Start()
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;
            _actual_fog = 1.0f;

            _player = FindObjectOfType<Player>();
            _current_radius = MinRadius;
            _last_position = OriginPoint.position;

            StartCoroutine(InitializeAsteroids());
            StartCoroutine(GenerateAsteroids());
            StartCoroutine(ReduceFog());
        }
        private void Update()
        {
            RenderSettings.fogDensity = _actual_fog * _player.cameraSystem.interpolation;
        }

    }
}