using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingScreen : MonoBehaviour {

    public GameObject Construction;

	public void ShowConstructionMenu()
    {
        Construction.SetActive(true);
    }

    private void Start()
    {
        Construction.SetActive(false);
    }
}
