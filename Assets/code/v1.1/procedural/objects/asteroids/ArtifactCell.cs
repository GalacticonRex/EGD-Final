using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ArtifactCell : MonoBehaviour
    {
        public GameObject Source;

        public int MinCount = 100;
        public int MaxCount = 1000;

        private int _count;
        private System.Random _random;

        public void Init()
        {
            AsteroidGridCell cell = GetComponent<AsteroidGridCell>();

            int value = cell.Location[0] ^ cell.Location[1] ^ cell.Location[2];
            _random = new System.Random(UniverseMap.GetSeed() ^ value);

            _count = _random.Next(MinCount, MaxCount);
            for (int i=0;i<_count;i++)
            {
                AssetDatabase.ArtifactOutput output = cell.ParentGrid.GetRandomArtifact(_random);
                GameObject obj = Instantiate(Source);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(
                    Random.Range(-cell.ParentGrid.GridSize / 2, cell.ParentGrid.GridSize / 2),
                    Random.Range(-cell.ParentGrid.GridSize / 2, cell.ParentGrid.GridSize / 2),
                    Random.Range(-cell.ParentGrid.GridSize / 2, cell.ParentGrid.GridSize / 2) );
                ArtifactObject artf = obj.GetComponent<ArtifactObject>();
                artf.Name = output.name;
                artf.Source = "Dated to " + output.date.ToString() + "\r\n...\r\n" + output.data;
            }
        }
    }
}