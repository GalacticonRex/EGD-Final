using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDelay : MonoBehaviour {

    public float DelayTime;
    public UnityEngine.Events.UnityEvent onDelay;

	private IEnumerator DelayedStart()
    {
        yield return new WaitForSecondsRealtime(DelayTime);
        onDelay.Invoke();
    }
    private void Start () {
        StartCoroutine(DelayedStart());
	}
}
