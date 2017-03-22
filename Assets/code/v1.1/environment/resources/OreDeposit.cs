using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDeposit : MonoBehaviour {
    public float InitialAmount;

    private float _total;

    public float Remaining()
    {
        return _total;
    }
    public float Extract( float amount )
    {
        if ( _total <= amount )
        {
            Asteroid parent = GetComponentInParent<Asteroid>();
            MeshRenderer mr = parent.GetComponent<MeshRenderer>();
            mr.sharedMaterial = parent.Source.GetNormalMaterial();

            Caption cap = GetComponentInParent<Caption>();
            Destroy(cap);

            foreach ( Transform t in transform )
                Destroy(t.gameObject);
            Destroy(this);

            return _total;
        }
        else
        {
            _total -= amount;
            return amount;
        }
    }

    private void Start()
    {
        _total = InitialAmount;
    }
}
