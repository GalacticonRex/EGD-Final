using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToColorScreen : MonoBehaviour {

    public UnityEngine.UI.Image Image;
    public KeyCode CancelAnimation = KeyCode.Escape;

    private string _target;
    private bool _transporting = false;
    private bool _fading = false;
    private float _duration;

    private IEnumerator Fade(float delay, float duration)
    {
        _transporting = true;
        _duration = duration;
        yield return new WaitForSecondsRealtime(delay);
        _fading = true;
        yield return new WaitForSecondsRealtime(duration);
        SceneManager.LoadScene(_target);
    }

    public void Push(Color col, string target, float delay, float duration)
    {
        if (_transporting)
            return;

        col.a = 0;
        Image.color = col;
        _target = target;
        StartCoroutine(Fade(delay, duration));
    }
    public void Set(Color col)
    {
        Image.color = col;
    }

    private void Update()
    {
        if (_transporting && Input.GetKeyDown(CancelAnimation))
        {
            StopAllCoroutines();
            SceneManager.LoadScene(_target);
        }
        if ( _fading )
        {
            Color col = Image.color;
            col.a += Time.unscaledDeltaTime / _duration;
            Image.color = col;
        }
    }

}
