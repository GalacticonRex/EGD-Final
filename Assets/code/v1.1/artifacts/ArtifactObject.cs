using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class Artifact
    {
        private string _name;
        private string _data;

        public Artifact(string n, string d)
        {
            _name = n;
            _data = d;
        }

        public string Name()
        {
            return _name;
        }
        public string Source()
        {
            return _data;
        }
    }
    public class ArtifactObject : MonoBehaviour
    {
        public string Name;
        public string Source;
        public bool Extracting;

        private CameraSystem _player;
        private Renderer[] _renderer;
        private Collider _collider;

        private Artifact _artf;

        public Artifact ArtfObject
        {
            get
            {
                return _artf;
            }
        }

        public void PickUp(DroneAI drone)
        {
            Vector3 dif = drone.transform.position - transform.position;

            transform.SetParent(drone.transform);
            transform.localPosition = dif;

            drone.AddArtifact(_artf);

            // Destroy Colliders
            Collider[] collids = GetComponentsInChildren<Collider>();
            foreach (Collider collid in collids)
            {
                Destroy(collid);
            }

            // Destroy Caption Text
            CaptionText cap = GetComponent<CaptionText>();
            Destroy(cap);

            // Destroy Caption
            Canvas canv = GetComponentInChildren<Canvas>();
            if (canv != null)
                Destroy(canv.gameObject);
        }

        private void Start()
        {
            _player = FindObjectOfType<CameraSystem>();
            _renderer = GetComponentsInChildren<Renderer>();
            _collider = GetComponent<Collider>();

            if (_renderer == null || _collider == null)
                Destroy(this);

            _artf = new Artifact(Name, Source);
        }

    }
}