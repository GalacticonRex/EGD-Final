using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TutorialManager : MonoBehaviour
    {
        private static int _current_index = 0;

        public GameObject TutorialBox;
        public UnityEngine.UI.Text TutorialText;
        public UnityEngine.UI.Button Skip;
        public UnityEngine.UI.Button Next;
        public UnityEngine.UI.Button Close;

        private Player _player;
        private TutorialItem[] _tutorial_items;
        private bool block = false;

        private IEnumerator Wait(int index, float time)
        {
            yield return new WaitForSeconds(time);
            FocusItem(index);
        }

        public void FocusItem(int index)
        {
            if (index >= _tutorial_items.Length) {
                CloseTutorial();
                return;
            }

            TutorialItem item = _tutorial_items[index];
            _current_index = index;

            item.OnEnter.Invoke();

            if ( item.TextToShow != null && item.TextToShow.Length != 0 )
            {
                TutorialBox.SetActive(true);
                TutorialText.text = item.TextToShow;
            }
            else
            {
                TutorialBox.SetActive(false);
            }

            if (item.Target != null)
            {
                _player.cameraSystem.SetTarget(item.Target, item.TransitionTime);
            }
            if ( item.Duration == -1 )
            {
                Next.gameObject.SetActive(true);
            }
            else
            {
                Next.gameObject.SetActive(false);
                StartCoroutine(Wait(index + 1, item.Duration));
            }
        }

        public void SkipItem()
        {
            StopAllCoroutines();
            _current_index++;
            while (_current_index < _tutorial_items.Length && !_tutorial_items[_current_index].Breakpoint)
            {
                _current_index++;
            }
            FocusItem(_current_index);
        }
        public void NextItem()
        {
            FocusItem(_current_index + 1);
        }
        public void CloseTutorial()
        {
            StopAllCoroutines();
            _player.cameraSystem.SetTarget(_player.cameraSystem.PlayerRear, 2.0f);
            TutorialBox.SetActive(false);
            _current_index = int.MaxValue;
        }
        public void OpenTutorial()
        {
            if ( !Stargun.StargunIsReady() )
                FocusItem(_current_index);
        }

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            _tutorial_items = GetComponents<TutorialItem>();
        }
        private void Update()
        {

        }
    }
}