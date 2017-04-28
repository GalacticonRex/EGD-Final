using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBot : MonoBehaviour {

    public UnityEngine.UI.Text Target;

    private bool _completed = true;
    private int _index;
    private string _current;
    private static Queue<string> _data = new Queue<string>();
    private static DialogueBot _my_bot;

    public static void Push(string data)
    {
        _data.Enqueue(data);
        _my_bot.gameObject.SetActive(true);
    }
    public static bool IsComplete()
    {
        return _my_bot.LocalIsComplete();
    }
    public static void ForceComplete()
    {
        _my_bot.LocalForceComplete();
    }

    public bool LocalIsComplete()
    {
        return _completed;
    }
    public void LocalForceComplete()
    {
        StopAllCoroutines();
        Target.text = _current;
        _index = 0;
        _current = "";
        _completed = true;
    }

    private IEnumerator PrintToScreen(string current)
    {
        _completed = false;
        _current = current;
        Target.text = "";
        for (_index = 0; _index <= current.Length; _index++)
        {
            yield return new WaitForEndOfFrame();
            Target.text = _current.Substring(0, _index);
        }
        _completed = true;
    }

    private bool CheckForDialogue()
    {
        if (_completed && _data.Count > 0)
        {
            PrintToScreen(_data.Dequeue());
            return true;
        }
        return false;
    }

    private void Awake()
    {
        _my_bot = this;
        if ( !CheckForDialogue() )
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (!CheckForDialogue())
        {
            gameObject.SetActive(false);
        }
    }
}
