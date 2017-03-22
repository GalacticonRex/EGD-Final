using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBeacons : MonoBehaviour {

    public Generator Gen;
    public GameObject Object;
    public Vector3 LowerBound;
    public Vector3 UpperBound;
    
    Vector3 RandomPosition()
    {
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
        return new Vector3(x, y, z);
    }

    void CreateShip(string txt)
    {
        GameObject j = Instantiate(Object);
        j.transform.position = RandomPosition();
        Beacon b = j.GetComponent<Beacon>();
        b.Text = txt;
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        Environment env = Gen.currentEnvironment();
        for (int i = 0; i < env.actorCount(); i++)
        {
            Actor a = env.getActor(i);
            CreateShip(a.describe());
            for (int k = 0; k < a.relationshipCount(); k++)
            {
                CreateShip(a.relationship(k).describe());
            }
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(LateStart());
    }
}
