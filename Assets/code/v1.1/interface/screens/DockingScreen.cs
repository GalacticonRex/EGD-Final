using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingScreen : MonoBehaviour {

    public GameObject Construction;

	public void ShowConstructionMenu()
    {
        Construction.SetActive(!Construction.activeInHierarchy);
    }

    private void Start()
    {
        Construction.SetActive(false);
    }
}
