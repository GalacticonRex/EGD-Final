using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class SpawnAsteroids : MonoBehaviour
    {
        public AsteroidMeshes Source;
        public GameObject CaptionSrc;
        public float Radius;
        public int[] Clustering;
        public int Maximum;

        private Vector3 GeneratePosition(float radius)
        {
            Vector2 v = Random.insideUnitCircle * radius;
            float yval;
            if (Random.value < 0.5f)
            {
                yval = Random.Range(8.0f, 12.0f);
            }
            else
            {
                yval = Random.Range(-12.0f, -8.0f);
            }
            return new Vector3(v.x, yval, v.y);
        }
        private void GenerateTransform(Vector3 location, float radius, float minS, float maxS, Transform t)
        {
            float v = (Random.value * Random.value) * (maxS - minS) + minS;

            t.localPosition = location + GeneratePosition(radius);
            t.localRotation = Quaternion.LookRotation(Random.onUnitSphere, Random.onUnitSphere);
            t.localScale = new Vector3(v, v, v);
        }
        private void GenerateCluster(Vector3 location, float radius, int count, int depth)
        {
            if (depth == 0)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject go = new GameObject();
                    Asteroid a = go.AddComponent<Asteroid>();
                    a.Source = Source;

                    SphereCollider col = go.AddComponent<SphereCollider>();
                    col.radius = Source.MaxPerturb;

                    Rigidbody rb = go.AddComponent<Rigidbody>();

                    go.transform.parent = transform;

                    if (Random.value < Source.OreProbablilty / 100.0f)
                    {
                        a.Matl = Source.GetSpecialMaterial();

                        OreDeposit o = go.AddComponent<OreDeposit>();
                        o.InitialAmount = (Random.value * Random.value) * (Source.OreMaximum - Source.OreMinimum) + Source.OreMinimum;

                        GameObject ore = Instantiate(Source.OreDesposits);
                        ore.transform.parent = go.transform;

                        GenerateTransform(location, radius, 4.0f, 8.0f, go.transform);

                        Caption c = go.AddComponent<Caption>();
                        c.Source = CaptionSrc;
                        c.TextData = "Ore Deposit of " + Mathf.RoundToInt(o.InitialAmount).ToString();
                        c.Radius = 2.0f * go.transform.localScale.x;
                    }
                    else
                    {
                        a.Matl = Source.GetNormalMaterial();
                        GenerateTransform(location, radius, 1.0f, 6.0f, go.transform);
                    }

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