using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MonoBehaviour {
    public string NextScene;
    private Coroutine _coroutine;

	public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
    }
    private IEnumerator LaunchNewScene(float wait)
    {
        yield return new WaitForSeconds(wait);
        LoadNextScene();
    }
    private void Start()
    {
        _coroutine = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            _coroutine = StartCoroutine(LaunchNewScene(2.0f));
        else
            Destroy(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
