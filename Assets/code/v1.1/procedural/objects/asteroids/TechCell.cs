using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TechCell : MonoBehaviour
    {

        public GameObject[] Source;
        public float[] Probability;

        public int MinCount = 100;
        public int MaxCount = 1000;

        private int _count;
        private System.Random _random;

        private IEnumerator GenerateCell()
        {
            AsteroidGridCell cell = GetComponent<AsteroidGridCell>();
            for ( int i=0;i<_count;i++ )
            {
                float random = (float)_random.NextDouble();
                int selected = 0;
                for ( int j=0;j<Probability.Length;j++ )
                {
                    random -= Probability[j];
                    if ( random <= 0 )
                    {
                        selected = j;
                        break;
                    }
                }
                GameObject gen = Instantiate(Source[selected]);
                gen.transform.SetParent(transform);
                gen.transform.localPosition = new Vector3(
                    ((float)_random.NextDouble() - 0.5f) * cell.Size,
                    ((float)_random.NextDouble() - 0.5f) * cell.Size,
                    ((float)_random.NextDouble() - 0.5f) * cell.Size);

                yield return null;
            }
            print("Completed creating all " + transform.childCount + " tech objects!");
        }

        public void Init()
        {
            AsteroidGridCell cell = GetComponent<AsteroidGridCell>();

            int value = cell.Location[0] ^ cell.Location[1] ^ cell.Location[2];
            _random = new System.Random(UniverseMap.GetSeed() ^ value);
            _count = cell.ParentGrid.GetRandomIntValue(MinCount, MaxCount);

            StartCoroutine(GenerateCell());
        }
    }
}