using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class CaptionText : MonoBehaviour
    {
        public float Radius;
        public UnityEngine.UI.Image PanelUI;
        public UnityEngine.UI.Text TextUI;

        private Player _player;
        private Camera _camera;

        public string Text
        {
            get { return TextUI.text; }
            set { TextUI.text = value; }
        }

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            _camera = _player.GetComponentInChildren<Camera>();
        }
        private void Update()
        {
            float distance = Vector3.Distance(_player.transform.position, transform.position);
            float hidedist = Vector3.Distance(_camera.transform.position, transform.position);

            float alpha = (hidedist < 2 * Radius + 2 * _camera.nearClipPlane) ? 0.0f : (_player.selector.MaxDistance - distance) / _player.selector.MaxDistance;

            Vector3 dir = transform.parent.position - _camera.transform.position;

            transform.position = transform.parent.position - dir.normalized * Radius;
            transform.rotation = Quaternion.LookRotation(_camera.transform.forward, _camera.transform.up);

            Vector3 scale = transform.parent.lossyScale;
            float scalex = dir.magnitude / scale.x / 1000.0f;
            transform.localScale = new Vector3(scalex, scalex, 1.0f);

            Color temp;

            temp = PanelUI.color;
            temp.a = 0.5f * _player.cameraSystem.interpolation * alpha;
            PanelUI.color = temp;

            temp = TextUI.color;
            temp.a = _player.cameraSystem.interpolation * alpha;
            TextUI.color = temp;
        }
    }
}