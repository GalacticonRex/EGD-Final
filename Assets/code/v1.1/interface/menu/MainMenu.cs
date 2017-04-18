using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastStar
{
    public class MainMenu : MonoBehaviour
    {

        public void Play()
        {
            SceneManager.LoadScene("homebase");
        }
        public void Exit()
        {
            Application.Quit();
        }
        public void ArtifactTest()
        {
            SceneManager.LoadScene("dialogue_test");
        }

    }
}