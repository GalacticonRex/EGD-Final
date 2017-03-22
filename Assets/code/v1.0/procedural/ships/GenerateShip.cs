using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateShip : MonoBehaviour {

    public GameObject[] Fronts;
    public GameObject[] Rears;

    private GameObject _front;
    private GameObject _rear;

    // Use this for initialization
    void Start () {
        _front = Instantiate(Fronts[Random.Range(0, Fronts.Length)]);
        _rear = Instantiate(Rears[Random.Range(0, Rears.Length)]);

        _front.transform.parent = transform;
        _rear.transform.parent = transform;

        _front.transform.localPosition = new Vector3();
        _front.transform.localRotation = new Quaternion();

        _rear.transform.localPosition = new Vector3();
        _rear.transform.localRotation = new Quaternion();

        Rigidbody frontr = _front.AddComponent<Rigidbody>();
        frontr.useGravity = false;
        frontr.drag = 0.01f;

        Rigidbody rearr = _rear.AddComponent<Rigidbody>();
        rearr.useGravity = false;
        rearr.drag = 0.01f;

        FixedJoint frontj = _front.AddComponent<FixedJoint>();
        frontj.connectedAnchor = new Vector3();
        frontj.connectedBody = rearr;
    }
}
