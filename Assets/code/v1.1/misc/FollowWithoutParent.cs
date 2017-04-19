using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithoutParent : MonoBehaviour {

    public GameObject ToFollow;
	void LateUpdate () {
        transform.position = ToFollow.transform.position;
    }

}
