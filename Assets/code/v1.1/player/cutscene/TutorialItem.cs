using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TutorialItem : MonoBehaviour {

        public bool Breakpoint = false;

        public CameraView Target = null;
        public float TransitionTime = 0.0f;

        public string TextToShow = "";
        public float Duration = -1.0f;

        public UnityEngine.Events.UnityEvent OnEnter;

    }
}