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

    public GameObject Create(Type t)
    {
        switch( t )
        {
            case Type.Core:
                return CreateCore();
            case Type.Arm:
                return CreateArm();
            case Type.Endpoint:
                return CreateEndpoint();
            case Type.ArmOrCore:
                return CreateArmOrCore();
            case Type.ArmOrEndpoint:
                return CreateArmOrEndpoint();
            case Type.CoreOrEndpoint:
                return CreateCoreOrEndpoint();
            case Type.Any:
                return CreateAny();
            default:
                return null;
        }
    }

    public GameObject CreateArm()
    {
        return Instantiate(_arms[Random.Range(0, _arms.Count)]);
    }
    public GameObject CreateCore()
    {
        return Instantiate(_cores[Random.Range(0, _cores.Count)]);
    }
    public GameObject CreateEndpoint()
    {
        return Instantiate(_endpoints[Random.Range(0, _endpoints.Count)]);
    }

    public GameObject CreateArmOrEndpoint()
    {
        if (Random.value < ArmProbability / (EndpointProbability + ArmProbability))
            return CreateArm();

        else
            return CreateEndpoint();
    }
    public GameObject CreateArmOrCore()
    {
        if (Random.value < ArmProbability / (CoreProbability + ArmProbability))
            return CreateArm();

        else
            return CreateCore();
    }
    public GameObject CreateCoreOrEndpoint()
    {
        if (Random.value < CoreProbability / (CoreProbability + EndpointProbability))
            return CreateCore();

        else
            return CreateEndpoint();
    }
    public GameObject CreateAny()
    {
        float total = CoreProbability + EndpointProbability + ArmProbability;
        float data = Random.value;

        if (data < EndpointProbability / total)
            return CreateEndpoint();

        else if (data < ArmProbability / total)
            return CreateArm();

        else
            return CreateCore();
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
