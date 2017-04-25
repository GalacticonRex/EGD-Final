using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastStar {
    public class DeathWhenClose : MonoBehaviour {

        public GameObject Track;
        public Color AffectColor;
        public float MinDistance;
        public float MaxDistance;

        private Player _player;

        private void Start()
        {
            _player = Track.GetComponent<Player>();
        }
        private void Update()
        {
            float distance = Vector3.Distance(Track.transform.position, transform.position);
            if (distance < MinDistance)
            {
                SceneManager.LoadScene("lose");
            }
            if (distance < MaxDistance)
            {
                float ratio = (distance - MinDistance) / (MaxDistance - MinDistance);

            }
        }
    }
}