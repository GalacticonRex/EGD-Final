using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscSpawner : MonoBehaviour {

    public GameObject Spawnable;
    public int MinCount;
    public int MaxCount;

    void Start () {
        int count = Random.Range(MinCount, MaxCount);
        for ( int i=0;i<count;i++ )
        {
            Instantiate(Spawnable);
        }
	}
}
