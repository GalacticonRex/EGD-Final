using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ArtifactScreen : MonoBehaviour
    {
        public GameObject TextBox;
        public UnityEngine.UI.Text TextTarget;

        private ObjectLog _obj_log;
        private string _target_data;
        private InterfaceMenu _menus;
        private Coroutine _process;

        public void OnShow()
        {
            _target_data = "";
            TextTarget.text = "";
        }

        public void AddArtifact(Artifact a)
        {
            GameObject go = _obj_log.Push();
            ArtifactUIItem artf = go.GetComponent<ArtifactUIItem>();
            artf.Item = a;
            UnityEngine.UI.Button but = go.GetComponent<UnityEngine.UI.Button>();
            but.onClick.AddListener(artf.Select);
        }
        public void Push(ArtifactUIItem item)
        {
            if (_process != null)
            {
                StopCoroutine(_process);
                _process = null;
            }
            _target_data = item.Item.Source();
            _process = StartCoroutine(ShowArtifact());
        }

        private IEnumerator ShowArtifact()
        {
            TextBox.SetActive(true);
            TextTarget.text = "";
            foreach (char c in _target_data)
            {
                yield return new WaitForSecondsRealtime(0.03f);
                TextTarget.text += c;
            }
            _process = null;
        }

        public void Init()
        {
            Start();
        }

        private void Start()
        {
            if (_obj_log != null)
                return;

            _menus = FindObjectOfType<InterfaceMenu>();
            _obj_log = GetComponentInChildren<ObjectLog>();
        }
        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.ArtifactViewing)
                return;

            Time.timeScale = 0.0f;
            if (Input.GetKeyDown(KeyCode.Space) && _process != null)
            {
                TextTarget.text = _target_data;
                StopCoroutine(_process);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _menus.GoTo(InterfaceMenu.MenuType.Regular);
            }
        }
    }
}