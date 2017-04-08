using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneUIBinding : MonoBehaviour {

    public UnityEngine.UI.Text AvailableDrones;
    public UnityEngine.UI.Text MaxDrones;

    public int availableDrones
    {
        set { AvailableDrones.text = value.ToString(); }
    }
    public int maxDrones
    {
        set { MaxDrones.text = value.ToString(); }
    }

}
