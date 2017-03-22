using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShips : MonoBehaviour {

    public GameObject Object;
    public int HowMany = 10;
    public Vector3 LowerBound = new Vector3(-1, -1, -1);
    public Vector3 UpperBound = new Vector3(1, 1, 1);

    // Use this for initialization
    void Start () {
	    for ( int i=0;i<HowMany;i++ )
        {
            GameObject j = Instantiate(Object);
            float x = Random.Range(Mathf.Sign(LowerBound.x) * Mathf.Sqrt(Mathf.Abs(LowerBound.x)),
                                    Mathf.Sign(UpperBound.x) * Mathf.Sqrt(Mathf.Abs(UpperBound.x))) *
                      Random.Range(Mathf.Sign(LowerBound.x) * Mathf.Sqrt(Mathf.Abs(LowerBound.x)),
                                    Mathf.Sign(UpperBound.x) * Mathf.Sqrt(Mathf.Abs(UpperBound.x)));
            float y = Random.Range(Mathf.Sign(LowerBound.y) * Mathf.Sqrt(Mathf.Abs(LowerBound.y)),
                                    Mathf.Sign(UpperBound.y) * Mathf.Sqrt(Mathf.Abs(UpperBound.y))) *
                      Random.Range(Mathf.Sign(LowerBound.y) * Mathf.Sqrt(Mathf.Abs(LowerBound.y)),
                                    Mathf.Sign(UpperBound.y) * Mathf.Sqrt(Mathf.Abs(UpperBound.y)));
            float z = Random.Range(Mathf.Sign(LowerBound.z) * Mathf.Sqrt(Mathf.Abs(LowerBound.z)),
                                    Mathf.Sign(UpperBound.z) * Mathf.Sqrt(Mathf.Abs(UpperBound.z))) *
                      Random.Range(Mathf.Sign(LowerBound.z) * Mathf.Sqrt(Mathf.Abs(LowerBound.z)),
                                    Mathf.Sign(UpperBound.z) * Mathf.Sqrt(Mathf.Abs(UpperBound.z)));
            j.transform.parent = transform;
            j.transform.localPosition = new Vector3(x, y, z);
        }
	}
}
