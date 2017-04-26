using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class AsteroidGenerator : MonoBehaviour { 
        public GameObject Source;
        public GameObject SourceWithOre;
        public float TargetDensity = 0.2f;

        public float MinDensity;
        public float MaxDensity;

        public float[] SizeProbability;
        public float[] MinAsteroidSize;
        public float[] MaxAsteroidSize;

        public float[] OreProbability;
        public float[] OreMinimum;
        public float[] OreMaximum;

        private System.Random _random;
        private bool _complete = false;
        private GameObject _last_attempted_to_place;
        private float _full_volume;
        private float _asteroid_volume;

        private bool PlaceAsteroid(Vector3 position, GameObject obj)
        {
            SphereCollider collidA = obj.GetComponent<SphereCollider>();
            float radiusA = collidA.radius * obj.transform.localScale.x;

            foreach ( Transform t in transform )
            {
                SphereCollider collidB = t.GetComponent<SphereCollider>();
                float distance = Vector3.Distance(t.position - collidB.center, obj.transform.position - collidA.center);
                float radiusB = collidB.radius * t.localScale.x;

                if (distance < radiusA + radiusB)
                    return false;
            }
            obj.transform.localPosition = position;
            return true;
        }
        private GameObject MakeAsteroid()
        {
            int index = 0;
            float rand_value = (float)_random.NextDouble();
            while (rand_value > SizeProbability[index])
            {
                rand_value -= SizeProbability[index];
                index++;
            }

            GameObject go;
            if ((float)_random.NextDouble() < OreProbability[index] / 100.0f)
            {
                go = Instantiate(SourceWithOre);
                go.GetComponent<AsteroidObject>().RandomGenerator = _random;

                OreDeposit ore = go.GetComponent<OreDeposit>();
                ore.InitialAmount = ((float)_random.NextDouble() * (float)_random.NextDouble()) * (OreMaximum[index] - OreMinimum[index]) + OreMinimum[index];
                ore.SetAmount(ore.InitialAmount);
            }
            else
            {
                go = Instantiate(Source);
                go.GetComponent<AsteroidObject>().RandomGenerator = _random;
            }

            float scale;
            while ((scale = ((float)_random.NextDouble() * (float)_random.NextDouble()) * (MaxAsteroidSize[index] - MinAsteroidSize[index]) + MinAsteroidSize[index]) + _asteroid_volume > _full_volume) ;
            go.transform.localScale = new Vector3(scale, scale, scale);
            go.transform.localRotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);

            AsteroidObject obj = go.GetComponent<AsteroidObject>();
            obj.Start();

            go.SetActive(false);

            return go;
        }

        private IEnumerator GenerateCell()
        {
            AsteroidGridCell cell = GetComponent<AsteroidGridCell>();
            while (!_complete)
            {
                yield return null;

                if (_last_attempted_to_place == null)
                    _last_attempted_to_place = MakeAsteroid();

                for (int i = 0; i < 10; i++)
                {
                    SphereCollider collid = _last_attempted_to_place.GetComponent<SphereCollider>();
                    float radius = collid.radius * collid.transform.localScale.x;
                    Vector3 offset = collid.center * collid.transform.localScale.x;

                    Vector3 position = new Vector3(
                        Random.Range(-cell.Size / 2 + radius - offset.x, cell.Size / 2 - radius + offset.x),
                        Random.Range(-cell.Size / 2 + radius - offset.y, cell.Size / 2 - radius + offset.y),
                        Random.Range(-cell.Size / 2 + radius - offset.z, cell.Size / 2 - radius + offset.z)
                        );

                    if (PlaceAsteroid(position, _last_attempted_to_place))
                    {
                        float volume = 4.0f / 3.0f * Mathf.PI * (radius * radius * radius);

                        _last_attempted_to_place.transform.SetParent(transform);
                        _last_attempted_to_place.SetActive(true);
                        _last_attempted_to_place.transform.localPosition = position;

                        _asteroid_volume += volume;

                        _last_attempted_to_place = null;

                        break;
                    }
                }

                if (_asteroid_volume / _full_volume >= TargetDensity)
                {
                    _complete = true;
                }
            }
            print("Completed creating all " + transform.childCount + " asteroids!");
        }

        private void GenerateCell_SinglePass()
        {
            AsteroidGridCell cell = GetComponent<AsteroidGridCell>();
            while (!_complete)
            {
                if (_last_attempted_to_place == null)
                    _last_attempted_to_place = MakeAsteroid();

                for (int i = 0; i < 10; i++)
                {
                    SphereCollider collid = _last_attempted_to_place.GetComponent<SphereCollider>();
                    float radius = collid.radius * collid.transform.localScale.x;
                    Vector3 offset = collid.center * collid.transform.localScale.x;

                    Vector3 position = new Vector3(
                        Random.Range(-cell.Size / 2 + radius - offset.x, cell.Size / 2 - radius + offset.x),
                        Random.Range(-cell.Size / 2 + radius - offset.y, cell.Size / 2 - radius + offset.y),
                        Random.Range(-cell.Size / 2 + radius - offset.z, cell.Size / 2 - radius + offset.z)
                        );

                    if (PlaceAsteroid(position, _last_attempted_to_place))
                    {
                        float volume = 4.0f / 3.0f * Mathf.PI * (radius * radius * radius);

                        _last_attempted_to_place.transform.SetParent(transform);
                        _last_attempted_to_place.SetActive(true);
                        _last_attempted_to_place.transform.localPosition = position;

                        _asteroid_volume += volume;

                        _last_attempted_to_place = null;

                        break;
                    }
                }

                if (_asteroid_volume / _full_volume >= TargetDensity)
                {
                    _complete = true;
                }
            }
            print("Completed creating all " + transform.childCount + " asteroids!");
        }

        public void ForceCompletion()
        {
            _complete = true;
        }
        
        public void Init()
        {
            AsteroidGridCell cell = GetComponent<AsteroidGridCell>();
            int value = cell.Location[0] ^ cell.Location[1] ^ cell.Location[2];
            _random = new System.Random(UniverseMap.GetSeed() ^ value);
            _full_volume = cell.Size * cell.Size * cell.Size;

            float rvalue = cell.ParentGrid.GetRandomValue();
            TargetDensity = ((rvalue * rvalue * rvalue * rvalue) * (MaxDensity + MinDensity) - MinDensity);

            StartCoroutine(GenerateCell());
        }
    }
}