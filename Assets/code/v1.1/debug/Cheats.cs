using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastStar
{
    public class Cheats : MonoBehaviour
    {
        public KeyCode FillEnergy = KeyCode.None;
        public KeyCode CreateOre = KeyCode.None;
        public KeyCode ReturnToMainMenu = KeyCode.None;

        private ResourceManager _resources;

        private void Start()
        {
            _resources = FindObjectOfType<ResourceManager>();
        }
        void Update()
        {
            if (_resources != null && Input.GetKeyDown(FillEnergy))
            {
                _resources.RequestEnergy(-100.0f);
            }
            if (_resources != null && Input.GetKeyDown(CreateOre))
            {
                _resources.RequestStorage(100.0f);
            }
            if ( Input.GetKeyDown(ReturnToMainMenu) )
            {
                SceneManager.LoadScene("title_screen");
            }
        }
    }
}