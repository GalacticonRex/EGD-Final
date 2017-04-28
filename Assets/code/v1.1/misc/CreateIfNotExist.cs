using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CreateIfNotExist : MonoBehaviour
    {
        public GameObject CreateIfNone;

        private void Awake()
        {
            AssetDatabase go = FindObjectOfType<AssetDatabase>();
            if (go == null)
            {
                DontDestroyOnLoad(Instantiate(CreateIfNone));
            }
        }
    }
}