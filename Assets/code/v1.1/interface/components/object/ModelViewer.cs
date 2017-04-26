using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ModelViewer : MonoBehaviour
    {

        public string ReferenceName;
        public GameObject Object;
        public int Width;
        public int Height;
        public Vector3 Position = new Vector3(0,0,10);
        public Vector3 Rotation = new Vector3(0,0,0);

        public Texture Result
        {
            get
            {
                return _render_texture;
            }
        }
        public Transform CameraTransform
        {
            get
            {
                return _camera.transform;
            }
        }

        private GameObject _instance;
        private RenderTexture _render_texture;
        private Camera _camera;

        private void Awake()
        {
            _render_texture = new RenderTexture(Width, Height, 16);

            _camera = gameObject.GetComponent<Camera>();
            _camera.targetTexture = _render_texture;

            _instance = Instantiate(Object);

            int target_layer = LayerMask.NameToLayer("ModelView");
            Queue<GameObject> instances = new Queue<GameObject>();
            instances.Enqueue(_instance);
            while ( instances.Count > 0 )
            {
                GameObject go = instances.Dequeue();
                go.layer = target_layer;
                foreach(Transform t in go.transform)
                {
                    instances.Enqueue(t.gameObject);
                }
            }

            _instance.transform.SetParent(transform);
            _instance.transform.position = transform.position + Position;
            _instance.transform.localRotation = Quaternion.Euler(Rotation);
        }
        private void Update()
        {
            _instance.transform.position = transform.position + Position;
            _instance.transform.localRotation = Quaternion.Euler(Rotation);
        }

    }
}