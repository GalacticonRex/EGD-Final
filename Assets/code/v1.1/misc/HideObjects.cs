using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class HideObjects : MonoBehaviour
    {
        public bool Hide;
        public GameObject[] HideThese;

        void Start()
        {
            if (Hide)
            {
                foreach (GameObject go in HideThese)
                {
                    go.SetActive(false);
                }
            }
        }
    }
}