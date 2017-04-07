using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class HideObjects : MonoBehaviour
    {
        public bool Hide;
        public GameObject[] HideThese;

        public void SwapElements()
        {
            Hide = !Hide;
            foreach (GameObject go in HideThese)
            {
                go.SetActive(!Hide);
            }
        }
        public void ShowElements()
        {
            Hide = false;
            foreach (GameObject go in HideThese)
            {
                go.SetActive(!Hide);
            }
        }
        public void HideElements()
        {
            Hide = true;
            foreach (GameObject go in HideThese)
            {
                go.SetActive(!Hide);
            }
        }

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