using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class Stargun : MonoBehaviour {

        public CameraView VictoryView;
        public GameObject PlayerType;
        public GameObject BioType;
        public GameObject BugType;
        public GameObject FishType;

        public static bool PlayerIsFound = false;
        public static bool BioIsFound = false;
        public static bool BugIsFound = false;
        public static bool FishIsFound = false;

        public static bool StargunIsReady()
        {
            return PlayerIsFound && BioIsFound && BugIsFound && FishIsFound;
        }
        private void Awake() {
            if (!PlayerIsFound)
                PlayerType.SetActive(false);
            if (!BioIsFound)
                BioType.SetActive(false);
            if (!BugIsFound)
                BugType.SetActive(false);
            if (!FishIsFound)
                FishType.SetActive(false);
            if (StargunIsReady())
            {
                CameraSystem sys = FindObjectOfType<CameraSystem>();
                sys.InitialView = VictoryView;
            }
        }
        private void Update()
        {
            if (PlayerIsFound)
                PlayerType.SetActive(true);
        }

    }
}