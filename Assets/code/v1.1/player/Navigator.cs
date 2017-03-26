using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour {

    private CameraSystem _camera;
    private ObjectSeletor _selection;
    private ResourceManager _resources;

    public GameObject ShipComponent;
    public float Velocity;
    public LocationSelector Selector;
    public LocationSelector Destination;

    private Rigidbody _body;
    private bool _has_destination;
    private bool _navigation_initiated_in_normal_view;
    private bool _finding_location;

    public void Arrived()
    {
        Destination.Hide();
        _has_destination = false;
    }
    public void SetDestination(Vector3 dst)
    {
        Destination.SetLocation(dst);
        _has_destination = true;
    }
    public Vector3 GetDestination()
    {
        return (_has_destination) ? Destination.GetLocation() : transform.position;
    }
    public bool HasDestination()
    {
        return _has_destination;
    }

    private bool Move(Vector3 m)
    {
        float dist = Vector3.Distance(transform.position, m);
        if (_resources.RequestEnergy(dist / 100.0f))
        {
            _body.MovePosition(m);
            return true;
        }
        return false;
    }

    private void Start () {
        _body = GetComponent<Rigidbody>();
        _selection = GetComponent<ObjectSeletor>();
        _camera = GetComponent<CameraSystem>();
        _resources = GetComponent<ResourceManager>();
        _navigation_initiated_in_normal_view = false;
    }

    private void Update () {
        Selector.SetVisibility((_camera.interpolation < 0.1f));
        Destination.SetVisibility((_has_destination && _camera.interpolation < 0.1f));

        Selector.FindLocation();

        if (Input.GetMouseButtonDown(0) && !_selection.hovering)
            _finding_location = true;
        if (Input.GetMouseButtonUp(0))
            _finding_location = false;

        if (_finding_location)
        {
            Vector3 nloc = Selector.GetLocation();
            if (Vector3.Distance(transform.position, nloc) > 3.0f)
            {
                if (_camera.scanMode)
                    Destination.Show();
                else
                    _navigation_initiated_in_normal_view = true;
                SetDestination(nloc);
            }
        }
        else if (!Input.GetMouseButton(0) && _navigation_initiated_in_normal_view)
        {
            _navigation_initiated_in_normal_view = false;
            Arrived();
        }

        if (_has_destination)
        {
            Vector3 destination = GetDestination();
            Vector3 distance = destination - transform.position;
            Vector3 direction = distance.normalized;
            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            ShipComponent.transform.rotation = target;

            if (distance.magnitude > 3.0f)
            {
                Move(transform.position + ShipComponent.transform.forward * Velocity * Time.deltaTime);
            }
            else
            {
                if ( Move(destination) )
                    Arrived();
            }
        }
    }
}
