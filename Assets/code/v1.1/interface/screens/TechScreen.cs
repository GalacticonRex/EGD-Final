using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class TechScreen : MonoBehaviour
    {
        public ModelImage Draggable;
        public TechModelViewers Viewers;
        public ObjectGrid ObjGrid;

        private string _target_data;
        private InterfaceMenu _menus;
        private Coroutine _process;

        public void AddTech(TechPiece tech)
        {
            ObjGrid.PushElement(0, tech);
        }
        public void RemoveTech(TechPiece tech)
        {
            int i = ObjGrid.GetIndexOfMounted(tech);
            ObjGrid.PopElement(i);
        }

        public void PassToDraggable(TechPiece tech)
        {
            if (tech == null)
                return;

            Draggable.gameObject.SetActive(true);

            ModelViewer view;
            if ((view = Viewers.GetModelView(tech.Name())) != null)
            {
                Draggable.SetSource(view);
            }
        }

        private void Awake()
        {
            _menus = FindObjectOfType<InterfaceMenu>();
            Draggable.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (_menus.CurrentMenu != InterfaceMenu.MenuType.TechRecombination)
                return;

            Time.timeScale = 0.0f;
        }
    }
}