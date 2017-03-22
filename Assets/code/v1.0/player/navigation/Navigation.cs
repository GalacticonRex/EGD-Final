using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour {
    public float Velocity;
    public float TurnRate = 8.0f;
    public Rigidbody Body;

    private PlayerMove _player_move;
    private float _linear_velocity;
    private float _angular_velocity;
    private Vector3 _angular_axis;
    private Vector3 _destination;

    public void SetDestination(Vector3 v)
    {
        _destination = v;
    }

    private void Start()
    {
        _player_move = GetComponent<PlayerMove>();
    }
    private void Update()
    {
        if ( !_player_move.HasDestination() )
        {

        }
        else if ( Vector3.Distance(transform.position, _destination) > Velocity / 10.0f )
        {
            Vector3 dir = (_destination - transform.position).normalized;
            float dist = Vector3.Distance(_destination, transform.position);
            Vector3 axis = Vector3.Cross(transform.forward, dir);
            Quaternion toward = Quaternion.LookRotation(dir);
            float angle = Quaternion.Angle(transform.rotation, toward);

            _angular_velocity = angle / TurnRate;
            _angular_axis = Vector3.Lerp(_angular_axis, axis, 0.2f);
            _linear_velocity = Velocity / (1.0f+_angular_velocity);
        }
        else
        {
            _linear_velocity = Mathf.Max(0.0f, (_linear_velocity) * 0.7f);
            _angular_velocity = Mathf.Max(0.0f, (_angular_velocity) * 0.7f);
        }

        Body.MovePosition(transform.position + transform.forward * _linear_velocity * Time.deltaTime);
        Body.MoveRotation(transform.rotation * Quaternion.AngleAxis(_angular_velocity * Time.deltaTime, _angular_axis));

        Ray r = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if ( Physics.Raycast(r, out hit, 20.0f) )
        {
            //hit.normal
        }
    }
}
