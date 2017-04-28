using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour {
    public string TargetScene;
	public void Activate()
    {
        SceneManager.LoadScene(TargetScene);
    }
}
