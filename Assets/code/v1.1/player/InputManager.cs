using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LastStar
{
    public class InputManager : MonoBehaviour
    {
        private bool _hit_ui;
        private GameObject _hit;
        private Vector3 _hit_location;

        public bool ui { get { return _hit_ui; } }
        public GameObject hit { get { return _hit; } }
        public Vector3 location { get { return _hit_location; } }

        // Use this for initialization
        private void Start()
        {
            _hit_ui = false;
            _hit = null;
            _hit_location = new Vector3();
        }

        private void Update()
        {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData ptr = new PointerEventData(EventSystem.current);
            ptr.position = Input.mousePosition;
            EventSystem.current.RaycastAll(ptr, results);

            if (results.Count > 0)
            {
                print("Hit UI!");
                _hit_ui = true;
                _hit = results[0].gameObject;
                _hit_location = results[0].worldPosition;
            }
            else
            {
                _hit_ui = false;
                RaycastHit hit;
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(r, out hit))
                {
                    print("Hit Object!");
                    _hit = hit.collider.gameObject;
                    _hit_location = hit.point;
                }
                else
                {
                    print("Hit Nothing!");
                    _hit = null;
                    _hit_location = new Vector3();
                }
            }
        }
    }
}