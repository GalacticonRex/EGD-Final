using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour {

    public float IterationTime = 2.0f;
    public float DistanceThreshold;
    public ObjectManager Parent;

    private IEnumerator CheckDistance()
    {
        while (Vector3.Distance(transform.position, Parent.OriginPoint.position) < DistanceThreshold)
        {
            yield return new WaitForSeconds(IterationTime);
        }
        gameObject.SetActive(false);
        Parent.Push(gameObject);
    }

    public void Init() {
        StartCoroutine(CheckDistance());
	}
}
