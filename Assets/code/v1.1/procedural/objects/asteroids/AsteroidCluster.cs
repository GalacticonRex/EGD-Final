using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class AsteroidCluster : MonoBehaviour
    {
        public GameObject AsteroidSrc;
        public GameObject AsteroidOreSrc;

        public float OreProbability = 1.0f;
        public float OreMinimum = 50.0f;
        public float OreMaximum = 1000.0f;

        public float Radius;
        public int[] Clustering;
        public int Maximum;

        public float MinYRange = 16.0f;
        public float MaxYRange = 48.0f;

        public float MinAsteroidSize = 4.0f;
        public float MaxAsteroidSize = 12.0f;

        private Vector3 GeneratePosition(float radius)
        {
            Vector2 v = Random.insideUnitCircle * radius;
            float yval;
            if (Random.value < 0.5f)
            {
                yval = Random.Range(MinYRange, MaxYRange);
            }
            else
            {
                yval = Random.Range(-MaxYRange, -MinYRange);
            }
            return new Vector3(v.x, yval, v.y);
        }
        private void GenerateTransform(Vector3 location, float radius, Transform t)
        {
            float v = (Random.value * Random.value) * (MaxAsteroidSize - MinAsteroidSize) + MinAsteroidSize;

            t.localPosition = location + GeneratePosition(radius);
            t.localRotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
            t.localScale = new Vector3(v, v, v);
        }
        private GameObject MakeAsteroid(Vector3 location, float radius)
        {
            GameObject go;

            if (Random.value < OreProbability / 100.0f)
            {
                go = Instantiate(AsteroidOreSrc);
                go.transform.parent = transform;

                go.GetComponent<MeshRenderer>().sharedMaterial = Asteroids.GetSpecialMaterial();

                GenerateTransform(location, radius, go.transform);

                OreDeposit o = go.GetComponent<OreDeposit>();
                o.InitialAmount = (Random.value * Random.value) * (OreMaximum - OreMinimum) + OreMinimum;
            }
            else
            {
                go = Instantiate(AsteroidSrc);
                go.transform.parent = transform;

                go.GetComponent<MeshRenderer>().sharedMaterial = Asteroids.GetNormalMaterial();

                GenerateTransform(location, radius, go.transform);
            }

            SphereCollider sp = go.GetComponent<SphereCollider>();
            sp.radius = 1.3f;

            return go;
        }
        private void GenerateCluster(Vector3 location, float radius, int count, int depth)
        {
            if (depth == 0)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject go = MakeAsteroid(location, radius);
                    go.name = "Asteroid " + go.transform.position.ToString();
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Vector3 new_location = location + GeneratePosition(radius);
                    GenerateCluster(new_location, radius / 3, Clustering[depth - 1], depth - 1);
                }
            }
        }
        private void Start()
        {
            int asteroidsPerCluster = 1;
            for (int i = 0; i < Clustering.Length; i++)
                asteroidsPerCluster *= Clustering[i];

            int totalClusters = Maximum / asteroidsPerCluster;
            GenerateCluster(new Vector3(), Radius, totalClusters, Clustering.Length);
        }
    }
}