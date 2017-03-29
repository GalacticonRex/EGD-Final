using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TechSpawner : MonoBehaviour
    {
        public GameObject[] Technologies;
        public float Radius;
        public float Count;

        void Start()
        {
            for (int i = 0; i < Count; i++)
            {
                Vector2 pos2d = Random.insideUnitCircle * Radius;
                Vector3 position = new Vector3(pos2d.x, Random.Range(-4, 4), pos2d.y);
                GameObject go = Instantiate(Technologies[Random.Range(0, Technologies.Length)]);
                go.transform.parent = transform;
                go.transform.localPosition = position;
            }
        }
    }
}