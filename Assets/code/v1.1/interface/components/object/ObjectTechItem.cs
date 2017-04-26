using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar
{
    public class ObjectTechItem : MonoBehaviour
    {
        public ModelImage Image;
        private TechScreen _tech_screen;
        private TechModelViewers _viewers;

        public void SetTechItem()
        {
            ObjectGridItem item = GetComponent<ObjectGridItem>();
            TechPiece tech = item.Mounted as TechPiece;
            if (tech == null)
                Image.SetSource(null);
            else
            {
                ModelViewer view;
                if ((view = _viewers.GetModelView(tech.Name())) != null)
                    Image.SetSource(view);
                else
                    Image.SetSource(null);
            }
        }

        public void RemoveFromGrid()
        {
            ObjectGridItem item = GetComponent<ObjectGridItem>();
            TechPiece tech = item.ParentGrid.PopElement(item.Index) as TechPiece;
            _tech_screen.PassToDraggable(tech);
        }

        private void Awake()
        {
            _viewers = FindObjectOfType<TechModelViewers>();

            UnityEngine.UI.Button but = GetComponent<UnityEngine.UI.Button>();
            but.onClick.AddListener(RemoveFromGrid);

            ObjectGridItem item = GetComponent<ObjectGridItem>();
            item.OnMountChange.AddListener(SetTechItem);
        }
        private void Start()
        {
            _tech_screen = GetComponentInParent<TechScreen>();
        }
    }
}