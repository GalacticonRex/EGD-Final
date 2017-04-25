using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class SelectMesh : MonoBehaviour
    {
        void Start()
        {
            OreDeposit ore = GetComponent<OreDeposit>();
            if ( ore == null )
            {
                GetComponent<MeshRenderer>().sharedMaterial = Asteroids.GetNormalMaterial();
            }
            else
            {
                GetComponent<MeshRenderer>().sharedMaterial = Asteroids.GetSpecialMaterial();
            }
        }
    }
}