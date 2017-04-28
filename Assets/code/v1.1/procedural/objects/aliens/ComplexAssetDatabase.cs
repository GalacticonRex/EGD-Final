using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexAssetDatabase : MonoBehaviour {

    public enum Type
    {
        Core = 0x1,
        Arm = 0x2,
        Endpoint = 0x4,

        ArmOrEndpoint = 0x6,
        ArmOrCore = 0x3,
        CoreOrEndpoint = 0x5,

        Any = 0x7
    }

    public GameObject[] Assets;
    public float CoreProbability = 0.1f;
    public float ArmProbability = 0.4f;
    public float EndpointProbability = 0.5f;

    private List<GameObject> _cores = new List<GameObject>();
    private List<GameObject> _arms = new List<GameObject>();
    private List<GameObject> _endpoints = new List<GameObject>();

    public GameObject Create(System.Random r, Type t)
    {
        switch( t )
        {
            case Type.Core:
                return CreateCore(r);
            case Type.Arm:
                return CreateArm(r);
            case Type.Endpoint:
                return CreateEndpoint(r);
            case Type.ArmOrCore:
                return CreateArmOrCore(r);
            case Type.ArmOrEndpoint:
                return CreateArmOrEndpoint(r);
            case Type.CoreOrEndpoint:
                return CreateCoreOrEndpoint(r);
            case Type.Any:
                return CreateAny(r);
            default:
                return null;
        }
    }

    public GameObject CreateArm(System.Random rand)
    {
        return Instantiate(_arms[rand.Next(0, _arms.Count)]);
    }
    public GameObject CreateCore(System.Random rand)
    {
        return Instantiate(_cores[rand.Next(0, _cores.Count)]);
    }
    public GameObject CreateEndpoint(System.Random rand)
    {
        return Instantiate(_endpoints[rand.Next(0, _endpoints.Count)]);
    }

    public GameObject CreateArmOrEndpoint(System.Random rand)
    {
        if ((float)rand.NextDouble() < ArmProbability / (EndpointProbability + ArmProbability))
            return CreateArm(rand);

        else
            return CreateEndpoint(rand);
    }
    public GameObject CreateArmOrCore(System.Random rand)
    {
        if ((float)rand.NextDouble() < ArmProbability / (CoreProbability + ArmProbability))
            return CreateArm(rand);

        else
            return CreateCore(rand);
    }
    public GameObject CreateCoreOrEndpoint(System.Random rand)
    {
        if ((float)rand.NextDouble() < CoreProbability / (CoreProbability + EndpointProbability))
            return CreateCore(rand);

        else
            return CreateEndpoint(rand);
    }
    public GameObject CreateAny(System.Random rand)
    {
        float total = CoreProbability + EndpointProbability + ArmProbability;
        float data = (float)rand.NextDouble();

        if (data < EndpointProbability / total)
            return CreateEndpoint(rand);

        else if (data < ArmProbability / total)
            return CreateArm(rand);

        else
            return CreateCore(rand);
    }

    private void Awake () {
	    foreach( GameObject go in Assets )
        {
            ComplexMesh mesh = go.GetComponent<ComplexMesh>();
            if ((mesh.MeshType & Type.Core) != 0)
                _cores.Add(go);
            if ((mesh.MeshType & Type.Arm) != 0)
                _arms.Add(go);
            if ((mesh.MeshType & Type.Endpoint) != 0)
                _endpoints.Add(go);
        }
	}
}
