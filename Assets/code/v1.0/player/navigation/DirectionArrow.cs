using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour {

    public Transform Rotate;
    public Transform Scale;

    private PlayerMove _player_move;

    private void Start()
    {
        _player_move = FindObjectOfType<PlayerMove>();
    }

	private void Update () {
        Vector3 direction = _player_move.GetDestination() - transform.position;
        float log = Mathf.Log(Mathf.Max(0.0f, direction.magnitude - 10.0f));
        float mag = Mathf.Min(log / 25.0f, 0.5f);
        if (mag > 0)
        {
            Rotate.rotation = Quaternion.LookRotation(direction);
            Scale.localScale = new Vector3(mag, mag, mag);
        }
	}
}
