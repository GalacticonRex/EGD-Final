using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothNavigation : MonoBehaviour {

    public float Velocity = 10.0f;
    public float TurnRate = 0.1f;
    public Rigidbody Body;

    private PlayerMove _player_move;
    private Vector3 _destination;
    private bool _moving;
    private float _rotation;
    private Vector3 _init_pos;
    private Quaternion _init_rot;

    public bool IsMoving()
    {
        return _moving;
    }

    // Use this for initialization
    void Start () {
        _moving = false;
        _player_move = FindObjectOfType<PlayerMove>();
    }
	
	// Update is called once per frame
	void Update () {
		if (_player_move.HasDestination())
        {
            Vector3 dst = _player_move.GetDestination();
            if (!_moving || _destination != dst)
            {
                _moving = true;
                _init_pos = transform.position;
                _init_rot = transform.rotation;
                _rotation = 0.0f;
                _destination = dst;
            }

            Vector3 distance = _destination - transform.position;
            Vector3 direction = distance.normalized;
            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = target;

            if (distance.magnitude > 3.0f)
            {
                Body.MovePosition(transform.position + transform.forward * Velocity * Time.deltaTime);
            }
            else
            {
                Body.MovePosition(_destination);
                _moving = false;
                _player_move.Arrived();
            }
        }
	}
}
